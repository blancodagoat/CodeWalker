using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.Extractor;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExtractForm());
        }, "Launching Extractor");
    }
}
