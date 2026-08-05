using CodeWalker.GameFiles;
using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.Peds;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PedsForm());
            GTAFolder.UpdateSettings();
        }, "Launching Ped Viewer");
    }
}
