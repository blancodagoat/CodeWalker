using CodeWalker.GameFiles;
using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.RPFExplorer;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExploreForm());
            GTAFolder.UpdateSettings();
        }, "Launching RPF Explorer");
    }
}
