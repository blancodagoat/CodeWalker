using CodeWalker.GameFiles;
using System.Windows;

namespace CodeWalker.ParticleEditorWpf
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            GTAFolder.UpdateSettings();
            base.OnExit(e);
        }
    }
}
