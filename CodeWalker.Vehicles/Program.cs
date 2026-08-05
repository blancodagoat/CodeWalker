using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.Vehicles;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VehicleForm());
            GTAFolder.UpdateSettings();
        }, "Launching Vehicle Viewer");
    }
}
