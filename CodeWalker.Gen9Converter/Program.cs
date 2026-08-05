using CodeWalker.Tools;
using CodeWalker.Utils;
using System;
using System.Windows.Forms;

namespace CodeWalker.Gen9Converter;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SessionLog.Run(args, () =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConvertAssetsForm());
        }, "Launching Gen9 Converter");
    }
}
