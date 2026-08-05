using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.ErrorReport;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ReportForm());
        }, "Launching Error Report Tool");
    }
}
