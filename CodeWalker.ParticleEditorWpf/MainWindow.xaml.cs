using CodeWalker.Forms;
using CodeWalker.ParticleEditorWpf.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using WF = System.Windows.Forms;

namespace CodeWalker.ParticleEditorWpf
{
    public partial class MainWindow : Window
    {
        private readonly ParticleEditorViewModel vm = new ParticleEditorViewModel();
        private ParticleViewportHost viewport;
        private DispatcherTimer transportTimer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // embed the DirectX viewport (a borderless child WinForms Form)
            viewport = new ParticleViewportHost
            {
                TopLevel = false,
                Dock = WF.DockStyle.Fill,
            };
            ViewportHost.Child = viewport;
            viewport.Show();
            vm.AttachHost(viewport);

            // spacebar toggles play/pause when the (WinForms) viewport has focus
            viewport.PlayPauseRequested += (s, ev) => Dispatcher.BeginInvoke(new Action(TogglePlayPause));

            // WPF-native property editor + keyframe timeline editor
            PropertyPanel.Attach(vm);
            KeyframeEditor.Attach(vm);

            // push selection changes into the editors
            vm.PropertyChanged += Vm_PropertyChanged;

            // transport readout timer (also advances the timeline playhead)
            transportTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            transportTimer.Tick += (s, ev) => { vm.UpdateTransportReadout(); KeyframeEditor.TickPlayhead(); };
            transportTimer.Start();
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParticleEditorViewModel.SelectedObject))
            {
                PropertyPanel?.SetTarget(vm.SelectedObject);
                KeyframeEditor?.SetTarget(vm.SelectedObject);
            }
        }

        private void EffectsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            vm.SelectedNode = e.NewValue as TreeNodeVM;
        }

        // spacebar toggles play/pause - but not while typing in a text field
        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Space) return;
            if (System.Windows.Input.Keyboard.FocusedElement is System.Windows.Controls.TextBox) return;
            TogglePlayPause();
            e.Handled = true;
        }

        private void TogglePlayPause()
        {
            if (vm.PlayPauseCommand?.CanExecute(null) == true) vm.PlayPauseCommand.Execute(null);
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e) => Close();

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            transportTimer?.Stop();
            try { viewport?.Close(); } catch { }
        }
    }
}
