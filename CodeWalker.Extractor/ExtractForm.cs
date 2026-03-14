using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeWalker.GameFiles;

namespace CodeWalker.Extractor
{
    public partial class ExtractForm : Form
    {
        public ExtractForm()
        {
            InitializeComponent();
        }

        private void SelectFolder(TextBox tb)
        {
            FolderBrowserDialog.SelectedPath = tb.Text;
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tb.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void InputFolderBrowseButton_Click(object sender, EventArgs e)
        {
            SelectFolder(InputFolderTextBox);
        }

        private void OutputFolderBrowseButton_Click(object sender, EventArgs e)
        {
            SelectFolder(OutputFolderTextBox);
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            LogTextBox.Clear();

            string gtaFolder = InputFolderTextBox.Text?.Replace('/', '\\');
            string outputFolder = OutputFolderTextBox.Text?.Replace('/', '\\');

            if (string.IsNullOrEmpty(gtaFolder) || string.IsNullOrEmpty(outputFolder))
            {
                MessageBox.Show("Please select both input and output folders.");
                return;
            }

            if (!Directory.Exists(gtaFolder))
            {
                MessageBox.Show("Input folder does not exist.");
                return;
            }

            bool extractMeta = MetaCheckbox.Checked;
            bool extractCarcols = CarcolsCheckbox.Checked;
            bool extractAudio = AudioCheckbox.Checked;
            bool extractLang = LangCheckbox.Checked;
            bool extractTextures = TexturesCheckbox.Checked;
            bool extractModels = ModelsCheckbox.Checked;
            bool extractYmaps = YmapsCheckbox.Checked;
            bool convertRpf = ConvertRPFCheckbox.Checked;
            bool preserveStructure = PreserveFolderStructureCheckbox.Checked;

            if (!extractMeta && !extractCarcols && !extractAudio && !extractLang && !extractTextures && !extractModels && !extractYmaps && !convertRpf)
            {
                MessageBox.Show("Please select at least one file type to extract or an RPF option.");
                return;
            }

            SetUIEnabled(false);

            Task.Run(() => ExtractFiles(gtaFolder, outputFolder, extractMeta, extractCarcols, extractAudio, extractLang, extractTextures, extractModels, extractYmaps, convertRpf, preserveStructure));
        }

        private void SetUIEnabled(bool enabled)
        {
            BeginInvoke(new Action(() =>
            {
                InputFolderTextBox.Enabled = enabled;
                InputFolderBrowseButton.Enabled = enabled;
                OutputFolderTextBox.Enabled = enabled;
                OutputFolderBrowseButton.Enabled = enabled;
                MetaCheckbox.Enabled = enabled;
                CarcolsCheckbox.Enabled = enabled;
                AudioCheckbox.Enabled = enabled;
                LangCheckbox.Enabled = enabled;
                TexturesCheckbox.Enabled = enabled;
                ModelsCheckbox.Enabled = enabled;
                YmapsCheckbox.Enabled = enabled;
                ConvertRPFCheckbox.Enabled = enabled;
                PreserveFolderStructureCheckbox.Enabled = enabled;
                SelectAllCheckbox.Enabled = enabled;
                ExtractButton.Enabled = enabled;
                ExtractProgressBar.Visible = !enabled;
            }));
        }

        private void ExtractFiles(string gtaFolder, string outputFolder, bool extractMeta, bool extractCarcols, bool extractAudio, bool extractLang, bool extractTextures, bool extractModels, bool extractYmaps, bool convertRpf, bool preserveStructure)
        {
            if (extractMeta || extractCarcols || extractAudio || extractLang || extractTextures || extractModels || extractYmaps)
            {
                ExtractFileTypes(gtaFolder, outputFolder, extractMeta, extractCarcols, extractAudio, extractLang, extractTextures, extractModels, extractYmaps, convertRpf, preserveStructure);
            }

            SetUIEnabled(true);
        }

        private void ConvertSingleRPF(RpfFile rpf, string relPath)
        {
            if (rpf.Encryption == RpfEncryption.NG || rpf.Encryption == RpfEncryption.AES)
            {
                Log($"Converting {relPath} from {rpf.Encryption} to OPEN...");
                RpfFile.SetEncryptionType(rpf, RpfEncryption.OPEN);
                RpfFile.Defragment(rpf, null, true);
                Log($"  Converted and defragmented");
            }
        }

        private void ExtractFileTypes(string gtaFolder, string outputFolder, bool extractMeta, bool extractCarcols, bool extractAudio, bool extractLang, bool extractTextures, bool extractModels, bool extractYmaps, bool convertRpf, bool preserveStructure)
        {
            Log("Scanning RPF files...");
            var manager = new RpfManager();
            manager.Init(gtaFolder, false,
                msg => Log($"  {msg}"),
                err => Log($"  ERROR: {err}"),
                false, false);

            Log($"Found {manager.AllRpfs.Count} RPF files");
            Log("");

            if (convertRpf)
            {
                Log("Converting NG encrypted RPFs to OPEN...");
                int convertedCount = 0;
                foreach (var rpf in manager.AllRpfs)
                {
                    if (rpf.Encryption == RpfEncryption.NG || rpf.Encryption == RpfEncryption.AES)
                    {
                        try
                        {
                            ConvertSingleRPF(rpf, rpf.Path);
                            convertedCount++;
                        }
                        catch (Exception ex)
                        {
                            Log($"  ERROR converting {rpf.Path}: {ex.Message}");
                        }
                    }
                }
                Log($"Converted {convertedCount} RPF files to OPEN encryption");
                Log("");
            }

            var stats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var extractedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int totalFiles = 0;
            int processedFiles = 0;

            foreach (var rpf in manager.AllRpfs)
            {
                if (rpf.AllEntries == null) continue;
                foreach (var entry in rpf.AllEntries)
                {
                    if (entry is RpfFileEntry) totalFiles++;
                }
            }

            Log($"Total files to process: {totalFiles}");
            Log("Extracting...");

            string GetUniquePath(string baseFolder, string filename)
            {
                string ext = Path.GetExtension(filename);
                string nameWithoutExt = Path.GetFileNameWithoutExtension(filename);
                string path = Path.Combine(baseFolder, filename);

                if (!extractedPaths.Contains(path))
                {
                    extractedPaths.Add(path);
                    return path;
                }

                int counter = 1;
                while (true)
                {
                    string newPath = Path.Combine(baseFolder, $"{nameWithoutExt}_{counter}{ext}");
                    if (!extractedPaths.Contains(newPath))
                    {
                        extractedPaths.Add(newPath);
                        return newPath;
                    }
                    counter++;
                }
            }

            void IncrementStat(string key)
            {
                if (!stats.ContainsKey(key)) stats[key] = 0;
                stats[key]++;
            }

            foreach (var rpf in manager.AllRpfs)
            {
                if (rpf.AllEntries == null) continue;

                foreach (var entry in rpf.AllEntries)
                {
                    if (entry is not RpfFileEntry fileEntry) continue;

                    string name = fileEntry.Name;
                    string nameLower = name.ToLowerInvariant();

                    try
                    {
                        bool shouldExtract = false;
                        string? targetFolder = null;

                        if (extractMeta && nameLower.EndsWith(".meta"))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "meta");
                        }
                        else if (extractCarcols && (nameLower.Contains("carcols") || nameLower.Contains("carvariations") || nameLower.Contains("carmodcols")))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "carcols");
                        }
                        else if (extractLang && nameLower.EndsWith(".gxt2"))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "languages");
                        }
                        else if (extractAudio && (nameLower.EndsWith(".rel") || nameLower.EndsWith(".awc")))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "audio");
                        }
                        else if (extractTextures && nameLower.EndsWith(".ytd"))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "textures");
                        }
                        else if (extractModels && (nameLower.EndsWith(".ydr") || nameLower.EndsWith(".yft") || nameLower.EndsWith(".ydd")))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "models");
                        }
                        else if (extractYmaps && nameLower.EndsWith(".ymap"))
                        {
                            shouldExtract = true;
                            targetFolder = Path.Combine(outputFolder, "ymaps");
                        }

                        if (shouldExtract && targetFolder != null)
                        {
                            byte[]? data = rpf.ExtractFile(fileEntry);
                            if (data != null)
                            {
                                string fullPath;
                                if (preserveStructure)
                                {
                                    string relativePath = fileEntry.Path;
                                    fullPath = Path.Combine(outputFolder, relativePath);
                                }
                                else
                                {
                                    fullPath = GetUniquePath(targetFolder, name);
                                }
                                
                                string? fullDir = Path.GetDirectoryName(fullPath);
                                if (fullDir != null)
                                {
                                    Directory.CreateDirectory(fullDir);
                                }
                                File.WriteAllBytes(fullPath, data);
                                IncrementStat(preserveStructure ? Path.GetDirectoryName(fullPath) ?? targetFolder : targetFolder);
                            }
                        }
                    }
                    catch
                    {
                        // Skip files that fail to extract
                    }

                    processedFiles++;
                    if (processedFiles % 1000 == 0)
                    {
                        int progress = (int)((processedFiles / (double)totalFiles) * 1000);
                        UpdateProgress(progress);
                    }
                }
            }

            UpdateProgress(1000);
            Log("");
            Log("Extraction complete!");
            Log("");

            foreach (var kvp in stats.OrderByDescending(k => k.Value))
            {
                string folderName = Path.GetFileName(kvp.Key);
                Log($"{folderName}: {kvp.Value} files");
            }

            SetUIEnabled(true);
        }

        private void Log(string message)
        {
            BeginInvoke(new Action(() =>
            {
                LogTextBox.AppendText(message + "\r\n");
                LogTextBox.ScrollToCaret();
            }));
        }

        private void UpdateProgress(int value)
        {
            BeginInvoke(new Action(() =>
            {
                ExtractProgressBar.Value = Math.Max(0, Math.Min(value, 1000));
            }));
        }

        private void SelectAllCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            bool check = SelectAllCheckbox.Checked;
            MetaCheckbox.Checked = check;
            CarcolsCheckbox.Checked = check;
            AudioCheckbox.Checked = check;
            LangCheckbox.Checked = check;
            TexturesCheckbox.Checked = check;
            ModelsCheckbox.Checked = check;
            YmapsCheckbox.Checked = check;
        }
    }
}
