using CodeWalker.GameFiles;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWalker.Project.Panels
{
    public partial class EditYndNodePanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public YndNode CurrentPathNode { get; set; }
        public YndLink CurrentPathLink { get; set; }
        public YndFile CurrentYndFile { get; set; }

        private bool populatingui = false;

        public EditYndNodePanel(ProjectForm projectForm)
        {
            ProjectForm = projectForm;
            InitializeComponent();
        }

        public void SetPathNode(YndNode node)
        {
            CurrentPathNode = node;
            CurrentPathLink = null;
            CurrentYndFile = node?.Ynd;
            Tag = node;
            UpdateFormTitle();
            UpdateYndNodeUI();
        }

        private void UpdateFormTitle()
        {
            var sn = CurrentPathNode.StreetName.Hash == 0 ? "Path node" : CurrentPathNode?.StreetName.ToString() ?? string.Empty;
            Text = sn + " " + CurrentPathNode.NodeID.ToString();
        }

        public void UpdateYndNodeUI()
        {
            LoadNodeTab();
            LoadJunctionTab();
            LoadLinkTab();
        }

        // ==================================================
        // Node Tab
        // ==================================================

        private void LoadNodeTab()
        {
            CurrentPathLink = null;

            if (CurrentPathNode == null)
            {
                NodeDeleteButton.Enabled = false;
                NodeAddToProjectButton.Enabled = false;
                NodeAreaIDUpDown.Value = 0;
                NodeNodeIDUpDown.Value = 0;
                NodePositionTextBox.Text = string.Empty;
                NodeStreetHashTextBox.Text = string.Empty;
                NodeStreetNameLabel.Text = "Name: [None]";
                UpdateNodeFlagsControls();
                UpdateRawFlagsDisplay();
                return;
            }

            populatingui = true;

            var n = CurrentPathNode.RawData;
            NodeDeleteButton.Enabled = ProjectForm.YndExistsInProject(CurrentYndFile);
            NodeAddToProjectButton.Enabled = !NodeDeleteButton.Enabled;

            NodeAreaIDUpDown.Value = n.AreaID;
            NodeNodeIDUpDown.Value = n.NodeID;
            NodePositionTextBox.Text = FloatUtil.GetVector3String(CurrentPathNode.Position);

            var streetname = GlobalText.TryGetString(n.StreetName.Hash);
            NodeStreetHashTextBox.Text = n.StreetName.Hash.ToString();
            NodeStreetNameLabel.Text = "Name: " + ((n.StreetName.Hash == 0) ? "[None]" : (string.IsNullOrEmpty(streetname) ? "[Not found]" : streetname));

            // Populate combo boxes once
            if (NodeSpeedComboBox.Items.Count == 0)
                NodeSpeedComboBox.Items.AddRange(Enum.GetValues(typeof(YndNodeSpeed)).Cast<object>().ToArray());
            if (NodeSpecialComboBox.Items.Count == 0)
                NodeSpecialComboBox.Items.AddRange(Enum.GetValues(typeof(YndNodeSpecialType)).Cast<object>().ToArray());

            UpdateNodeFlagsControls();
            UpdateRawFlagsDisplay();


            NodeEnableDisableButton.Text = CurrentPathNode.IsDisabledUnk0 ? "Enable Section" : "Disable Section";

            populatingui = false;

            if (ProjectForm.WorldForm != null)
                ProjectForm.WorldForm.SelectObject(CurrentPathNode);
        }

        private void UpdateNodeFlagsControls()
        {
            var n = CurrentPathNode;

            // Flags0 booleans
            NodeFloodGroupUpDown.Value = n?.FloodGroup ?? 0;
            NodeOffRoadCheckBox.Checked = n?.OffRoad ?? false;
            NodeNoBigVehiclesCheckBox.Checked = n?.NoBigVehicles ?? false;
            NodeCannotGoRightCheckBox.Checked = n?.CannotGoRight ?? false;
            NodeCannotGoLeftCheckBox.Checked = n?.CannotGoLeft ?? false;
            NodeSlipRoadCheckBox.Checked = n?.SlipRoad ?? false;
            NodeIndicateKeepLeftCheckBox.Checked = n?.IndicateKeepLeft ?? false;
            NodeIndicateKeepRightCheckBox.Checked = n?.IndicateKeepRight ?? false;

            // Flags1 booleans
            NodeNoGpsCheckBox.Checked = n?.NoGps ?? false;
            NodeIsJunctionCheckBox.Checked = n?.IsJunction ?? false;
            NodeSwitchedOffOriginalCheckBox.Checked = n?.IsDisabledUnk1 ?? false;
            NodeWaterNodeCheckBox.Checked = n?.WaterNode ?? false;
            NodeHighwayCheckBox.Checked = n?.Highway ?? false;
            NodeSwitchedOffCheckBox.Checked = n?.IsDisabledUnk0 ?? false;
            NodeQualifiesAsJunctionCheckBox.Checked = n?.QualifiesAsJunction ?? false;
            NodeTunnelCheckBox.Checked = n?.Tunnel ?? false;
            NodeLeftOnlyCheckBox.Checked = n?.LeftOnly ?? false;

            // Numeric values
            NodeHeuristicUpDown.Value = n?.HeuristicValue ?? 0;
            NodeDensityUpDown.Value = n?.Density ?? 0;
            NodeDeadEndnessUpDown.Value = n?.DeadEndness ?? 0;

            // Special + Speed
            NodeSpecialComboBox.SelectedItem = n?.Special ?? YndNodeSpecialType.None;
            NodeIsPedNodeCheckBox.Checked = n?.IsPedNode ?? false;
            NodeSpeedComboBox.SelectedItem = n?.Speed ?? (YndNodeSpeed)(-1);


        }

        private void UpdateRawFlagsDisplay()
        {
            var n = CurrentPathNode;
            uint f0 = n?.Flags0 ?? 0;
            uint f1 = n?.Flags1 ?? 0;
            NodeFlags0HexLabel.Text = "0x" + f0.ToString("X8");
            NodeFlags1HexLabel.Text = "0x" + f1.ToString("X8");
            NodeFlags0UpDown.Value = f0;
            NodeFlags1UpDown.Value = f1;
        }

        private void ApplyNodeFlagsFromControls()
        {
            if (populatingui) return;
            if (CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                // Flags0 named properties
                CurrentPathNode.FloodGroup = (int)NodeFloodGroupUpDown.Value;
                CurrentPathNode.OffRoad = NodeOffRoadCheckBox.Checked;
                CurrentPathNode.NoBigVehicles = NodeNoBigVehiclesCheckBox.Checked;
                CurrentPathNode.CannotGoRight = NodeCannotGoRightCheckBox.Checked;
                CurrentPathNode.CannotGoLeft = NodeCannotGoLeftCheckBox.Checked;
                CurrentPathNode.SlipRoad = NodeSlipRoadCheckBox.Checked;
                CurrentPathNode.IndicateKeepLeft = NodeIndicateKeepLeftCheckBox.Checked;
                CurrentPathNode.IndicateKeepRight = NodeIndicateKeepRightCheckBox.Checked;

                // Flags1 named properties
                CurrentPathNode.NoGps = NodeNoGpsCheckBox.Checked;
                CurrentPathNode.IsJunction = NodeIsJunctionCheckBox.Checked;
                CurrentPathNode.IsDisabledUnk1 = NodeSwitchedOffOriginalCheckBox.Checked;
                CurrentPathNode.WaterNode = NodeWaterNodeCheckBox.Checked;
                CurrentPathNode.Highway = NodeHighwayCheckBox.Checked;
                CurrentPathNode.IsDisabledUnk0 = NodeSwitchedOffCheckBox.Checked;
                CurrentPathNode.QualifiesAsJunction = NodeQualifiesAsJunctionCheckBox.Checked;
                CurrentPathNode.Tunnel = NodeTunnelCheckBox.Checked;
                CurrentPathNode.LeftOnly = NodeLeftOnlyCheckBox.Checked;

                // Numeric flag values
                CurrentPathNode.HeuristicValue = (int)NodeHeuristicUpDown.Value;
                CurrentPathNode.Density = (int)NodeDensityUpDown.Value;
                CurrentPathNode.DeadEndness = (int)NodeDeadEndnessUpDown.Value;

                ProjectForm.SetYndHasChanged(true);

                // Allow partner nodes to recheck junction status
                if (CurrentPathNode.Links != null)
                {
                    foreach (var link in CurrentPathNode.Links)
                        link.Node2?.CheckIfJunction();
                }
            }

            populatingui = true;
            UpdateRawFlagsDisplay();
            populatingui = false;
        }

        private void ApplyNodeFlagsFromRaw()
        {
            if (populatingui) return;
            if (CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentPathNode.Flags0 = (uint)NodeFlags0UpDown.Value;
                CurrentPathNode.Flags1 = (uint)NodeFlags1UpDown.Value;
                ProjectForm.SetYndHasChanged(true);
            }

            populatingui = true;
            UpdateNodeFlagsControls();
            NodeFlags0HexLabel.Text = "0x" + CurrentPathNode.Flags0.ToString("X8");
            NodeFlags1HexLabel.Text = "0x" + CurrentPathNode.Flags1.ToString("X8");
            populatingui = false;
        }

        private static uint SetBit(uint value, int bit, bool set)
        {
            uint mask = 1u << bit;
            return set ? (value | mask) : (value & ~mask);
        }

        // ==================================================
        // Link Tab
        // ==================================================

        private void LoadLinkTab()
        {
            // Populate link list
            populatingui = true;
            NodeLinkCountLabel.Text = "Link Count: " + (CurrentPathNode?.LinkCount ?? 0);
            NodeLinksListBox.Items.Clear();
            if (CurrentPathNode?.Links != null)
            {
                foreach (var link in CurrentPathNode.Links)
                    NodeLinksListBox.Items.Add(link);
            }
            if (CurrentPathLink != null)
            {
                NodeLinksListBox.SelectedItem = CurrentPathLink;
            }
            populatingui = false;

            if (CurrentPathLink == null)
            {
                LinkPanel.Enabled = false;
                LinkAreaIDUpDown.Value = 0;
                LinkNodeIDUpDown.Value = 0;
                LinkDistanceUpDown.Value = 0;
                LinkStatusLabel.Text = "";
                UpdateLinkFlagsControls();
                UpdateLinkRawDisplay();
                return;
            }

            populatingui = true;
            LinkPanel.Enabled = true;

            LinkAreaIDUpDown.Value = CurrentPathLink.Node2?.AreaID ?? 0;
            LinkNodeIDUpDown.Value = CurrentPathLink.Node2?.NodeID ?? 0;
            LinkDistanceUpDown.Value = CurrentPathLink.Distance;
            LinkStatusLabel.Text = "";

            UpdateLinkFlagsControls();
            UpdateLinkRawDisplay();

            populatingui = false;

            if (ProjectForm.WorldForm != null)
                ProjectForm.WorldForm.SelectObject(CurrentPathLink);
        }

        private void UpdateLinkFlagsControls()
        {
            var l = CurrentPathLink;
            LinkGpsBothWaysCheckBox.Checked = l?.GpsBothWays ?? false;
            LinkNarrowRoadCheckBox.Checked = l?.NarrowRoad ?? false;
            LinkDontUseForNavCheckBox.Checked = l?.DontUseForNavigation ?? false;
            LinkShortcutCheckBox.Checked = l?.Shortcut ?? false;
            LinkNegativeOffsetCheckBox.Checked = l?.NegativeOffset ?? false;
            LinkOffsetUpDown.Value = l?.OffsetValue ?? 0;
            LinkFwdLanesUpDown.Value = l?.LaneCountForward ?? 0;
            LinkBackLanesUpDown.Value = l?.LaneCountBackward ?? 0;
        }

        private void UpdateLinkRawDisplay()
        {
            uint f = CurrentPathLink?.Flags0 ?? 0;
            LinkFlags0HexLabel.Text = "0x" + f.ToString("X8");
            LinkFlags0UpDown.Value = f;
        }

        private void ApplyLinkFlagsFromControls()
        {
            if (populatingui) return;
            if (CurrentPathLink == null) return;

            bool updgfx = false;
            lock (ProjectForm.ProjectSyncRoot)
            {
                // Read-modify-write Flags0 for read-only properties
                uint f = CurrentPathLink.Flags0;
                f = SetBit(f, 0, LinkGpsBothWaysCheckBox.Checked);      // GpsBothWays
                f = SetBit(f, 9, LinkNarrowRoadCheckBox.Checked);       // NarrowRoad
                f = SetBit(f, 15, LinkNegativeOffsetCheckBox.Checked);  // NegativeOffset
                f = SetBit(f, 16, LinkDontUseForNavCheckBox.Checked);   // DontUseForNavigation
                // OffsetValue: bits 12-14
                f = (f & ~(0x7u << 12)) | (((uint)LinkOffsetUpDown.Value & 0x7u) << 12);
                CurrentPathLink.Flags0 = f;

                bool shortcutChanged = CurrentPathLink.Shortcut != LinkShortcutCheckBox.Checked;
                CurrentPathLink.Shortcut = LinkShortcutCheckBox.Checked;

                int fwd = (int)LinkFwdLanesUpDown.Value;
                if (fwd != CurrentPathLink.LaneCountForward)
                {
                    CurrentPathLink.SetForwardLanesBidirectionally(fwd);
                    updgfx = true;
                }

                int back = (int)LinkBackLanesUpDown.Value;
                if (back != CurrentPathLink.LaneCountBackward)
                {
                    CurrentPathLink.SetBackwardLanesBidirectionally(back);
                    updgfx = true;
                }

                if (shortcutChanged) updgfx = true;

                ProjectForm.SetYndHasChanged(true);
            }

            populatingui = true;
            UpdateLinkRawDisplay();
            populatingui = false;

            if (updgfx && ProjectForm.WorldForm != null && CurrentYndFile != null)
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
        }

        private void ApplyLinkFlagsFromRaw()
        {
            if (populatingui) return;
            if (CurrentPathLink == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentPathLink.Flags0 = (uint)LinkFlags0UpDown.Value;
                ProjectForm.SetYndHasChanged(true);
            }

            populatingui = true;
            UpdateLinkFlagsControls();
            LinkFlags0HexLabel.Text = "0x" + CurrentPathLink.Flags0.ToString("X8");
            populatingui = false;

            if (ProjectForm.WorldForm != null && CurrentYndFile != null)
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
        }

        // ==================================================
        // Junction Tab
        // ==================================================

        private void LoadJunctionTab()
        {
            var junc = CurrentPathNode?.Junction;
            if (junc == null)
            {
                JunctionEnableCheckBox.Checked = false;
                JunctionPanel.Enabled = false;
                JunctionMaxZUpDown.Value = 0;
                JunctionMinZUpDown.Value = 0;
                JunctionPosXUpDown.Value = 0;
                JunctionPosYUpDown.Value = 0;
                JunctionDimXUpDown.Value = 1;
                JunctionDimYUpDown.Value = 1;
                JunctionHeightmapTextBox.Text = string.Empty;
                return;
            }

            populatingui = true;
            JunctionEnableCheckBox.Checked = CurrentPathNode.HasJunction;
            JunctionPanel.Enabled = JunctionEnableCheckBox.Checked;
            JunctionMaxZUpDown.Value = (decimal)junc.MaxZ / 32;
            JunctionMinZUpDown.Value = (decimal)junc.MinZ / 32;
            JunctionPosXUpDown.Value = (decimal)junc.PositionX / 4;
            JunctionPosYUpDown.Value = (decimal)junc.PositionY / 4;
            JunctionDimXUpDown.Value = junc.Heightmap.CountX;
            JunctionDimYUpDown.Value = junc.Heightmap.CountY;
            JunctionHeightmapTextBox.Text = junc.Heightmap?.GetDataString() ?? "";
            populatingui = false;
        }

        // ==================================================
        // Link helpers
        // ==================================================

        private void AddPathLink()
        {
            if (CurrentPathNode == null) return;

            var l = CurrentPathNode.AddLink();
            CurrentPathLink = l;
            LoadLinkTab();
            NodeLinksListBox.SelectedItem = l;

            if (ProjectForm.WorldForm != null)
                ProjectForm.WorldForm.UpdatePathNodeGraphics(CurrentPathNode, false);
        }

        private void RemovePathLink()
        {
            if (CurrentPathLink == null || CurrentPathNode == null) return;

            var partners = CurrentPathLink.Node2.Links.Where(l => l.Node2 == CurrentPathNode);
            foreach (var partner in partners)
                partner.Node1.RemoveLink(partner);

            if (!CurrentPathNode.RemoveLink(CurrentPathLink)) return;

            CurrentPathLink = null;
            LoadLinkTab();

            if (ProjectForm.WorldForm != null)
                ProjectForm.WorldForm.UpdatePathNodeGraphics(CurrentPathNode, false);
        }

        private void UpdatePathNodeLinkage()
        {
            if (CurrentPathLink == null || CurrentYndFile == null) return;

            YndNode linknode = null;
            ushort areaid = CurrentPathLink._RawData.AreaID;
            ushort nodeid = CurrentPathLink._RawData.NodeID;

            if (areaid == CurrentYndFile.AreaID)
            {
                if (CurrentYndFile.Nodes != null && nodeid < CurrentYndFile.Nodes.Length)
                    linknode = CurrentYndFile.Nodes[nodeid];
            }
            else
            {
                if (ProjectForm.WorldForm != null)
                    linknode = ProjectForm.WorldForm.GetPathNodeFromSpace(areaid, nodeid);
            }

            if (linknode == null)
                LinkStatusLabel.Text = "Unable to find node " + areaid + ":" + nodeid + ".";
            else
                LinkStatusLabel.Text = "";

            var partner = CurrentPathLink.Node2.Links.FirstOrDefault(l => l.Node2 == CurrentPathNode);
            partner?.Node1.RemoveLink(partner);

            CurrentPathLink.Node2 = linknode;
            CurrentPathLink.UpdateLength();
            var l2 = linknode?.AddLink(CurrentPathNode);

            if (l2 != null && partner != null)
                l2.CopyFlags(partner);

            if (ProjectForm.WorldForm != null)
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
        }

        // ==================================================
        // Node Info event handlers
        // ==================================================

        private void NodeAreaIDUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;
            ushort areaid = (ushort)NodeAreaIDUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.AreaID != areaid)
                {
                    CurrentPathNode.AreaID = areaid;
                    ProjectForm.SetYndHasChanged(true);
                }
            }
            ProjectForm.ProjectExplorer?.UpdatePathNodeTreeNode(CurrentPathNode);
        }

        private void NodeNodeIDUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;
            ushort nodeid = (ushort)NodeNodeIDUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.NodeID != nodeid)
                {
                    CurrentPathNode.NodeID = nodeid;
                    ProjectForm.SetYndHasChanged(true);
                }
            }
            ProjectForm.ProjectExplorer?.UpdatePathNodeTreeNode(CurrentPathNode);
        }

        // GTA V path-node grid constants: 32x32 grid with each cell 512 units
        // (corner at -8192,-8192). AreaID = CellY * 32 + CellX. See YndFile.AreaID
        // and SpaceNodeGrid in CodeWalker.Core for the authoritative formula.
        private const float YndGridCornerX = -8192.0f;
        private const float YndGridCornerY = -8192.0f;
        private const float YndGridCellSize = 512.0f;
        private const int YndGridCellCountX = 32;
        private const int YndGridCellCountY = 32;

        private static int GetAreaIdForPosition(Vector3 position)
        {
            int cellX = (int)Math.Floor((position.X - YndGridCornerX) / YndGridCellSize);
            int cellY = (int)Math.Floor((position.Y - YndGridCornerY) / YndGridCellSize);
            if (cellX < 0 || cellX >= YndGridCellCountX) return -1;
            if (cellY < 0 || cellY >= YndGridCellCountY) return -1;
            return cellY * YndGridCellCountX + cellX;
        }

        /// <summary>
        /// Makes sure a YND file exists for the area containing <paramref name="newPosition"/>.
        /// If the node is staying inside its current ynd, this is a no-op. If the node is
        /// crossing into an area without a ynd yet, the user is prompted to create one.
        /// Returns false when the move should be aborted (user cancelled, out-of-bounds, etc).
        /// </summary>
        private bool EnsureYndForPosition(Vector3 newPosition)
        {
            if (CurrentPathNode == null) return false;
            if (ProjectForm?.WorldForm == null) return true; // no space => let caller no-op

            int newAreaID = GetAreaIdForPosition(newPosition);
            if (newAreaID < 0)
            {
                MessageBox.Show(
                    "The new position is outside the GTA V path-node grid (-8192..8192). The move will be cancelled.",
                    "Position out of range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // Same area => nothing to do.
            if (CurrentPathNode.Ynd != null && CurrentPathNode.Ynd.AreaID == newAreaID)
            {
                return true;
            }

            var targetYnd = ProjectForm.FindOrCreateYndForArea(newAreaID, promptUser: true);
            return targetYnd != null;
        }

        /// <summary>
        /// Public entry point that mirrors the algorithm used internally for auto-migration:
        /// verifies the node's current position is consistent with its owning ynd, and if
        /// not, arranges for it to be migrated to the correct area file (prompting to create
        /// a new ynd when needed).
        /// </summary>
        public void CheckNodeAreaAfterMove(YndNode node)
        {
            if (node == null) return;
            if (ProjectForm?.WorldForm == null) return;

            int newAreaID = GetAreaIdForPosition(node.Position);
            if (newAreaID < 0) return;
            if (node.Ynd != null && node.Ynd.AreaID == newAreaID) return;

            var targetYnd = ProjectForm.FindOrCreateYndForArea(newAreaID, promptUser: true);
            if (targetYnd == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                node.SetYndNodePosition(ProjectForm.WorldForm.Space, node.Position, out var affectedFiles);
                if (affectedFiles != null)
                {
                    foreach (var af in affectedFiles)
                    {
                        if (af == null) continue;
                        ProjectForm.AddYndToProject(af);
                        ProjectForm.SetYndHasChanged(af, true);
                    }
                }
                ProjectForm.SetYndHasChanged(true);
            }
        }

        private void NodePositionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;
            Vector3 v = FloatUtil.ParseVector3String(NodePositionTextBox.Text);
            bool change = false;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Position != v)
                {
                    // Ensure a YND exists for the area the node is being moved into.
                    // If the user declines (or it's outside the map), abort the move
                    // instead of letting SetYndNodePosition silently revert.
                    if (!EnsureYndForPosition(v))
                    {
                        // Refresh the textbox back to the node's current position so
                        // the UI doesn't show a phantom value.
                        populatingui = true;
                        NodePositionTextBox.Text = FloatUtil.GetVector3String(CurrentPathNode.Position);
                        populatingui = false;
                        return;
                    }

                    CurrentPathNode.SetYndNodePosition(ProjectForm.WorldForm.Space, v, out var affectedFiles);
                    foreach (var affectedFile in affectedFiles)
                    {
                        ProjectForm.AddYndToProject(affectedFile);
                        ProjectForm.SetYndHasChanged(affectedFile, true);
                    }

                    // Node's owning Ynd may have changed -- keep the panel's
                    // CurrentYndFile in sync with reality.
                    CurrentYndFile = CurrentPathNode.Ynd;
                    if (CurrentYndFile != null)
                    {
                        ProjectForm.AddYndToProject(CurrentYndFile);
                    }

                    ProjectForm.SetYndHasChanged(true);
                    change = true;
                }
            }
            if (change && ProjectForm.WorldForm != null)
            {
                ProjectForm.WorldForm.SetWidgetPosition(CurrentPathNode.Position);
                ProjectForm.WorldForm.UpdatePathNodeGraphics(CurrentPathNode, false);
            }
        }

        private void NodeStreetHashTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;
            uint.TryParse(NodeStreetHashTextBox.Text, out uint hash);
            var streetname = GlobalText.TryGetString(hash);
            NodeStreetNameLabel.Text = "Name: " + ((hash == 0) ? "[None]" : (string.IsNullOrEmpty(streetname) ? "[Not found]" : streetname));

            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.StreetName.Hash != hash)
                {
                    CurrentPathNode.StreetName = hash;
                    ProjectForm.SetYndHasChanged(true);
                }
            }
            ProjectForm.ProjectExplorer?.UpdatePathNodeTreeNode(CurrentPathNode);
        }

        private void NodeGoToButton_Click(object sender, EventArgs e)
        {
            if (CurrentPathNode == null || ProjectForm.WorldForm == null) return;
            ProjectForm.WorldForm.GoToPosition(CurrentPathNode.Position);
        }

        private void NodeAddToProjectButton_Click(object sender, EventArgs e)
        {
            if (CurrentPathNode?.Ynd != null)
                ProjectForm.AddYndToProject(CurrentPathNode.Ynd);
        }

        private void NodeDeleteButton_Click(object sender, EventArgs e)
        {
            ProjectForm.SetProjectItem(CurrentPathNode);
            ProjectForm.DeletePathNode();
        }

        // ==================================================
        // Node Flag event handlers
        // ==================================================

        private void NodeFlagCheckBox_Changed(object sender, EventArgs e)
        {
            ApplyNodeFlagsFromControls();
        }

        private void NodeValueUpDown_Changed(object sender, EventArgs e)
        {
            ApplyNodeFlagsFromControls();
        }

        private void NodeFlags0Raw_ValueChanged(object sender, EventArgs e)
        {
            ApplyNodeFlagsFromRaw();
        }

        private void NodeFlags1Raw_ValueChanged(object sender, EventArgs e)
        {
            ApplyNodeFlagsFromRaw();
        }

        private void NodeSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                var speed = (YndNodeSpeed)NodeSpeedComboBox.SelectedItem;
                if (CurrentPathNode.Speed != speed)
                {
                    CurrentPathNode.Speed = speed;
                    ProjectForm.SetYndHasChanged(true);
                    populatingui = true;
                    UpdateRawFlagsDisplay();
                    populatingui = false;
                    ProjectForm.WorldForm?.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
        }

        private void NodeSpecialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                var special = (YndNodeSpecialType)NodeSpecialComboBox.SelectedItem;
                if (CurrentPathNode.Special != special)
                {
                    var isPedNode = CurrentPathNode.IsPedNode;
                    bool specialIsPedNode = YndNode.IsSpecialTypeAPedNode(special);
                    if (isPedNode != specialIsPedNode)
                    {
                        var res = MessageBox.Show(
                            specialIsPedNode
                                ? "This will change this node from a vehicle node to a ped node. This will remove all links. Continue?"
                                : "This will change this node from a ped node to a vehicle node. This will remove all links. Continue?",
                            "Are you sure?",
                            MessageBoxButtons.YesNo
                        );

                        if (res == DialogResult.No)
                        {
                            NodeSpecialComboBox.SelectedItem = CurrentPathNode.Special;
                            return;
                        }

                        if (ProjectForm != null)
                        {
                            CurrentPathNode.RemoveYndLinksForNode(ProjectForm.WorldForm.Space, out var affectedFiles);
                            ProjectForm.AddYndToProject(CurrentYndFile);
                            ProjectForm.WorldForm?.UpdatePathYndGraphics(CurrentYndFile, false);
                            foreach (var file in affectedFiles)
                            {
                                ProjectForm.AddYndToProject(file);
                                ProjectForm.WorldForm?.UpdatePathYndGraphics(file, false);
                                ProjectForm.SetYndHasChanged(file, true);
                            }
                        }
                    }

                    CurrentPathNode.Special = special;
                    NodeIsPedNodeCheckBox.Checked = CurrentPathNode.IsPedNode;
                    populatingui = true;
                    UpdateRawFlagsDisplay();
                    populatingui = false;
                    ProjectForm.SetYndHasChanged(true);
                }
            }
        }

        // ==================================================
        // Utility button handlers
        // ==================================================

        private void NodeFloodCopyButton_Click(object sender, EventArgs e)
        {
            if (CurrentPathNode == null) return;

            CurrentPathNode.FloodCopyFlags(out var affectedFiles);

            ProjectForm.AddYndToProject(CurrentYndFile);
            ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);

            foreach (var affectedFile in affectedFiles)
            {
                ProjectForm.AddYndToProject(affectedFile);
                ProjectForm.SetYndHasChanged(affectedFile, true);
                ProjectForm.WorldForm.UpdatePathYndGraphics(affectedFile, false);
            }
        }

        private void NodeEnableDisableButton_Click(object sender, EventArgs e)
        {
            if (CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentPathNode.IsDisabledUnk0 = !CurrentPathNode.IsDisabledUnk0;
                CurrentPathNode.IsDisabledUnk1 = CurrentPathNode.IsDisabledUnk0;
                CurrentPathNode.FloodCopyFlags(out var affectedFiles);

                NodeEnableDisableButton.Text = CurrentPathNode.IsDisabledUnk0 ? "Enable Section" : "Disable Section";

                ProjectForm.AddYndToProject(CurrentYndFile);
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);

                foreach (var affectedFile in affectedFiles)
                {
                    ProjectForm.AddYndToProject(affectedFile);
                    ProjectForm.SetYndHasChanged(affectedFile, true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(affectedFile, false);
                }
            }

            populatingui = true;
            UpdateNodeFlagsControls();
            UpdateRawFlagsDisplay();
            populatingui = false;
        }

        // ==================================================
        // Links list event handlers
        // ==================================================

        private void NodeLinksListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            CurrentPathLink = NodeLinksListBox.SelectedItem as YndLink;
            LoadLinkTab();
        }

        private void NodeAddLinkButton_Click(object sender, EventArgs e)
        {
            AddPathLink();
        }

        private void NodeRemoveLinkButton_Click(object sender, EventArgs e)
        {
            RemovePathLink();
        }

        // ==================================================
        // Link Tab event handlers
        // ==================================================

        private void LinkAreaIDUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathLink == null) return;
            ushort areaid = (ushort)LinkAreaIDUpDown.Value;
            bool change = false;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathLink._RawData.AreaID != areaid)
                {
                    CurrentPathLink._RawData.AreaID = areaid;
                    ProjectForm.SetYndHasChanged(true);
                    change = true;
                }
            }
            if (change)
            {
                UpdatePathNodeLinkage();
                NodeLinksListBox.Items[NodeLinksListBox.SelectedIndex] = NodeLinksListBox.SelectedItem;
            }
        }

        private void LinkNodeIDUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathLink == null) return;
            ushort nodeid = (ushort)LinkNodeIDUpDown.Value;
            bool change = false;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathLink._RawData.NodeID != nodeid)
                {
                    CurrentPathLink._RawData.NodeID = nodeid;
                    ProjectForm.SetYndHasChanged(true);
                    change = true;
                }
            }
            if (change)
            {
                UpdatePathNodeLinkage();
                NodeLinksListBox.Items[NodeLinksListBox.SelectedIndex] = NodeLinksListBox.SelectedItem;
            }
        }

        private void LinkFlagCheckBox_Changed(object sender, EventArgs e)
        {
            ApplyLinkFlagsFromControls();
        }

        private void LinkValueUpDown_Changed(object sender, EventArgs e)
        {
            ApplyLinkFlagsFromControls();
        }

        private void LinkDistanceUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathLink == null) return;
            byte length = (byte)LinkDistanceUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathLink.Distance != length)
                {
                    CurrentPathLink.Distance = length;
                    ProjectForm.SetYndHasChanged(true);
                }
            }
        }

        private void LinkFlags0Raw_ValueChanged(object sender, EventArgs e)
        {
            ApplyLinkFlagsFromRaw();
        }

        private void LinkSelectPartnerButton_Click(object sender, EventArgs e)
        {
            if (CurrentPathLink == null) return;

            var partner = CurrentPathLink.Node2.Links.FirstOrDefault(l => l.Node2 == CurrentPathNode);
            if (partner == null)
            {
                MessageBox.Show("Could not find partner!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CurrentPathNode = partner.Node1;
            CurrentPathLink = partner;
            LoadNodeTab();
            LoadLinkTab();
            NodeLinksListBox.SelectedItem = partner;
        }

        // ==================================================
        // Junction event handlers
        // ==================================================

        private void JunctionEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode == null) return;

            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.HasJunction != JunctionEnableCheckBox.Checked)
                {
                    CurrentPathNode.HasJunction = JunctionEnableCheckBox.Checked;
                    if (CurrentPathNode.HasJunction && CurrentPathNode.Junction == null)
                    {
                        var j = new YndJunction();
                        j._RawData.HeightmapDimX = 16;
                        j._RawData.HeightmapDimY = 16;
                        j.MaxZ = (short)(CurrentPathNode.Position.Z * 32 + 32);
                        j.MinZ = (short)(CurrentPathNode.Position.Z * 32 - 32);
                        j.PositionX = (short)(CurrentPathNode.Position.X * 4f - j.RawData.HeightmapDimY * 4f);
                        j.PositionY = (short)(CurrentPathNode.Position.Y * 4f - j.RawData.HeightmapDimY * 4f);
                        j.Heightmap = new YndJunctionHeightmap(Enumerable.Repeat((byte)255, j._RawData.HeightmapDimX * j._RawData.HeightmapDimY).ToArray(), j);
                        j.RefData = new NodeJunctionRef() { AreaID = (ushort)CurrentPathNode.AreaID, NodeID = (ushort)CurrentPathNode.NodeID };
                        CurrentPathNode.Junction = j;
                    }
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
            LoadJunctionTab();
        }

        private void JunctionMaxZUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            short val = (short)(JunctionMaxZUpDown.Value * 32);
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction.MaxZ != val)
                {
                    CurrentPathNode.Junction.MaxZ = val;
                    CurrentPathNode.Junction._RawData.MaxZ = val;
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
        }

        private void JunctionMinZUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            short val = (short)(JunctionMinZUpDown.Value * 32);
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction.MinZ != val)
                {
                    CurrentPathNode.Junction.MinZ = val;
                    CurrentPathNode.Junction._RawData.MinZ = val;
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
        }

        private void JunctionPosXUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            short val = (short)(JunctionPosXUpDown.Value * 4);
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction.PositionX != val)
                {
                    CurrentPathNode.Junction.PositionX = val;
                    CurrentPathNode.Junction._RawData.PositionX = val;
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
        }

        private void JunctionPosYUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            short val = (short)(JunctionPosYUpDown.Value * 4);
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction.PositionY != val)
                {
                    CurrentPathNode.Junction.PositionY = val;
                    CurrentPathNode.Junction._RawData.PositionY = val;
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
        }

        private void JunctionDimXUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            byte val = (byte)JunctionDimXUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction._RawData.HeightmapDimX != val)
                {
                    CurrentPathNode.Junction._RawData.HeightmapDimX = val;
                    CurrentPathNode.Junction.ResizeHeightmap();
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
            LoadJunctionTab();
        }

        private void JunctionDimYUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            byte val = (byte)JunctionDimYUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                if (CurrentPathNode.Junction._RawData.HeightmapDimY != val)
                {
                    CurrentPathNode.Junction._RawData.HeightmapDimY = val;
                    CurrentPathNode.Junction.ResizeHeightmap();
                    ProjectForm.SetYndHasChanged(true);
                    ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
                }
            }
            LoadJunctionTab();
        }

        private void JunctionHeightmapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentPathNode.Junction.SetHeightmap(JunctionHeightmapTextBox.Text);
                ProjectForm.SetYndHasChanged(true);
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
            }
        }

        private void JunctionGenerateButton_Click(object sender, EventArgs e)
        {
            if (populatingui || CurrentPathNode?.Junction == null) return;
            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentPathNode.GenerateYndNodeJunctionHeightMap(ProjectForm.WorldForm.Space);
                ProjectForm.SetYndHasChanged(true);
                ProjectForm.WorldForm.UpdatePathYndGraphics(CurrentYndFile, false);
            }
            LoadJunctionTab();
        }

        private void lblLinkFwdLanes_Click(object sender, EventArgs e)
        {

        }
    }
}
