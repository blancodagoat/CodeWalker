using CodeWalker.GameFiles;
using CodeWalker.Properties;
using CodeWalker.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWalker.Tools
{
    public partial class ShrinkTexForm : Form
    {
        private volatile bool InProgress = false;
        private volatile bool AbortOperation = false;

        public ShrinkTexForm()
        {
            InitializeComponent();
            MaxSizeComboBox.SelectedIndex = 1; // 2048
            OutputFolderRadio.Checked = true;
        }

        private void ShrinkTexForm_Load(object sender, EventArgs e)
        {
            try
            {
                // needed to open encrypted rpf archives; loose folders work without keys
                GTA5Keys.LoadFromPath(GTAFolder.CurrentGTAFolder, GTAFolder.IsGen9, Settings.Default.Key);
            }
            catch
            {
                Log("GTA keys not loaded - encrypted rpf archives will not open, loose folders still work.");
            }
        }

        private void InputFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = InputTextBox.Text;
            if (FolderBrowserDialog.ShowDialogNew() == DialogResult.OK)
            {
                InputTextBox.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void InputRpfBrowseButton_Click(object sender, EventArgs e)
        {
            if (OpenRpfDialog.ShowDialog() == DialogResult.OK)
            {
                InputTextBox.Text = OpenRpfDialog.FileName;
            }
        }

        private void OutputBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = OutputTextBox.Text;
            if (FolderBrowserDialog.ShowDialogNew() == DialogResult.OK)
            {
                OutputTextBox.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void UpdateOutputPath(object sender, EventArgs e)
        {
            var t = InputTextBox.Text.TrimEnd('\\', '/');
            if (t.Length == 0) return;
            if (t.EndsWith(".rpf", StringComparison.OrdinalIgnoreCase)) t = t.Substring(0, t.Length - 4);
            OutputTextBox.Text = t + (OutputRpfRadio.Checked ? "_shrunk.rpf" : "_shrunk");
        }

        private void Log(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { Log(text); }));
                }
                else
                {
                    LogTextBox.AppendText(text + Environment.NewLine);
                }
            }
            catch { }
        }

        private void ShrinkButton_Click(object sender, EventArgs e)
        {
            if (InProgress) return;

            var input = InputTextBox.Text.TrimEnd('\\', '/');
            var output = OutputTextBox.Text.TrimEnd('\\', '/');
            bool outputRpf = OutputRpfRadio.Checked;
            bool genLods = GenLodsCheckBox.Checked;
            if (!Directory.Exists(input) && !File.Exists(input))
            {
                MessageBox.Show("Input doesn't exist: " + input);
                return;
            }
            if (string.Equals(input, output, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Output must be different from the input.");
                return;
            }
            int cap = int.Parse(MaxSizeComboBox.Text);

            InProgress = true;
            AbortOperation = false;
            LogTextBox.Clear();
            Log($"Shrinking {input} (max {cap}px) -> {output}");

            Task.Run(() =>
            {
                try
                {
                    var stats = PackShrinker.ShrinkPack(input, output, cap, outputRpf, Log, () => AbortOperation, genLods);
                    if ((stats != null) && !outputRpf)
                    {
                        Log("The folder is laid out for texoverride: drop its contents into tex_overrides.");
                    }
                }
                catch (Exception ex)
                {
                    Log("Error: " + ex.Message);
                }
                InProgress = false;
            });
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            AbortOperation = true;
        }
    }
}
