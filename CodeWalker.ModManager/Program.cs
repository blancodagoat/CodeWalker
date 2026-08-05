using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.ModManager;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModManagerForm());
        }, "Launching Mod Manager");
    }
}
