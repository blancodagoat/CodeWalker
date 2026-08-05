using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWalker.Utils;

/// <summary>
/// Session lifecycle logging to file and console (when available).
/// </summary>
public static class SessionLog
{
    private const int AttachParentProcess = -1;

    private static readonly object Sync = new();
    private static StreamWriter? _writer;
    private static string? _sessionPath;
    private static bool _consoleEnabled;
    private static bool _initialized;
    private static int _shutdown;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(int dwProcessId);

    public static string? SessionPath => _sessionPath;

    public static void Run(string[]? args, Action body, string? launchLabel = null)
    {
        Initialize(args);

        if (!string.IsNullOrEmpty(launchLabel))
            WriteLine(launchLabel);

        try
        {
            body();
        }
        catch (Exception ex)
        {
            LogException("main", ex);
            MessageBox.Show(
                "An unexpected error was encountered!\n" + ex,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Shutdown("normal");
        }
    }

    public static void Initialize(string[]? args = null)
    {
        lock (Sync)
        {
            if (_initialized)
                return;

            _initialized = true;
            EnableConsole(args);

            string logDir = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logDir);

            string appName = SanitizeFileName(Assembly.GetEntryAssembly()?.GetName().Name ?? "app");
            string stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _sessionPath = Path.Combine(logDir, $"session-{appName}-{stamp}-{Environment.ProcessId}.log");

            var stream = new FileStream(_sessionPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false))
            {
                AutoFlush = true
            };

            RegisterHandlers();

            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            WriteLine("========== SESSION START ==========");
            WriteLine($"App: {Assembly.GetEntryAssembly()?.GetName().Name}");
            WriteLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            WriteLine($"PID: {Environment.ProcessId}");
            WriteLine($"Version: {version}");
            WriteLine($"Exe: {Environment.ProcessPath}");
            WriteLine($"CWD: {Environment.CurrentDirectory}");
            WriteLine($"Args: {(args is { Length: > 0 } ? string.Join(' ', args) : "(none)")}");
            WriteLine($"Console: {(_consoleEnabled ? "yes" : "no")}");
            WriteLine($"Log: {_sessionPath}");
            WriteLine("==================================");
        }
    }

    public static void WriteLine(string message)
    {
        lock (Sync)
        {
            string line = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";

            _writer?.WriteLine(line);

            if (_consoleEnabled)
            {
                try
                {
                    Console.WriteLine(line);
                }
                catch
                {
                    // Console may be unavailable after detach.
                }
            }

            Debug.WriteLine(line);
        }
    }

    public static void LogException(string source, Exception ex)
    {
        WriteLine($"EXCEPTION ({source}): {ex.GetType().Name}: {ex.Message}");
        WriteLine(ex.ToString());
    }

    public static void Shutdown(string reason)
    {
        if (Interlocked.Exchange(ref _shutdown, 1) != 0)
            return;

        lock (Sync)
        {
            WriteLine("==================================");
            WriteLine($"========== SESSION END ({reason}) ==========");
            WriteLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

            try
            {
                _writer?.Flush();
                _writer?.Dispose();
            }
            catch
            {
                // Best effort on shutdown.
            }
            finally
            {
                _writer = null;
            }
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');

        return name.Replace(' ', '_');
    }

    private static void EnableConsole(string[]? args)
    {
        bool wantConsole = args?.Any(a => string.Equals(a, "console", StringComparison.OrdinalIgnoreCase)) == true;

#if DEBUG
        wantConsole = true;
#endif

        if (AttachConsole(AttachParentProcess))
        {
            _consoleEnabled = true;
            return;
        }

        if (wantConsole && AllocConsole())
        {
            _consoleEnabled = true;
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch
            {
                // Non-fatal.
            }
        }
    }

    private static void RegisterHandlers()
    {
        Application.ApplicationExit += (_, _) => Shutdown("normal");
        Application.ThreadException += (_, e) =>
        {
            LogException("UI thread", e.Exception);
            Shutdown("crash");
        };
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                LogException("unhandled", ex);
            else
                WriteLine($"UNHANDLED: {e.ExceptionObject}");

            Shutdown(e.IsTerminating ? "fatal" : "crash");
        };
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Shutdown("process exit");
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            LogException("unobserved task", e.Exception);
            e.SetObserved();
        };

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
    }
}
