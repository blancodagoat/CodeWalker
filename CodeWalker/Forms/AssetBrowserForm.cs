using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeWalker.Forms
{
    /// <summary>
    /// Asset Browser dialog for browsing loaded YTYP archetypes.
    ///
    /// Preview mode: TEXT-ONLY (v1). The right-hand info panel shows archetype
    /// name, asset name, ytyp source, hash, drawable dict, texture dict, clip
    /// dict, bounding box (min/max/center), bounding sphere, LOD distance, type
    /// and flags. A SharpDX viewport rendering the drawable (stretch goal) was
    /// intentionally skipped to keep this change self-contained and avoid
    /// pulling in the ModelForm render pipeline / DXManager wiring.
    /// </summary>
    public partial class AssetBrowserForm : Form
    {
        private readonly GameFileCache GameFileCache;
        private readonly List<Archetype> _allArchetypes = new List<Archetype>();
        private readonly List<Archetype> _filtered = new List<Archetype>();

        /// <summary>
        /// The archetype hash selected by the user, valid after the dialog
        /// closes with DialogResult.OK. Returns MetaHash(0) if nothing was
        /// chosen.
        /// </summary>
        public MetaHash SelectedArchetype { get; private set; }

        /// <summary>
        /// The selected archetype's display name (for convenience so the
        /// caller can set a TextBox without re-looking-up the archetype).
        /// </summary>
        public string SelectedArchetypeName { get; private set; }

        public AssetBrowserForm(GameFileCache cache)
        {
            GameFileCache = cache;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PopulateArchetypes();
            ApplyFilter();
        }

        private void PopulateArchetypes()
        {
            _allArchetypes.Clear();
            if (GameFileCache?.YtypDict == null) return;

            foreach (var kvp in GameFileCache.YtypDict)
            {
                var ytyp = kvp.Value;
                if (ytyp?.AllArchetypes == null) continue;
                foreach (var arch in ytyp.AllArchetypes)
                {
                    if (arch == null) continue;
                    _allArchetypes.Add(arch);
                }
            }

            _allArchetypes.Sort((a, b) =>
            {
                var an = a?.Name ?? string.Empty;
                var bn = b?.Name ?? string.Empty;
                return string.Compare(an, bn, StringComparison.OrdinalIgnoreCase);
            });

            StatusLabel.Text = _allArchetypes.Count + " archetypes loaded";
        }

        private void ApplyFilter()
        {
            _filtered.Clear();
            var filter = FilterTextBox.Text?.Trim() ?? string.Empty;

            uint filterHash = 0;
            bool isHash = uint.TryParse(filter, out filterHash);

            if (string.IsNullOrEmpty(filter))
            {
                _filtered.AddRange(_allArchetypes);
            }
            else
            {
                foreach (var arch in _allArchetypes)
                {
                    var name = arch.Name ?? string.Empty;
                    var asset = arch.AssetName ?? string.Empty;
                    if (name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        asset.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (isHash && arch.Hash.Hash == filterHash))
                    {
                        _filtered.Add(arch);
                    }
                }
            }

            // Cap display for UI responsiveness.
            const int maxDisplay = 5000;
            ArchetypeListBox.BeginUpdate();
            ArchetypeListBox.Items.Clear();
            int count = Math.Min(_filtered.Count, maxDisplay);
            for (int i = 0; i < count; i++)
            {
                ArchetypeListBox.Items.Add(_filtered[i].Name ?? ("#" + _filtered[i].Hash.ToString()));
            }
            ArchetypeListBox.EndUpdate();

            if (_filtered.Count > maxDisplay)
            {
                StatusLabel.Text = _filtered.Count + " matches (showing first " + maxDisplay + ")";
            }
            else
            {
                StatusLabel.Text = _filtered.Count + " / " + _allArchetypes.Count + " archetypes";
            }

            if (ArchetypeListBox.Items.Count > 0)
            {
                ArchetypeListBox.SelectedIndex = 0;
            }
            else
            {
                InfoTextBox.Text = string.Empty;
                OkBtn.Enabled = false;
            }
        }

        private void ShowArchetypeInfo(Archetype arch)
        {
            if (arch == null)
            {
                InfoTextBox.Text = string.Empty;
                OkBtn.Enabled = false;
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Name:         " + (arch.Name ?? string.Empty));
            sb.AppendLine("Asset Name:   " + (arch.AssetName ?? string.Empty));
            sb.AppendLine("Hash:         " + arch.Hash.ToString() + "  (#" + arch.Hash.Hash + ")");
            sb.AppendLine("Type:         " + arch.Type);
            sb.AppendLine("Ytyp:         " + (arch.Ytyp?.RpfFileEntry?.Name ?? arch.Ytyp?.Name ?? "(unknown)"));
            sb.AppendLine();
            sb.AppendLine("Drawable Dict: " + arch.DrawableDict.ToString());
            sb.AppendLine("Texture Dict:  " + arch.TextureDict.ToString());
            sb.AppendLine("Clip Dict:     " + arch.ClipDict.ToString());
            sb.AppendLine();
            sb.AppendLine("BB Min:       " + arch.BBMin);
            sb.AppendLine("BB Max:       " + arch.BBMax);
            sb.AppendLine("BB Size:      " + (arch.BBMax - arch.BBMin));
            sb.AppendLine("BS Center:    " + arch.BSCenter);
            sb.AppendLine("BS Radius:    " + arch.BSRadius.ToString("0.###"));
            sb.AppendLine("LOD Dist:     " + arch.LodDist.ToString("0.###"));
            sb.AppendLine();
            sb.AppendLine("Flags:        0x" + arch._BaseArchetypeDef.flags.ToString("X8") +
                          " (" + arch._BaseArchetypeDef.flags + ")");
            sb.AppendLine("Special Attr: " + arch._BaseArchetypeDef.specialAttribute);

            var ta = arch as TimeArchetype;
            if (ta != null)
            {
                sb.AppendLine();
                sb.AppendLine("TimeArchetype");
                sb.AppendLine("Time Flags:   0x" + ta.TimeFlags.ToString("X8"));
                sb.AppendLine("Extra Flag:   " + ta.ExtraFlag);
            }

            var mlo = arch as MloArchetype;
            if (mlo != null)
            {
                sb.AppendLine();
                sb.AppendLine("MloArchetype");
                sb.AppendLine("Entities:     " + (mlo.entities?.Length ?? 0));
                sb.AppendLine("Rooms:        " + (mlo.rooms?.Length ?? 0));
                sb.AppendLine("Portals:      " + (mlo.portals?.Length ?? 0));
            }

            InfoTextBox.Text = sb.ToString();
            OkBtn.Enabled = true;
        }

        private Archetype GetSelectedArchetype()
        {
            var idx = ArchetypeListBox.SelectedIndex;
            if (idx < 0 || idx >= _filtered.Count) return null;
            return _filtered[idx];
        }

        private void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ArchetypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowArchetypeInfo(GetSelectedArchetype());
        }

        private void ArchetypeListBox_DoubleClick(object sender, EventArgs e)
        {
            if (GetSelectedArchetype() != null)
            {
                AcceptSelection();
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            AcceptSelection();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            SelectedArchetype = new MetaHash(0);
            SelectedArchetypeName = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AcceptSelection()
        {
            var arch = GetSelectedArchetype();
            if (arch == null) return;
            SelectedArchetype = arch.Hash;
            SelectedArchetypeName = arch.Name ?? arch.Hash.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
