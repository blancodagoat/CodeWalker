namespace CodeWalker.Project.Panels
{
    partial class EditYndNodePanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            PathNodeTabControl = new TabControl();
            NodeTabPage = new TabPage();
            NodeInfoGroupBox = new GroupBox();
            lblAreaID = new Label();
            NodeAreaIDUpDown = new NumericUpDown();
            lblNodeID = new Label();
            lblSpeed = new Label();
            NodeNodeIDUpDown = new NumericUpDown();
            NodeSpeedComboBox = new ComboBox();
            lblPosition = new Label();
            NodePositionTextBox = new TextBox();
            lblStreetHash = new Label();
            NodeStreetHashTextBox = new TextBox();
            NodeStreetNameLabel = new Label();
            NodeGoToButton = new Button();
            NodeAddToProjectButton = new Button();
            NodeDeleteButton = new Button();
            Flags0GroupBox = new GroupBox();
            NodeOffRoadCheckBox = new CheckBox();
            lblFloodGroup = new Label();
            NodeFloodGroupUpDown = new NumericUpDown();
            NodeNoBigVehiclesCheckBox = new CheckBox();
            NodeCannotGoRightCheckBox = new CheckBox();
            NodeCannotGoLeftCheckBox = new CheckBox();
            NodeSlipRoadCheckBox = new CheckBox();
            NodeIndicateKeepLeftCheckBox = new CheckBox();
            NodeIndicateKeepRightCheckBox = new CheckBox();
            lblSpecial = new Label();
            NodeSpecialComboBox = new ComboBox();
            NodeIsPedNodeCheckBox = new CheckBox();
            lblRawFlags0 = new Label();
            NodeFlags0HexLabel = new Label();
            NodeFlags0UpDown = new NumericUpDown();
            Flags1GroupBox = new GroupBox();
            NodeNoGpsCheckBox = new CheckBox();
            NodeIsJunctionCheckBox = new CheckBox();
            NodeSwitchedOffCheckBox = new CheckBox();
            NodeSwitchedOffOriginalCheckBox = new CheckBox();
            NodeWaterNodeCheckBox = new CheckBox();
            NodeHighwayCheckBox = new CheckBox();
            NodeQualifiesAsJunctionCheckBox = new CheckBox();
            NodeTunnelCheckBox = new CheckBox();
            NodeLeftOnlyCheckBox = new CheckBox();
            lblHeuristic = new Label();
            NodeHeuristicUpDown = new NumericUpDown();
            lblDensity = new Label();
            NodeDensityUpDown = new NumericUpDown();
            lblDeadEndness = new Label();
            NodeDeadEndnessUpDown = new NumericUpDown();
            lblRawFlags1 = new Label();
            NodeFlags1HexLabel = new Label();
            NodeFlags1UpDown = new NumericUpDown();
            NodeFloodCopyButton = new Button();
            NodeEnableDisableButton = new Button();
            LinkTabPage = new TabPage();
            NodeLinkCountLabel = new Label();
            NodeLinksListBox = new ListBox();
            NodeAddLinkButton = new Button();
            NodeRemoveLinkButton = new Button();
            LinkPanel = new Panel();
            LinkTargetGroupBox = new GroupBox();
            lblLinkAreaID = new Label();
            LinkAreaIDUpDown = new NumericUpDown();
            lblLinkNodeID = new Label();
            LinkNodeIDUpDown = new NumericUpDown();
            LinkFlagsGroupBox = new GroupBox();
            LinkGpsBothWaysCheckBox = new CheckBox();
            LinkShortcutCheckBox = new CheckBox();
            LinkNarrowRoadCheckBox = new CheckBox();
            LinkDontUseForNavCheckBox = new CheckBox();
            LinkNegativeOffsetCheckBox = new CheckBox();
            lblLinkOffset = new Label();
            LinkOffsetUpDown = new NumericUpDown();
            lblLinkFwdLanes = new Label();
            LinkFwdLanesUpDown = new NumericUpDown();
            lblLinkBackLanes = new Label();
            LinkBackLanesUpDown = new NumericUpDown();
            lblLinkDistance = new Label();
            LinkDistanceUpDown = new NumericUpDown();
            lblLinkRawFlags = new Label();
            LinkFlags0HexLabel = new Label();
            LinkFlags0UpDown = new NumericUpDown();
            LinkSelectPartnerButton = new Button();
            LinkStatusLabel = new Label();
            JunctionTabPage = new TabPage();
            JunctionEnableCheckBox = new CheckBox();
            JunctionPanel = new Panel();
            lblJuncMaxZ = new Label();
            JunctionMaxZUpDown = new NumericUpDown();
            lblJuncMinZ = new Label();
            JunctionMinZUpDown = new NumericUpDown();
            lblJuncPosX = new Label();
            JunctionPosXUpDown = new NumericUpDown();
            lblJuncPosY = new Label();
            JunctionPosYUpDown = new NumericUpDown();
            lblJuncDimX = new Label();
            JunctionDimXUpDown = new NumericUpDown();
            lblJuncDimY = new Label();
            JunctionDimYUpDown = new NumericUpDown();
            lblJuncHeightmap = new Label();
            JunctionHeightmapTextBox = new TextBox();
            JunctionGenerateButton = new Button();
            PathNodeTabControl.SuspendLayout();
            NodeTabPage.SuspendLayout();
            NodeInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NodeAreaIDUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NodeNodeIDUpDown).BeginInit();
            Flags0GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NodeFloodGroupUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NodeFlags0UpDown).BeginInit();
            Flags1GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NodeHeuristicUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NodeDensityUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NodeDeadEndnessUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NodeFlags1UpDown).BeginInit();
            LinkTabPage.SuspendLayout();
            LinkPanel.SuspendLayout();
            LinkTargetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LinkAreaIDUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LinkNodeIDUpDown).BeginInit();
            LinkFlagsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LinkOffsetUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LinkFwdLanesUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LinkBackLanesUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LinkDistanceUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LinkFlags0UpDown).BeginInit();
            JunctionTabPage.SuspendLayout();
            JunctionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)JunctionMaxZUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)JunctionMinZUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)JunctionPosXUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)JunctionPosYUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)JunctionDimXUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)JunctionDimYUpDown).BeginInit();
            SuspendLayout();
            // 
            // PathNodeTabControl
            // 
            PathNodeTabControl.Controls.Add(NodeTabPage);
            PathNodeTabControl.Controls.Add(LinkTabPage);
            PathNodeTabControl.Controls.Add(JunctionTabPage);
            PathNodeTabControl.Dock = DockStyle.Fill;
            PathNodeTabControl.Location = new System.Drawing.Point(0, 0);
            PathNodeTabControl.Margin = new Padding(4, 3, 4, 3);
            PathNodeTabControl.Name = "PathNodeTabControl";
            PathNodeTabControl.SelectedIndex = 0;
            PathNodeTabControl.Size = new System.Drawing.Size(578, 587);
            PathNodeTabControl.TabIndex = 0;
            // 
            // NodeTabPage
            // 
            NodeTabPage.AutoScroll = true;
            NodeTabPage.Controls.Add(NodeInfoGroupBox);
            NodeTabPage.Controls.Add(Flags0GroupBox);
            NodeTabPage.Controls.Add(Flags1GroupBox);
            NodeTabPage.Controls.Add(NodeFloodCopyButton);
            NodeTabPage.Controls.Add(NodeEnableDisableButton);
            NodeTabPage.Location = new System.Drawing.Point(4, 24);
            NodeTabPage.Margin = new Padding(4, 3, 4, 3);
            NodeTabPage.Name = "NodeTabPage";
            NodeTabPage.Padding = new Padding(4, 3, 4, 3);
            NodeTabPage.Size = new System.Drawing.Size(570, 559);
            NodeTabPage.TabIndex = 0;
            NodeTabPage.Text = "Node";
            // 
            // NodeInfoGroupBox
            // 
            NodeInfoGroupBox.Controls.Add(lblAreaID);
            NodeInfoGroupBox.Controls.Add(NodeAreaIDUpDown);
            NodeInfoGroupBox.Controls.Add(lblNodeID);
            NodeInfoGroupBox.Controls.Add(lblSpeed);
            NodeInfoGroupBox.Controls.Add(NodeNodeIDUpDown);
            NodeInfoGroupBox.Controls.Add(NodeSpeedComboBox);
            NodeInfoGroupBox.Controls.Add(lblPosition);
            NodeInfoGroupBox.Controls.Add(NodePositionTextBox);
            NodeInfoGroupBox.Controls.Add(lblStreetHash);
            NodeInfoGroupBox.Controls.Add(NodeStreetHashTextBox);
            NodeInfoGroupBox.Controls.Add(NodeStreetNameLabel);
            NodeInfoGroupBox.Controls.Add(NodeGoToButton);
            NodeInfoGroupBox.Controls.Add(NodeAddToProjectButton);
            NodeInfoGroupBox.Controls.Add(NodeDeleteButton);
            NodeInfoGroupBox.Location = new System.Drawing.Point(7, 7);
            NodeInfoGroupBox.Margin = new Padding(4, 3, 4, 3);
            NodeInfoGroupBox.Name = "NodeInfoGroupBox";
            NodeInfoGroupBox.Padding = new Padding(4, 3, 4, 3);
            NodeInfoGroupBox.Size = new System.Drawing.Size(555, 122);
            NodeInfoGroupBox.TabIndex = 0;
            NodeInfoGroupBox.TabStop = false;
            NodeInfoGroupBox.Text = "Node Info";
            // 
            // lblAreaID
            // 
            lblAreaID.AutoSize = true;
            lblAreaID.Location = new System.Drawing.Point(391, 52);
            lblAreaID.Margin = new Padding(4, 0, 4, 0);
            lblAreaID.Name = "lblAreaID";
            lblAreaID.Size = new System.Drawing.Size(48, 15);
            lblAreaID.TabIndex = 0;
            lblAreaID.Text = "Area ID:";
            // 
            // NodeAreaIDUpDown
            // 
            NodeAreaIDUpDown.Location = new System.Drawing.Point(454, 47);
            NodeAreaIDUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeAreaIDUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            NodeAreaIDUpDown.Name = "NodeAreaIDUpDown";
            NodeAreaIDUpDown.Size = new System.Drawing.Size(93, 23);
            NodeAreaIDUpDown.TabIndex = 1;
            NodeAreaIDUpDown.ValueChanged += NodeAreaIDUpDown_ValueChanged;
            // 
            // lblNodeID
            // 
            lblNodeID.AutoSize = true;
            lblNodeID.Location = new System.Drawing.Point(391, 22);
            lblNodeID.Margin = new Padding(4, 0, 4, 0);
            lblNodeID.Name = "lblNodeID";
            lblNodeID.Size = new System.Drawing.Size(53, 15);
            lblNodeID.TabIndex = 2;
            lblNodeID.Text = "Node ID:";
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new System.Drawing.Point(13, 93);
            lblSpeed.Margin = new Padding(4, 0, 4, 0);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new System.Drawing.Size(42, 15);
            lblSpeed.TabIndex = 3;
            lblSpeed.Text = "Speed:";
            // 
            // NodeNodeIDUpDown
            // 
            NodeNodeIDUpDown.Location = new System.Drawing.Point(454, 17);
            NodeNodeIDUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeNodeIDUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            NodeNodeIDUpDown.Name = "NodeNodeIDUpDown";
            NodeNodeIDUpDown.Size = new System.Drawing.Size(93, 23);
            NodeNodeIDUpDown.TabIndex = 3;
            NodeNodeIDUpDown.ValueChanged += NodeNodeIDUpDown_ValueChanged;
            // 
            // NodeSpeedComboBox
            // 
            NodeSpeedComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            NodeSpeedComboBox.FormattingEnabled = true;
            NodeSpeedComboBox.Location = new System.Drawing.Point(97, 90);
            NodeSpeedComboBox.Margin = new Padding(4, 3, 4, 3);
            NodeSpeedComboBox.Name = "NodeSpeedComboBox";
            NodeSpeedComboBox.Size = new System.Drawing.Size(159, 23);
            NodeSpeedComboBox.TabIndex = 4;
            NodeSpeedComboBox.SelectedIndexChanged += NodeSpeedComboBox_SelectedIndexChanged;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new System.Drawing.Point(13, 30);
            lblPosition.Margin = new Padding(4, 0, 4, 0);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new System.Drawing.Size(53, 15);
            lblPosition.TabIndex = 4;
            lblPosition.Text = "Position:";
            // 
            // NodePositionTextBox
            // 
            NodePositionTextBox.Location = new System.Drawing.Point(97, 22);
            NodePositionTextBox.Margin = new Padding(4, 3, 4, 3);
            NodePositionTextBox.Name = "NodePositionTextBox";
            NodePositionTextBox.Size = new System.Drawing.Size(159, 23);
            NodePositionTextBox.TabIndex = 5;
            NodePositionTextBox.TextChanged += NodePositionTextBox_TextChanged;
            // 
            // lblStreetHash
            // 
            lblStreetHash.AutoSize = true;
            lblStreetHash.Location = new System.Drawing.Point(13, 60);
            lblStreetHash.Margin = new Padding(4, 0, 4, 0);
            lblStreetHash.Name = "lblStreetHash";
            lblStreetHash.Size = new System.Drawing.Size(70, 15);
            lblStreetHash.TabIndex = 6;
            lblStreetHash.Text = "Street Hash:";
            // 
            // NodeStreetHashTextBox
            // 
            NodeStreetHashTextBox.Location = new System.Drawing.Point(97, 52);
            NodeStreetHashTextBox.Margin = new Padding(4, 3, 4, 3);
            NodeStreetHashTextBox.Name = "NodeStreetHashTextBox";
            NodeStreetHashTextBox.Size = new System.Drawing.Size(159, 23);
            NodeStreetHashTextBox.TabIndex = 7;
            NodeStreetHashTextBox.TextChanged += NodeStreetHashTextBox_TextChanged;
            // 
            // NodeStreetNameLabel
            // 
            NodeStreetNameLabel.AutoSize = true;
            NodeStreetNameLabel.Location = new System.Drawing.Point(264, 60);
            NodeStreetNameLabel.Margin = new Padding(4, 0, 4, 0);
            NodeStreetNameLabel.Name = "NodeStreetNameLabel";
            NodeStreetNameLabel.Size = new System.Drawing.Size(82, 15);
            NodeStreetNameLabel.TabIndex = 8;
            NodeStreetNameLabel.Text = "Name: [None]";
            // 
            // NodeGoToButton
            // 
            NodeGoToButton.Location = new System.Drawing.Point(264, 22);
            NodeGoToButton.Margin = new Padding(4, 3, 4, 3);
            NodeGoToButton.Name = "NodeGoToButton";
            NodeGoToButton.Size = new System.Drawing.Size(64, 23);
            NodeGoToButton.TabIndex = 9;
            NodeGoToButton.Text = "Go To";
            NodeGoToButton.UseVisualStyleBackColor = true;
            NodeGoToButton.Click += NodeGoToButton_Click;
            // 
            // NodeAddToProjectButton
            // 
            NodeAddToProjectButton.Location = new System.Drawing.Point(358, 86);
            NodeAddToProjectButton.Margin = new Padding(4, 3, 4, 3);
            NodeAddToProjectButton.Name = "NodeAddToProjectButton";
            NodeAddToProjectButton.Size = new System.Drawing.Size(117, 28);
            NodeAddToProjectButton.TabIndex = 10;
            NodeAddToProjectButton.Text = "Add to Project";
            NodeAddToProjectButton.UseVisualStyleBackColor = true;
            NodeAddToProjectButton.Click += NodeAddToProjectButton_Click;
            // 
            // NodeDeleteButton
            // 
            NodeDeleteButton.Location = new System.Drawing.Point(483, 86);
            NodeDeleteButton.Margin = new Padding(4, 3, 4, 3);
            NodeDeleteButton.Name = "NodeDeleteButton";
            NodeDeleteButton.Size = new System.Drawing.Size(64, 28);
            NodeDeleteButton.TabIndex = 11;
            NodeDeleteButton.Text = "Delete";
            NodeDeleteButton.UseVisualStyleBackColor = true;
            NodeDeleteButton.Click += NodeDeleteButton_Click;
            // 
            // Flags0GroupBox
            // 
            Flags0GroupBox.Controls.Add(NodeOffRoadCheckBox);
            Flags0GroupBox.Controls.Add(lblFloodGroup);
            Flags0GroupBox.Controls.Add(NodeFloodGroupUpDown);
            Flags0GroupBox.Controls.Add(NodeNoBigVehiclesCheckBox);
            Flags0GroupBox.Controls.Add(NodeCannotGoRightCheckBox);
            Flags0GroupBox.Controls.Add(NodeCannotGoLeftCheckBox);
            Flags0GroupBox.Controls.Add(NodeSlipRoadCheckBox);
            Flags0GroupBox.Controls.Add(NodeIndicateKeepLeftCheckBox);
            Flags0GroupBox.Controls.Add(NodeIndicateKeepRightCheckBox);
            Flags0GroupBox.Controls.Add(lblSpecial);
            Flags0GroupBox.Controls.Add(NodeSpecialComboBox);
            Flags0GroupBox.Controls.Add(NodeIsPedNodeCheckBox);
            Flags0GroupBox.Controls.Add(lblRawFlags0);
            Flags0GroupBox.Controls.Add(NodeFlags0HexLabel);
            Flags0GroupBox.Controls.Add(NodeFlags0UpDown);
            Flags0GroupBox.Location = new System.Drawing.Point(7, 135);
            Flags0GroupBox.Margin = new Padding(4, 3, 4, 3);
            Flags0GroupBox.Name = "Flags0GroupBox";
            Flags0GroupBox.Padding = new Padding(4, 3, 4, 3);
            Flags0GroupBox.Size = new System.Drawing.Size(555, 135);
            Flags0GroupBox.TabIndex = 1;
            Flags0GroupBox.TabStop = false;
            Flags0GroupBox.Text = "Flags0";
            // 
            // NodeOffRoadCheckBox
            // 
            NodeOffRoadCheckBox.AutoSize = true;
            NodeOffRoadCheckBox.Location = new System.Drawing.Point(9, 18);
            NodeOffRoadCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeOffRoadCheckBox.Name = "NodeOffRoadCheckBox";
            NodeOffRoadCheckBox.Size = new System.Drawing.Size(73, 19);
            NodeOffRoadCheckBox.TabIndex = 0;
            NodeOffRoadCheckBox.Text = "Off Road";
            NodeOffRoadCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // lblFloodGroup
            // 
            lblFloodGroup.AutoSize = true;
            lblFloodGroup.Location = new System.Drawing.Point(415, 83);
            lblFloodGroup.Margin = new Padding(4, 0, 4, 0);
            lblFloodGroup.Name = "lblFloodGroup";
            lblFloodGroup.Size = new System.Drawing.Size(76, 15);
            lblFloodGroup.TabIndex = 14;
            lblFloodGroup.Text = "Flood Group:";
            // 
            // NodeFloodGroupUpDown
            // 
            NodeFloodGroupUpDown.Location = new System.Drawing.Point(497, 80);
            NodeFloodGroupUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeFloodGroupUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            NodeFloodGroupUpDown.Name = "NodeFloodGroupUpDown";
            NodeFloodGroupUpDown.Size = new System.Drawing.Size(50, 23);
            NodeFloodGroupUpDown.TabIndex = 15;
            NodeFloodGroupUpDown.ValueChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeNoBigVehiclesCheckBox
            // 
            NodeNoBigVehiclesCheckBox.AutoSize = true;
            NodeNoBigVehiclesCheckBox.Location = new System.Drawing.Point(140, 84);
            NodeNoBigVehiclesCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeNoBigVehiclesCheckBox.Name = "NodeNoBigVehiclesCheckBox";
            NodeNoBigVehiclesCheckBox.Size = new System.Drawing.Size(107, 19);
            NodeNoBigVehiclesCheckBox.TabIndex = 1;
            NodeNoBigVehiclesCheckBox.Text = "No Big Vehicles";
            NodeNoBigVehiclesCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeCannotGoRightCheckBox
            // 
            NodeCannotGoRightCheckBox.AutoSize = true;
            NodeCannotGoRightCheckBox.Location = new System.Drawing.Point(140, 18);
            NodeCannotGoRightCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeCannotGoRightCheckBox.Name = "NodeCannotGoRightCheckBox";
            NodeCannotGoRightCheckBox.Size = new System.Drawing.Size(114, 19);
            NodeCannotGoRightCheckBox.TabIndex = 2;
            NodeCannotGoRightCheckBox.Text = "Cannot Go Right";
            NodeCannotGoRightCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeCannotGoLeftCheckBox
            // 
            NodeCannotGoLeftCheckBox.AutoSize = true;
            NodeCannotGoLeftCheckBox.Location = new System.Drawing.Point(9, 39);
            NodeCannotGoLeftCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeCannotGoLeftCheckBox.Name = "NodeCannotGoLeftCheckBox";
            NodeCannotGoLeftCheckBox.Size = new System.Drawing.Size(106, 19);
            NodeCannotGoLeftCheckBox.TabIndex = 3;
            NodeCannotGoLeftCheckBox.Text = "Cannot Go Left";
            NodeCannotGoLeftCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeSlipRoadCheckBox
            // 
            NodeSlipRoadCheckBox.AutoSize = true;
            NodeSlipRoadCheckBox.Location = new System.Drawing.Point(9, 84);
            NodeSlipRoadCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeSlipRoadCheckBox.Name = "NodeSlipRoadCheckBox";
            NodeSlipRoadCheckBox.Size = new System.Drawing.Size(75, 19);
            NodeSlipRoadCheckBox.TabIndex = 4;
            NodeSlipRoadCheckBox.Text = "Slip Road";
            NodeSlipRoadCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeIndicateKeepLeftCheckBox
            // 
            NodeIndicateKeepLeftCheckBox.AutoSize = true;
            NodeIndicateKeepLeftCheckBox.Location = new System.Drawing.Point(140, 39);
            NodeIndicateKeepLeftCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeIndicateKeepLeftCheckBox.Name = "NodeIndicateKeepLeftCheckBox";
            NodeIndicateKeepLeftCheckBox.Size = new System.Drawing.Size(120, 19);
            NodeIndicateKeepLeftCheckBox.TabIndex = 5;
            NodeIndicateKeepLeftCheckBox.Text = "Indicate Keep Left";
            NodeIndicateKeepLeftCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeIndicateKeepRightCheckBox
            // 
            NodeIndicateKeepRightCheckBox.AutoSize = true;
            NodeIndicateKeepRightCheckBox.Location = new System.Drawing.Point(9, 60);
            NodeIndicateKeepRightCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeIndicateKeepRightCheckBox.Name = "NodeIndicateKeepRightCheckBox";
            NodeIndicateKeepRightCheckBox.Size = new System.Drawing.Size(128, 19);
            NodeIndicateKeepRightCheckBox.TabIndex = 6;
            NodeIndicateKeepRightCheckBox.Text = "Indicate Keep Right";
            NodeIndicateKeepRightCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // lblSpecial
            // 
            lblSpecial.AutoSize = true;
            lblSpecial.Location = new System.Drawing.Point(279, 20);
            lblSpecial.Margin = new Padding(4, 0, 4, 0);
            lblSpecial.Name = "lblSpecial";
            lblSpecial.Size = new System.Drawing.Size(47, 15);
            lblSpecial.TabIndex = 7;
            lblSpecial.Text = "Special:";
            // 
            // NodeSpecialComboBox
            // 
            NodeSpecialComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            NodeSpecialComboBox.FormattingEnabled = true;
            NodeSpecialComboBox.Location = new System.Drawing.Point(338, 16);
            NodeSpecialComboBox.Margin = new Padding(4, 3, 4, 3);
            NodeSpecialComboBox.Name = "NodeSpecialComboBox";
            NodeSpecialComboBox.Size = new System.Drawing.Size(206, 23);
            NodeSpecialComboBox.TabIndex = 8;
            NodeSpecialComboBox.SelectedIndexChanged += NodeSpecialComboBox_SelectedIndexChanged;
            // 
            // NodeIsPedNodeCheckBox
            // 
            NodeIsPedNodeCheckBox.AutoSize = true;
            NodeIsPedNodeCheckBox.Enabled = false;
            NodeIsPedNodeCheckBox.Location = new System.Drawing.Point(140, 60);
            NodeIsPedNodeCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeIsPedNodeCheckBox.Name = "NodeIsPedNodeCheckBox";
            NodeIsPedNodeCheckBox.Size = new System.Drawing.Size(89, 19);
            NodeIsPedNodeCheckBox.TabIndex = 9;
            NodeIsPedNodeCheckBox.Text = "Is Ped Node";
            // 
            // lblRawFlags0
            // 
            lblRawFlags0.AutoSize = true;
            lblRawFlags0.Location = new System.Drawing.Point(282, 48);
            lblRawFlags0.Margin = new Padding(4, 0, 4, 0);
            lblRawFlags0.Name = "lblRawFlags0";
            lblRawFlags0.Size = new System.Drawing.Size(32, 15);
            lblRawFlags0.TabIndex = 10;
            lblRawFlags0.Text = "Raw:";
            // 
            // NodeFlags0HexLabel
            // 
            NodeFlags0HexLabel.Location = new System.Drawing.Point(317, 48);
            NodeFlags0HexLabel.Margin = new Padding(4, 0, 4, 0);
            NodeFlags0HexLabel.Name = "NodeFlags0HexLabel";
            NodeFlags0HexLabel.Size = new System.Drawing.Size(84, 15);
            NodeFlags0HexLabel.TabIndex = 11;
            NodeFlags0HexLabel.Text = "0x00000000";
            // 
            // NodeFlags0UpDown
            // 
            NodeFlags0UpDown.Location = new System.Drawing.Point(407, 45);
            NodeFlags0UpDown.Margin = new Padding(4, 3, 4, 3);
            NodeFlags0UpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            NodeFlags0UpDown.Name = "NodeFlags0UpDown";
            NodeFlags0UpDown.Size = new System.Drawing.Size(140, 23);
            NodeFlags0UpDown.TabIndex = 12;
            NodeFlags0UpDown.ValueChanged += NodeFlags0Raw_ValueChanged;
            // 
            // Flags1GroupBox
            // 
            Flags1GroupBox.Controls.Add(NodeNoGpsCheckBox);
            Flags1GroupBox.Controls.Add(NodeIsJunctionCheckBox);
            Flags1GroupBox.Controls.Add(NodeSwitchedOffCheckBox);
            Flags1GroupBox.Controls.Add(NodeSwitchedOffOriginalCheckBox);
            Flags1GroupBox.Controls.Add(NodeWaterNodeCheckBox);
            Flags1GroupBox.Controls.Add(NodeHighwayCheckBox);
            Flags1GroupBox.Controls.Add(NodeQualifiesAsJunctionCheckBox);
            Flags1GroupBox.Controls.Add(NodeTunnelCheckBox);
            Flags1GroupBox.Controls.Add(NodeLeftOnlyCheckBox);
            Flags1GroupBox.Controls.Add(lblHeuristic);
            Flags1GroupBox.Controls.Add(NodeHeuristicUpDown);
            Flags1GroupBox.Controls.Add(lblDensity);
            Flags1GroupBox.Controls.Add(NodeDensityUpDown);
            Flags1GroupBox.Controls.Add(lblDeadEndness);
            Flags1GroupBox.Controls.Add(NodeDeadEndnessUpDown);
            Flags1GroupBox.Controls.Add(lblRawFlags1);
            Flags1GroupBox.Controls.Add(NodeFlags1HexLabel);
            Flags1GroupBox.Controls.Add(NodeFlags1UpDown);
            Flags1GroupBox.Location = new System.Drawing.Point(7, 276);
            Flags1GroupBox.Margin = new Padding(4, 3, 4, 3);
            Flags1GroupBox.Name = "Flags1GroupBox";
            Flags1GroupBox.Padding = new Padding(4, 3, 4, 3);
            Flags1GroupBox.Size = new System.Drawing.Size(554, 126);
            Flags1GroupBox.TabIndex = 2;
            Flags1GroupBox.TabStop = false;
            Flags1GroupBox.Text = "Flags1";
            // 
            // NodeNoGpsCheckBox
            // 
            NodeNoGpsCheckBox.AutoSize = true;
            NodeNoGpsCheckBox.Location = new System.Drawing.Point(221, 39);
            NodeNoGpsCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeNoGpsCheckBox.Name = "NodeNoGpsCheckBox";
            NodeNoGpsCheckBox.Size = new System.Drawing.Size(66, 19);
            NodeNoGpsCheckBox.TabIndex = 0;
            NodeNoGpsCheckBox.Text = "No GPS";
            NodeNoGpsCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeIsJunctionCheckBox
            // 
            NodeIsJunctionCheckBox.AutoSize = true;
            NodeIsJunctionCheckBox.Location = new System.Drawing.Point(9, 18);
            NodeIsJunctionCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeIsJunctionCheckBox.Name = "NodeIsJunctionCheckBox";
            NodeIsJunctionCheckBox.Size = new System.Drawing.Size(82, 19);
            NodeIsJunctionCheckBox.TabIndex = 1;
            NodeIsJunctionCheckBox.Text = "Is Junction";
            NodeIsJunctionCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeSwitchedOffCheckBox
            // 
            NodeSwitchedOffCheckBox.AutoSize = true;
            NodeSwitchedOffCheckBox.Location = new System.Drawing.Point(119, 18);
            NodeSwitchedOffCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeSwitchedOffCheckBox.Name = "NodeSwitchedOffCheckBox";
            NodeSwitchedOffCheckBox.Size = new System.Drawing.Size(94, 19);
            NodeSwitchedOffCheckBox.TabIndex = 2;
            NodeSwitchedOffCheckBox.Text = "Switched Off";
            NodeSwitchedOffCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeSwitchedOffOriginalCheckBox
            // 
            NodeSwitchedOffOriginalCheckBox.AutoSize = true;
            NodeSwitchedOffOriginalCheckBox.Location = new System.Drawing.Point(221, 60);
            NodeSwitchedOffOriginalCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeSwitchedOffOriginalCheckBox.Name = "NodeSwitchedOffOriginalCheckBox";
            NodeSwitchedOffOriginalCheckBox.Size = new System.Drawing.Size(147, 19);
            NodeSwitchedOffOriginalCheckBox.TabIndex = 3;
            NodeSwitchedOffOriginalCheckBox.Text = "Switched Off (Original)";
            NodeSwitchedOffOriginalCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeWaterNodeCheckBox
            // 
            NodeWaterNodeCheckBox.AutoSize = true;
            NodeWaterNodeCheckBox.Location = new System.Drawing.Point(9, 43);
            NodeWaterNodeCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeWaterNodeCheckBox.Name = "NodeWaterNodeCheckBox";
            NodeWaterNodeCheckBox.Size = new System.Drawing.Size(89, 19);
            NodeWaterNodeCheckBox.TabIndex = 4;
            NodeWaterNodeCheckBox.Text = "Water Node";
            NodeWaterNodeCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeHighwayCheckBox
            // 
            NodeHighwayCheckBox.AutoSize = true;
            NodeHighwayCheckBox.Location = new System.Drawing.Point(119, 39);
            NodeHighwayCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeHighwayCheckBox.Name = "NodeHighwayCheckBox";
            NodeHighwayCheckBox.Size = new System.Drawing.Size(73, 19);
            NodeHighwayCheckBox.TabIndex = 5;
            NodeHighwayCheckBox.Text = "Highway";
            NodeHighwayCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeQualifiesAsJunctionCheckBox
            // 
            NodeQualifiesAsJunctionCheckBox.AutoSize = true;
            NodeQualifiesAsJunctionCheckBox.Location = new System.Drawing.Point(221, 18);
            NodeQualifiesAsJunctionCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeQualifiesAsJunctionCheckBox.Name = "NodeQualifiesAsJunctionCheckBox";
            NodeQualifiesAsJunctionCheckBox.Size = new System.Drawing.Size(136, 19);
            NodeQualifiesAsJunctionCheckBox.TabIndex = 6;
            NodeQualifiesAsJunctionCheckBox.Text = "Qualifies As Junction";
            NodeQualifiesAsJunctionCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeTunnelCheckBox
            // 
            NodeTunnelCheckBox.AutoSize = true;
            NodeTunnelCheckBox.Location = new System.Drawing.Point(9, 64);
            NodeTunnelCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeTunnelCheckBox.Name = "NodeTunnelCheckBox";
            NodeTunnelCheckBox.Size = new System.Drawing.Size(63, 19);
            NodeTunnelCheckBox.TabIndex = 7;
            NodeTunnelCheckBox.Text = "Tunnel";
            NodeTunnelCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // NodeLeftOnlyCheckBox
            // 
            NodeLeftOnlyCheckBox.AutoSize = true;
            NodeLeftOnlyCheckBox.Location = new System.Drawing.Point(119, 60);
            NodeLeftOnlyCheckBox.Margin = new Padding(4, 3, 4, 3);
            NodeLeftOnlyCheckBox.Name = "NodeLeftOnlyCheckBox";
            NodeLeftOnlyCheckBox.Size = new System.Drawing.Size(74, 19);
            NodeLeftOnlyCheckBox.TabIndex = 8;
            NodeLeftOnlyCheckBox.Text = "Left Only";
            NodeLeftOnlyCheckBox.CheckedChanged += NodeFlagCheckBox_Changed;
            // 
            // lblHeuristic
            // 
            lblHeuristic.AutoSize = true;
            lblHeuristic.Location = new System.Drawing.Point(395, 16);
            lblHeuristic.Margin = new Padding(4, 0, 4, 0);
            lblHeuristic.Name = "lblHeuristic";
            lblHeuristic.Size = new System.Drawing.Size(57, 15);
            lblHeuristic.TabIndex = 9;
            lblHeuristic.Text = "Heuristic:";
            // 
            // NodeHeuristicUpDown
            // 
            NodeHeuristicUpDown.Location = new System.Drawing.Point(487, 14);
            NodeHeuristicUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeHeuristicUpDown.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
            NodeHeuristicUpDown.Name = "NodeHeuristicUpDown";
            NodeHeuristicUpDown.Size = new System.Drawing.Size(52, 23);
            NodeHeuristicUpDown.TabIndex = 10;
            NodeHeuristicUpDown.ValueChanged += NodeValueUpDown_Changed;
            // 
            // lblDensity
            // 
            lblDensity.AutoSize = true;
            lblDensity.Location = new System.Drawing.Point(395, 45);
            lblDensity.Margin = new Padding(4, 0, 4, 0);
            lblDensity.Name = "lblDensity";
            lblDensity.Size = new System.Drawing.Size(49, 15);
            lblDensity.TabIndex = 11;
            lblDensity.Text = "Density:";
            // 
            // NodeDensityUpDown
            // 
            NodeDensityUpDown.Location = new System.Drawing.Point(487, 43);
            NodeDensityUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeDensityUpDown.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            NodeDensityUpDown.Name = "NodeDensityUpDown";
            NodeDensityUpDown.Size = new System.Drawing.Size(52, 23);
            NodeDensityUpDown.TabIndex = 12;
            NodeDensityUpDown.ValueChanged += NodeValueUpDown_Changed;
            // 
            // lblDeadEndness
            // 
            lblDeadEndness.AutoSize = true;
            lblDeadEndness.Location = new System.Drawing.Point(395, 74);
            lblDeadEndness.Margin = new Padding(4, 0, 4, 0);
            lblDeadEndness.Name = "lblDeadEndness";
            lblDeadEndness.Size = new System.Drawing.Size(80, 15);
            lblDeadEndness.TabIndex = 13;
            lblDeadEndness.Text = "Deadendness:";
            // 
            // NodeDeadEndnessUpDown
            // 
            NodeDeadEndnessUpDown.Location = new System.Drawing.Point(487, 72);
            NodeDeadEndnessUpDown.Margin = new Padding(4, 3, 4, 3);
            NodeDeadEndnessUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            NodeDeadEndnessUpDown.Name = "NodeDeadEndnessUpDown";
            NodeDeadEndnessUpDown.Size = new System.Drawing.Size(52, 23);
            NodeDeadEndnessUpDown.TabIndex = 14;
            NodeDeadEndnessUpDown.ValueChanged += NodeValueUpDown_Changed;
            // 
            // lblRawFlags1
            // 
            lblRawFlags1.AutoSize = true;
            lblRawFlags1.Location = new System.Drawing.Point(7, 99);
            lblRawFlags1.Margin = new Padding(4, 0, 4, 0);
            lblRawFlags1.Name = "lblRawFlags1";
            lblRawFlags1.Size = new System.Drawing.Size(32, 15);
            lblRawFlags1.TabIndex = 15;
            lblRawFlags1.Text = "Raw:";
            // 
            // NodeFlags1HexLabel
            // 
            NodeFlags1HexLabel.Location = new System.Drawing.Point(42, 99);
            NodeFlags1HexLabel.Margin = new Padding(4, 0, 4, 0);
            NodeFlags1HexLabel.Name = "NodeFlags1HexLabel";
            NodeFlags1HexLabel.Size = new System.Drawing.Size(84, 15);
            NodeFlags1HexLabel.TabIndex = 16;
            NodeFlags1HexLabel.Text = "0x00000000";
            // 
            // NodeFlags1UpDown
            // 
            NodeFlags1UpDown.Location = new System.Drawing.Point(132, 96);
            NodeFlags1UpDown.Margin = new Padding(4, 3, 4, 3);
            NodeFlags1UpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            NodeFlags1UpDown.Name = "NodeFlags1UpDown";
            NodeFlags1UpDown.Size = new System.Drawing.Size(140, 23);
            NodeFlags1UpDown.TabIndex = 17;
            NodeFlags1UpDown.ValueChanged += NodeFlags1Raw_ValueChanged;
            // 
            // NodeFloodCopyButton
            // 
            NodeFloodCopyButton.Location = new System.Drawing.Point(7, 408);
            NodeFloodCopyButton.Margin = new Padding(4, 3, 4, 3);
            NodeFloodCopyButton.Name = "NodeFloodCopyButton";
            NodeFloodCopyButton.Size = new System.Drawing.Size(134, 29);
            NodeFloodCopyButton.TabIndex = 6;
            NodeFloodCopyButton.Text = "Flood Copy Flags";
            NodeFloodCopyButton.UseVisualStyleBackColor = true;
            NodeFloodCopyButton.Click += NodeFloodCopyButton_Click;
            // 
            // NodeEnableDisableButton
            // 
            NodeEnableDisableButton.Location = new System.Drawing.Point(149, 408);
            NodeEnableDisableButton.Margin = new Padding(4, 3, 4, 3);
            NodeEnableDisableButton.Name = "NodeEnableDisableButton";
            NodeEnableDisableButton.Size = new System.Drawing.Size(134, 29);
            NodeEnableDisableButton.TabIndex = 7;
            NodeEnableDisableButton.Text = "Disable Section";
            NodeEnableDisableButton.UseVisualStyleBackColor = true;
            NodeEnableDisableButton.Click += NodeEnableDisableButton_Click;
            // 
            // LinkTabPage
            // 
            LinkTabPage.AutoScroll = true;
            LinkTabPage.Controls.Add(NodeLinkCountLabel);
            LinkTabPage.Controls.Add(NodeLinksListBox);
            LinkTabPage.Controls.Add(NodeAddLinkButton);
            LinkTabPage.Controls.Add(NodeRemoveLinkButton);
            LinkTabPage.Controls.Add(LinkPanel);
            LinkTabPage.Location = new System.Drawing.Point(4, 24);
            LinkTabPage.Margin = new Padding(4, 3, 4, 3);
            LinkTabPage.Name = "LinkTabPage";
            LinkTabPage.Padding = new Padding(4, 3, 4, 3);
            LinkTabPage.Size = new System.Drawing.Size(570, 559);
            LinkTabPage.TabIndex = 1;
            LinkTabPage.Text = "Link";
            // 
            // NodeLinkCountLabel
            // 
            NodeLinkCountLabel.AutoSize = true;
            NodeLinkCountLabel.Location = new System.Drawing.Point(7, 7);
            NodeLinkCountLabel.Margin = new Padding(4, 0, 4, 0);
            NodeLinkCountLabel.Name = "NodeLinkCountLabel";
            NodeLinkCountLabel.Size = new System.Drawing.Size(77, 15);
            NodeLinkCountLabel.TabIndex = 8;
            NodeLinkCountLabel.Text = "Link Count: 0";
            // 
            // NodeLinksListBox
            // 
            NodeLinksListBox.FormattingEnabled = true;
            NodeLinksListBox.Location = new System.Drawing.Point(7, 25);
            NodeLinksListBox.Margin = new Padding(4, 3, 4, 3);
            NodeLinksListBox.Name = "NodeLinksListBox";
            NodeLinksListBox.Size = new System.Drawing.Size(450, 109);
            NodeLinksListBox.TabIndex = 9;
            NodeLinksListBox.SelectedIndexChanged += NodeLinksListBox_SelectedIndexChanged;
            // 
            // NodeAddLinkButton
            // 
            NodeAddLinkButton.Location = new System.Drawing.Point(463, 25);
            NodeAddLinkButton.Margin = new Padding(4, 3, 4, 3);
            NodeAddLinkButton.Name = "NodeAddLinkButton";
            NodeAddLinkButton.Size = new System.Drawing.Size(90, 29);
            NodeAddLinkButton.TabIndex = 10;
            NodeAddLinkButton.Text = "Add Link";
            NodeAddLinkButton.UseVisualStyleBackColor = true;
            NodeAddLinkButton.Click += NodeAddLinkButton_Click;
            // 
            // NodeRemoveLinkButton
            // 
            NodeRemoveLinkButton.Location = new System.Drawing.Point(463, 60);
            NodeRemoveLinkButton.Margin = new Padding(4, 3, 4, 3);
            NodeRemoveLinkButton.Name = "NodeRemoveLinkButton";
            NodeRemoveLinkButton.Size = new System.Drawing.Size(90, 29);
            NodeRemoveLinkButton.TabIndex = 11;
            NodeRemoveLinkButton.Text = "Remove";
            NodeRemoveLinkButton.UseVisualStyleBackColor = true;
            NodeRemoveLinkButton.Click += NodeRemoveLinkButton_Click;
            // 
            // LinkPanel
            // 
            LinkPanel.AutoScroll = true;
            LinkPanel.Controls.Add(LinkTargetGroupBox);
            LinkPanel.Controls.Add(LinkFlagsGroupBox);
            LinkPanel.Controls.Add(LinkSelectPartnerButton);
            LinkPanel.Controls.Add(LinkStatusLabel);
            LinkPanel.Enabled = false;
            LinkPanel.Location = new System.Drawing.Point(4, 145);
            LinkPanel.Margin = new Padding(4, 3, 4, 3);
            LinkPanel.Name = "LinkPanel";
            LinkPanel.Size = new System.Drawing.Size(562, 410);
            LinkPanel.TabIndex = 0;
            // 
            // LinkTargetGroupBox
            // 
            LinkTargetGroupBox.Controls.Add(lblLinkAreaID);
            LinkTargetGroupBox.Controls.Add(LinkAreaIDUpDown);
            LinkTargetGroupBox.Controls.Add(lblLinkNodeID);
            LinkTargetGroupBox.Controls.Add(LinkNodeIDUpDown);
            LinkTargetGroupBox.Location = new System.Drawing.Point(7, 7);
            LinkTargetGroupBox.Margin = new Padding(4, 3, 4, 3);
            LinkTargetGroupBox.Name = "LinkTargetGroupBox";
            LinkTargetGroupBox.Padding = new Padding(4, 3, 4, 3);
            LinkTargetGroupBox.Size = new System.Drawing.Size(567, 55);
            LinkTargetGroupBox.TabIndex = 0;
            LinkTargetGroupBox.TabStop = false;
            LinkTargetGroupBox.Text = "Link Target";
            // 
            // lblLinkAreaID
            // 
            lblLinkAreaID.AutoSize = true;
            lblLinkAreaID.Location = new System.Drawing.Point(103, 24);
            lblLinkAreaID.Margin = new Padding(4, 0, 4, 0);
            lblLinkAreaID.Name = "lblLinkAreaID";
            lblLinkAreaID.Size = new System.Drawing.Size(48, 15);
            lblLinkAreaID.TabIndex = 0;
            lblLinkAreaID.Text = "Area ID:";
            // 
            // LinkAreaIDUpDown
            // 
            LinkAreaIDUpDown.Location = new System.Drawing.Point(167, 21);
            LinkAreaIDUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkAreaIDUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            LinkAreaIDUpDown.Name = "LinkAreaIDUpDown";
            LinkAreaIDUpDown.Size = new System.Drawing.Size(93, 23);
            LinkAreaIDUpDown.TabIndex = 1;
            LinkAreaIDUpDown.ValueChanged += LinkAreaIDUpDown_ValueChanged;
            // 
            // lblLinkNodeID
            // 
            lblLinkNodeID.AutoSize = true;
            lblLinkNodeID.Location = new System.Drawing.Point(295, 25);
            lblLinkNodeID.Margin = new Padding(4, 0, 4, 0);
            lblLinkNodeID.Name = "lblLinkNodeID";
            lblLinkNodeID.Size = new System.Drawing.Size(53, 15);
            lblLinkNodeID.TabIndex = 2;
            lblLinkNodeID.Text = "Node ID:";
            // 
            // LinkNodeIDUpDown
            // 
            LinkNodeIDUpDown.Location = new System.Drawing.Point(359, 22);
            LinkNodeIDUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkNodeIDUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            LinkNodeIDUpDown.Name = "LinkNodeIDUpDown";
            LinkNodeIDUpDown.Size = new System.Drawing.Size(93, 23);
            LinkNodeIDUpDown.TabIndex = 3;
            LinkNodeIDUpDown.ValueChanged += LinkNodeIDUpDown_ValueChanged;
            // 
            // LinkFlagsGroupBox
            // 
            LinkFlagsGroupBox.Controls.Add(LinkGpsBothWaysCheckBox);
            LinkFlagsGroupBox.Controls.Add(LinkShortcutCheckBox);
            LinkFlagsGroupBox.Controls.Add(LinkNarrowRoadCheckBox);
            LinkFlagsGroupBox.Controls.Add(LinkDontUseForNavCheckBox);
            LinkFlagsGroupBox.Controls.Add(LinkNegativeOffsetCheckBox);
            LinkFlagsGroupBox.Controls.Add(lblLinkOffset);
            LinkFlagsGroupBox.Controls.Add(LinkOffsetUpDown);
            LinkFlagsGroupBox.Controls.Add(lblLinkFwdLanes);
            LinkFlagsGroupBox.Controls.Add(LinkFwdLanesUpDown);
            LinkFlagsGroupBox.Controls.Add(lblLinkBackLanes);
            LinkFlagsGroupBox.Controls.Add(LinkBackLanesUpDown);
            LinkFlagsGroupBox.Controls.Add(lblLinkDistance);
            LinkFlagsGroupBox.Controls.Add(LinkDistanceUpDown);
            LinkFlagsGroupBox.Controls.Add(lblLinkRawFlags);
            LinkFlagsGroupBox.Controls.Add(LinkFlags0HexLabel);
            LinkFlagsGroupBox.Controls.Add(LinkFlags0UpDown);
            LinkFlagsGroupBox.Location = new System.Drawing.Point(7, 69);
            LinkFlagsGroupBox.Margin = new Padding(4, 3, 4, 3);
            LinkFlagsGroupBox.Name = "LinkFlagsGroupBox";
            LinkFlagsGroupBox.Padding = new Padding(4, 3, 4, 3);
            LinkFlagsGroupBox.Size = new System.Drawing.Size(567, 150);
            LinkFlagsGroupBox.TabIndex = 1;
            LinkFlagsGroupBox.TabStop = false;
            LinkFlagsGroupBox.Text = "Link Flags";
            // 
            // LinkGpsBothWaysCheckBox
            // 
            LinkGpsBothWaysCheckBox.AutoSize = true;
            LinkGpsBothWaysCheckBox.Location = new System.Drawing.Point(8, 119);
            LinkGpsBothWaysCheckBox.Margin = new Padding(4, 3, 4, 3);
            LinkGpsBothWaysCheckBox.Name = "LinkGpsBothWaysCheckBox";
            LinkGpsBothWaysCheckBox.Size = new System.Drawing.Size(106, 19);
            LinkGpsBothWaysCheckBox.TabIndex = 0;
            LinkGpsBothWaysCheckBox.Text = "GPS Both Ways";
            LinkGpsBothWaysCheckBox.CheckedChanged += LinkFlagCheckBox_Changed;
            // 
            // LinkShortcutCheckBox
            // 
            LinkShortcutCheckBox.AutoSize = true;
            LinkShortcutCheckBox.Location = new System.Drawing.Point(8, 96);
            LinkShortcutCheckBox.Margin = new Padding(4, 3, 4, 3);
            LinkShortcutCheckBox.Name = "LinkShortcutCheckBox";
            LinkShortcutCheckBox.Size = new System.Drawing.Size(71, 19);
            LinkShortcutCheckBox.TabIndex = 1;
            LinkShortcutCheckBox.Text = "Shortcut";
            LinkShortcutCheckBox.CheckedChanged += LinkFlagCheckBox_Changed;
            // 
            // LinkNarrowRoadCheckBox
            // 
            LinkNarrowRoadCheckBox.AutoSize = true;
            LinkNarrowRoadCheckBox.Location = new System.Drawing.Point(8, 46);
            LinkNarrowRoadCheckBox.Margin = new Padding(4, 3, 4, 3);
            LinkNarrowRoadCheckBox.Name = "LinkNarrowRoadCheckBox";
            LinkNarrowRoadCheckBox.Size = new System.Drawing.Size(95, 19);
            LinkNarrowRoadCheckBox.TabIndex = 2;
            LinkNarrowRoadCheckBox.Text = "Narrow Road";
            LinkNarrowRoadCheckBox.CheckedChanged += LinkFlagCheckBox_Changed;
            // 
            // LinkDontUseForNavCheckBox
            // 
            LinkDontUseForNavCheckBox.AutoSize = true;
            LinkDontUseForNavCheckBox.Location = new System.Drawing.Point(8, 22);
            LinkDontUseForNavCheckBox.Margin = new Padding(4, 3, 4, 3);
            LinkDontUseForNavCheckBox.Name = "LinkDontUseForNavCheckBox";
            LinkDontUseForNavCheckBox.Size = new System.Drawing.Size(158, 19);
            LinkDontUseForNavCheckBox.TabIndex = 3;
            LinkDontUseForNavCheckBox.Text = "Don't Use For Navigation";
            LinkDontUseForNavCheckBox.CheckedChanged += LinkFlagCheckBox_Changed;
            // 
            // LinkNegativeOffsetCheckBox
            // 
            LinkNegativeOffsetCheckBox.AutoSize = true;
            LinkNegativeOffsetCheckBox.Location = new System.Drawing.Point(8, 71);
            LinkNegativeOffsetCheckBox.Margin = new Padding(4, 3, 4, 3);
            LinkNegativeOffsetCheckBox.Name = "LinkNegativeOffsetCheckBox";
            LinkNegativeOffsetCheckBox.Size = new System.Drawing.Size(108, 19);
            LinkNegativeOffsetCheckBox.TabIndex = 4;
            LinkNegativeOffsetCheckBox.Text = "Negative Offset";
            LinkNegativeOffsetCheckBox.CheckedChanged += LinkFlagCheckBox_Changed;
            // 
            // lblLinkOffset
            // 
            lblLinkOffset.AutoSize = true;
            lblLinkOffset.Location = new System.Drawing.Point(373, 26);
            lblLinkOffset.Margin = new Padding(4, 0, 4, 0);
            lblLinkOffset.Name = "lblLinkOffset";
            lblLinkOffset.Size = new System.Drawing.Size(42, 15);
            lblLinkOffset.TabIndex = 5;
            lblLinkOffset.Text = "Offset:";
            // 
            // LinkOffsetUpDown
            // 
            LinkOffsetUpDown.Location = new System.Drawing.Point(446, 18);
            LinkOffsetUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkOffsetUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            LinkOffsetUpDown.Name = "LinkOffsetUpDown";
            LinkOffsetUpDown.Size = new System.Drawing.Size(93, 23);
            LinkOffsetUpDown.TabIndex = 6;
            LinkOffsetUpDown.ValueChanged += LinkValueUpDown_Changed;
            // 
            // lblLinkFwdLanes
            // 
            lblLinkFwdLanes.AutoSize = true;
            lblLinkFwdLanes.Location = new System.Drawing.Point(181, 31);
            lblLinkFwdLanes.Margin = new Padding(4, 0, 4, 0);
            lblLinkFwdLanes.Name = "lblLinkFwdLanes";
            lblLinkFwdLanes.Size = new System.Drawing.Size(65, 15);
            lblLinkFwdLanes.TabIndex = 7;
            lblLinkFwdLanes.Text = "Fwd Lanes:";
            lblLinkFwdLanes.Click += lblLinkFwdLanes_Click;
            // 
            // LinkFwdLanesUpDown
            // 
            LinkFwdLanesUpDown.Location = new System.Drawing.Point(254, 23);
            LinkFwdLanesUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkFwdLanesUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            LinkFwdLanesUpDown.Name = "LinkFwdLanesUpDown";
            LinkFwdLanesUpDown.Size = new System.Drawing.Size(93, 23);
            LinkFwdLanesUpDown.TabIndex = 8;
            LinkFwdLanesUpDown.ValueChanged += LinkValueUpDown_Changed;
            // 
            // lblLinkBackLanes
            // 
            lblLinkBackLanes.AutoSize = true;
            lblLinkBackLanes.Location = new System.Drawing.Point(181, 60);
            lblLinkBackLanes.Margin = new Padding(4, 0, 4, 0);
            lblLinkBackLanes.Name = "lblLinkBackLanes";
            lblLinkBackLanes.Size = new System.Drawing.Size(68, 15);
            lblLinkBackLanes.TabIndex = 9;
            lblLinkBackLanes.Text = "Back Lanes:";
            // 
            // LinkBackLanesUpDown
            // 
            LinkBackLanesUpDown.Location = new System.Drawing.Point(254, 52);
            LinkBackLanesUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkBackLanesUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            LinkBackLanesUpDown.Name = "LinkBackLanesUpDown";
            LinkBackLanesUpDown.Size = new System.Drawing.Size(93, 23);
            LinkBackLanesUpDown.TabIndex = 10;
            LinkBackLanesUpDown.ValueChanged += LinkValueUpDown_Changed;
            // 
            // lblLinkDistance
            // 
            lblLinkDistance.AutoSize = true;
            lblLinkDistance.Location = new System.Drawing.Point(373, 56);
            lblLinkDistance.Margin = new Padding(4, 0, 4, 0);
            lblLinkDistance.Name = "lblLinkDistance";
            lblLinkDistance.Size = new System.Drawing.Size(55, 15);
            lblLinkDistance.TabIndex = 11;
            lblLinkDistance.Text = "Distance:";
            // 
            // LinkDistanceUpDown
            // 
            LinkDistanceUpDown.Location = new System.Drawing.Point(446, 52);
            LinkDistanceUpDown.Margin = new Padding(4, 3, 4, 3);
            LinkDistanceUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            LinkDistanceUpDown.Name = "LinkDistanceUpDown";
            LinkDistanceUpDown.Size = new System.Drawing.Size(93, 23);
            LinkDistanceUpDown.TabIndex = 12;
            LinkDistanceUpDown.ValueChanged += LinkDistanceUpDown_ValueChanged;
            // 
            // lblLinkRawFlags
            // 
            lblLinkRawFlags.AutoSize = true;
            lblLinkRawFlags.Location = new System.Drawing.Point(275, 100);
            lblLinkRawFlags.Margin = new Padding(4, 0, 4, 0);
            lblLinkRawFlags.Name = "lblLinkRawFlags";
            lblLinkRawFlags.Size = new System.Drawing.Size(32, 15);
            lblLinkRawFlags.TabIndex = 13;
            lblLinkRawFlags.Text = "Raw:";
            // 
            // LinkFlags0HexLabel
            // 
            LinkFlags0HexLabel.Location = new System.Drawing.Point(307, 100);
            LinkFlags0HexLabel.Margin = new Padding(4, 0, 4, 0);
            LinkFlags0HexLabel.Name = "LinkFlags0HexLabel";
            LinkFlags0HexLabel.Size = new System.Drawing.Size(84, 15);
            LinkFlags0HexLabel.TabIndex = 14;
            LinkFlags0HexLabel.Text = "0x00000000";
            // 
            // LinkFlags0UpDown
            // 
            LinkFlags0UpDown.Location = new System.Drawing.Point(399, 95);
            LinkFlags0UpDown.Margin = new Padding(4, 3, 4, 3);
            LinkFlags0UpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            LinkFlags0UpDown.Name = "LinkFlags0UpDown";
            LinkFlags0UpDown.Size = new System.Drawing.Size(140, 23);
            LinkFlags0UpDown.TabIndex = 15;
            LinkFlags0UpDown.ValueChanged += LinkFlags0Raw_ValueChanged;
            // 
            // LinkSelectPartnerButton
            // 
            LinkSelectPartnerButton.Location = new System.Drawing.Point(7, 225);
            LinkSelectPartnerButton.Margin = new Padding(4, 3, 4, 3);
            LinkSelectPartnerButton.Name = "LinkSelectPartnerButton";
            LinkSelectPartnerButton.Size = new System.Drawing.Size(128, 29);
            LinkSelectPartnerButton.TabIndex = 2;
            LinkSelectPartnerButton.Text = "Select Partner";
            LinkSelectPartnerButton.UseVisualStyleBackColor = true;
            LinkSelectPartnerButton.Click += LinkSelectPartnerButton_Click;
            // 
            // LinkStatusLabel
            // 
            LinkStatusLabel.AutoSize = true;
            LinkStatusLabel.Location = new System.Drawing.Point(7, 314);
            LinkStatusLabel.Margin = new Padding(4, 0, 4, 0);
            LinkStatusLabel.Name = "LinkStatusLabel";
            LinkStatusLabel.Size = new System.Drawing.Size(0, 15);
            LinkStatusLabel.TabIndex = 3;
            // 
            // JunctionTabPage
            // 
            JunctionTabPage.Controls.Add(JunctionEnableCheckBox);
            JunctionTabPage.Controls.Add(JunctionPanel);
            JunctionTabPage.Location = new System.Drawing.Point(4, 24);
            JunctionTabPage.Margin = new Padding(4, 3, 4, 3);
            JunctionTabPage.Name = "JunctionTabPage";
            JunctionTabPage.Padding = new Padding(4, 3, 4, 3);
            JunctionTabPage.Size = new System.Drawing.Size(570, 559);
            JunctionTabPage.TabIndex = 2;
            JunctionTabPage.Text = "Junction";
            // 
            // JunctionEnableCheckBox
            // 
            JunctionEnableCheckBox.AutoSize = true;
            JunctionEnableCheckBox.Location = new System.Drawing.Point(7, 12);
            JunctionEnableCheckBox.Margin = new Padding(4, 3, 4, 3);
            JunctionEnableCheckBox.Name = "JunctionEnableCheckBox";
            JunctionEnableCheckBox.Size = new System.Drawing.Size(109, 19);
            JunctionEnableCheckBox.TabIndex = 0;
            JunctionEnableCheckBox.Text = "Enable Junction";
            JunctionEnableCheckBox.CheckedChanged += JunctionEnableCheckBox_CheckedChanged;
            // 
            // JunctionPanel
            // 
            JunctionPanel.Controls.Add(lblJuncMaxZ);
            JunctionPanel.Controls.Add(JunctionMaxZUpDown);
            JunctionPanel.Controls.Add(lblJuncMinZ);
            JunctionPanel.Controls.Add(JunctionMinZUpDown);
            JunctionPanel.Controls.Add(lblJuncPosX);
            JunctionPanel.Controls.Add(JunctionPosXUpDown);
            JunctionPanel.Controls.Add(lblJuncPosY);
            JunctionPanel.Controls.Add(JunctionPosYUpDown);
            JunctionPanel.Controls.Add(lblJuncDimX);
            JunctionPanel.Controls.Add(JunctionDimXUpDown);
            JunctionPanel.Controls.Add(lblJuncDimY);
            JunctionPanel.Controls.Add(JunctionDimYUpDown);
            JunctionPanel.Controls.Add(lblJuncHeightmap);
            JunctionPanel.Controls.Add(JunctionHeightmapTextBox);
            JunctionPanel.Controls.Add(JunctionGenerateButton);
            JunctionPanel.Enabled = false;
            JunctionPanel.Location = new System.Drawing.Point(7, 38);
            JunctionPanel.Margin = new Padding(4, 3, 4, 3);
            JunctionPanel.Name = "JunctionPanel";
            JunctionPanel.Size = new System.Drawing.Size(554, 513);
            JunctionPanel.TabIndex = 1;
            // 
            // lblJuncMaxZ
            // 
            lblJuncMaxZ.AutoSize = true;
            lblJuncMaxZ.Location = new System.Drawing.Point(12, 12);
            lblJuncMaxZ.Margin = new Padding(4, 0, 4, 0);
            lblJuncMaxZ.Name = "lblJuncMaxZ";
            lblJuncMaxZ.Size = new System.Drawing.Size(42, 15);
            lblJuncMaxZ.TabIndex = 0;
            lblJuncMaxZ.Text = "Max Z:";
            // 
            // JunctionMaxZUpDown
            // 
            JunctionMaxZUpDown.DecimalPlaces = 4;
            JunctionMaxZUpDown.Increment = new decimal(new int[] { 3125, 0, 0, 327680 });
            JunctionMaxZUpDown.Location = new System.Drawing.Point(64, 8);
            JunctionMaxZUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionMaxZUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            JunctionMaxZUpDown.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            JunctionMaxZUpDown.Name = "JunctionMaxZUpDown";
            JunctionMaxZUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionMaxZUpDown.TabIndex = 1;
            JunctionMaxZUpDown.ValueChanged += JunctionMaxZUpDown_ValueChanged;
            // 
            // lblJuncMinZ
            // 
            lblJuncMinZ.AutoSize = true;
            lblJuncMinZ.Location = new System.Drawing.Point(12, 42);
            lblJuncMinZ.Margin = new Padding(4, 0, 4, 0);
            lblJuncMinZ.Name = "lblJuncMinZ";
            lblJuncMinZ.Size = new System.Drawing.Size(41, 15);
            lblJuncMinZ.TabIndex = 2;
            lblJuncMinZ.Text = "Min Z:";
            // 
            // JunctionMinZUpDown
            // 
            JunctionMinZUpDown.DecimalPlaces = 4;
            JunctionMinZUpDown.Increment = new decimal(new int[] { 3125, 0, 0, 327680 });
            JunctionMinZUpDown.Location = new System.Drawing.Point(64, 38);
            JunctionMinZUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionMinZUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            JunctionMinZUpDown.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            JunctionMinZUpDown.Name = "JunctionMinZUpDown";
            JunctionMinZUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionMinZUpDown.TabIndex = 3;
            JunctionMinZUpDown.ValueChanged += JunctionMinZUpDown_ValueChanged;
            // 
            // lblJuncPosX
            // 
            lblJuncPosX.AutoSize = true;
            lblJuncPosX.Location = new System.Drawing.Point(12, 72);
            lblJuncPosX.Margin = new Padding(4, 0, 4, 0);
            lblJuncPosX.Name = "lblJuncPosX";
            lblJuncPosX.Size = new System.Drawing.Size(39, 15);
            lblJuncPosX.TabIndex = 4;
            lblJuncPosX.Text = "Pos X:";
            // 
            // JunctionPosXUpDown
            // 
            JunctionPosXUpDown.DecimalPlaces = 2;
            JunctionPosXUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            JunctionPosXUpDown.Location = new System.Drawing.Point(64, 68);
            JunctionPosXUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionPosXUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            JunctionPosXUpDown.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
            JunctionPosXUpDown.Name = "JunctionPosXUpDown";
            JunctionPosXUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionPosXUpDown.TabIndex = 5;
            JunctionPosXUpDown.ValueChanged += JunctionPosXUpDown_ValueChanged;
            // 
            // lblJuncPosY
            // 
            lblJuncPosY.AutoSize = true;
            lblJuncPosY.Location = new System.Drawing.Point(12, 102);
            lblJuncPosY.Margin = new Padding(4, 0, 4, 0);
            lblJuncPosY.Name = "lblJuncPosY";
            lblJuncPosY.Size = new System.Drawing.Size(39, 15);
            lblJuncPosY.TabIndex = 6;
            lblJuncPosY.Text = "Pos Y:";
            // 
            // JunctionPosYUpDown
            // 
            JunctionPosYUpDown.DecimalPlaces = 2;
            JunctionPosYUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            JunctionPosYUpDown.Location = new System.Drawing.Point(64, 98);
            JunctionPosYUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionPosYUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            JunctionPosYUpDown.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
            JunctionPosYUpDown.Name = "JunctionPosYUpDown";
            JunctionPosYUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionPosYUpDown.TabIndex = 7;
            JunctionPosYUpDown.ValueChanged += JunctionPosYUpDown_ValueChanged;
            // 
            // lblJuncDimX
            // 
            lblJuncDimX.AutoSize = true;
            lblJuncDimX.Location = new System.Drawing.Point(12, 132);
            lblJuncDimX.Margin = new Padding(4, 0, 4, 0);
            lblJuncDimX.Name = "lblJuncDimX";
            lblJuncDimX.Size = new System.Drawing.Size(42, 15);
            lblJuncDimX.TabIndex = 8;
            lblJuncDimX.Text = "Dim X:";
            // 
            // JunctionDimXUpDown
            // 
            JunctionDimXUpDown.Location = new System.Drawing.Point(64, 128);
            JunctionDimXUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionDimXUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            JunctionDimXUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            JunctionDimXUpDown.Name = "JunctionDimXUpDown";
            JunctionDimXUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionDimXUpDown.TabIndex = 9;
            JunctionDimXUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            JunctionDimXUpDown.ValueChanged += JunctionDimXUpDown_ValueChanged;
            // 
            // lblJuncDimY
            // 
            lblJuncDimY.AutoSize = true;
            lblJuncDimY.Location = new System.Drawing.Point(12, 162);
            lblJuncDimY.Margin = new Padding(4, 0, 4, 0);
            lblJuncDimY.Name = "lblJuncDimY";
            lblJuncDimY.Size = new System.Drawing.Size(42, 15);
            lblJuncDimY.TabIndex = 10;
            lblJuncDimY.Text = "Dim Y:";
            // 
            // JunctionDimYUpDown
            // 
            JunctionDimYUpDown.Location = new System.Drawing.Point(64, 158);
            JunctionDimYUpDown.Margin = new Padding(4, 3, 4, 3);
            JunctionDimYUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            JunctionDimYUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            JunctionDimYUpDown.Name = "JunctionDimYUpDown";
            JunctionDimYUpDown.Size = new System.Drawing.Size(117, 23);
            JunctionDimYUpDown.TabIndex = 11;
            JunctionDimYUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            JunctionDimYUpDown.ValueChanged += JunctionDimYUpDown_ValueChanged;
            // 
            // lblJuncHeightmap
            // 
            lblJuncHeightmap.AutoSize = true;
            lblJuncHeightmap.Location = new System.Drawing.Point(12, 192);
            lblJuncHeightmap.Margin = new Padding(4, 0, 4, 0);
            lblJuncHeightmap.Name = "lblJuncHeightmap";
            lblJuncHeightmap.Size = new System.Drawing.Size(70, 15);
            lblJuncHeightmap.TabIndex = 12;
            lblJuncHeightmap.Text = "Heightmap:";
            // 
            // JunctionHeightmapTextBox
            // 
            JunctionHeightmapTextBox.Location = new System.Drawing.Point(12, 210);
            JunctionHeightmapTextBox.Margin = new Padding(4, 3, 4, 3);
            JunctionHeightmapTextBox.Multiline = true;
            JunctionHeightmapTextBox.Name = "JunctionHeightmapTextBox";
            JunctionHeightmapTextBox.ScrollBars = ScrollBars.Vertical;
            JunctionHeightmapTextBox.Size = new System.Drawing.Size(430, 300);
            JunctionHeightmapTextBox.TabIndex = 13;
            JunctionHeightmapTextBox.TextChanged += JunctionHeightmapTextBox_TextChanged;
            // 
            // JunctionGenerateButton
            // 
            JunctionGenerateButton.Location = new System.Drawing.Point(199, 155);
            JunctionGenerateButton.Margin = new Padding(4, 3, 4, 3);
            JunctionGenerateButton.Name = "JunctionGenerateButton";
            JunctionGenerateButton.Size = new System.Drawing.Size(117, 29);
            JunctionGenerateButton.TabIndex = 14;
            JunctionGenerateButton.Text = "Generate";
            JunctionGenerateButton.UseVisualStyleBackColor = true;
            JunctionGenerateButton.Click += JunctionGenerateButton_Click;
            // 
            // EditYndNodePanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(578, 587);
            Controls.Add(PathNodeTabControl);
            Margin = new Padding(4, 3, 4, 3);
            Name = "EditYndNodePanel";
            Text = "Edit Ynd Node";
            PathNodeTabControl.ResumeLayout(false);
            NodeTabPage.ResumeLayout(false);
            NodeInfoGroupBox.ResumeLayout(false);
            NodeInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NodeAreaIDUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NodeNodeIDUpDown).EndInit();
            Flags0GroupBox.ResumeLayout(false);
            Flags0GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NodeFloodGroupUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NodeFlags0UpDown).EndInit();
            Flags1GroupBox.ResumeLayout(false);
            Flags1GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NodeHeuristicUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NodeDensityUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NodeDeadEndnessUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NodeFlags1UpDown).EndInit();
            LinkTabPage.ResumeLayout(false);
            LinkTabPage.PerformLayout();
            LinkPanel.ResumeLayout(false);
            LinkPanel.PerformLayout();
            LinkTargetGroupBox.ResumeLayout(false);
            LinkTargetGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LinkAreaIDUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LinkNodeIDUpDown).EndInit();
            LinkFlagsGroupBox.ResumeLayout(false);
            LinkFlagsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LinkOffsetUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LinkFwdLanesUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LinkBackLanesUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LinkDistanceUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LinkFlags0UpDown).EndInit();
            JunctionTabPage.ResumeLayout(false);
            JunctionTabPage.PerformLayout();
            JunctionPanel.ResumeLayout(false);
            JunctionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)JunctionMaxZUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)JunctionMinZUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)JunctionPosXUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)JunctionPosYUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)JunctionDimXUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)JunctionDimYUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // Tab control
        private System.Windows.Forms.TabControl PathNodeTabControl;
        private System.Windows.Forms.TabPage NodeTabPage;
        private System.Windows.Forms.TabPage LinkTabPage;
        private System.Windows.Forms.TabPage JunctionTabPage;

        // Node Info
        private System.Windows.Forms.GroupBox NodeInfoGroupBox;
        private System.Windows.Forms.Label lblAreaID;
        private System.Windows.Forms.NumericUpDown NodeAreaIDUpDown;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.NumericUpDown NodeNodeIDUpDown;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.TextBox NodePositionTextBox;
        private System.Windows.Forms.Label lblStreetHash;
        private System.Windows.Forms.TextBox NodeStreetHashTextBox;
        private System.Windows.Forms.Label NodeStreetNameLabel;
        private System.Windows.Forms.Button NodeGoToButton;
        private System.Windows.Forms.Button NodeAddToProjectButton;
        private System.Windows.Forms.Button NodeDeleteButton;

        // Flags0
        private System.Windows.Forms.GroupBox Flags0GroupBox;
        private System.Windows.Forms.CheckBox NodeOffRoadCheckBox;
        private System.Windows.Forms.Label lblFloodGroup;
        private System.Windows.Forms.NumericUpDown NodeFloodGroupUpDown;
        private System.Windows.Forms.CheckBox NodeNoBigVehiclesCheckBox;
        private System.Windows.Forms.CheckBox NodeCannotGoRightCheckBox;
        private System.Windows.Forms.CheckBox NodeCannotGoLeftCheckBox;
        private System.Windows.Forms.CheckBox NodeSlipRoadCheckBox;
        private System.Windows.Forms.CheckBox NodeIndicateKeepLeftCheckBox;
        private System.Windows.Forms.CheckBox NodeIndicateKeepRightCheckBox;
        private System.Windows.Forms.Label lblSpecial;
        private System.Windows.Forms.ComboBox NodeSpecialComboBox;
        private System.Windows.Forms.CheckBox NodeIsPedNodeCheckBox;
        private System.Windows.Forms.Label lblRawFlags0;
        private System.Windows.Forms.Label NodeFlags0HexLabel;
        private System.Windows.Forms.NumericUpDown NodeFlags0UpDown;

        // Flags1
        private System.Windows.Forms.GroupBox Flags1GroupBox;
        private System.Windows.Forms.CheckBox NodeNoGpsCheckBox;
        private System.Windows.Forms.CheckBox NodeIsJunctionCheckBox;
        private System.Windows.Forms.CheckBox NodeSwitchedOffCheckBox;
        private System.Windows.Forms.CheckBox NodeSwitchedOffOriginalCheckBox;
        private System.Windows.Forms.CheckBox NodeWaterNodeCheckBox;
        private System.Windows.Forms.CheckBox NodeHighwayCheckBox;
        private System.Windows.Forms.CheckBox NodeQualifiesAsJunctionCheckBox;
        private System.Windows.Forms.CheckBox NodeTunnelCheckBox;
        private System.Windows.Forms.CheckBox NodeLeftOnlyCheckBox;
        private System.Windows.Forms.Label lblHeuristic;
        private System.Windows.Forms.NumericUpDown NodeHeuristicUpDown;
        private System.Windows.Forms.Label lblDensity;
        private System.Windows.Forms.NumericUpDown NodeDensityUpDown;
        private System.Windows.Forms.Label lblDeadEndness;
        private System.Windows.Forms.NumericUpDown NodeDeadEndnessUpDown;
        private System.Windows.Forms.Label lblRawFlags1;
        private System.Windows.Forms.Label NodeFlags1HexLabel;
        private System.Windows.Forms.NumericUpDown NodeFlags1UpDown;

        // Speed
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.ComboBox NodeSpeedComboBox;

        // Utility
        private System.Windows.Forms.Button NodeFloodCopyButton;
        private System.Windows.Forms.Button NodeEnableDisableButton;

        // Links
        private System.Windows.Forms.Label NodeLinkCountLabel;
        private System.Windows.Forms.ListBox NodeLinksListBox;
        private System.Windows.Forms.Button NodeAddLinkButton;
        private System.Windows.Forms.Button NodeRemoveLinkButton;

        // Link tab
        private System.Windows.Forms.Panel LinkPanel;
        private System.Windows.Forms.GroupBox LinkTargetGroupBox;
        private System.Windows.Forms.Label lblLinkAreaID;
        private System.Windows.Forms.NumericUpDown LinkAreaIDUpDown;
        private System.Windows.Forms.Label lblLinkNodeID;
        private System.Windows.Forms.NumericUpDown LinkNodeIDUpDown;
        private System.Windows.Forms.GroupBox LinkFlagsGroupBox;
        private System.Windows.Forms.CheckBox LinkGpsBothWaysCheckBox;
        private System.Windows.Forms.CheckBox LinkShortcutCheckBox;
        private System.Windows.Forms.CheckBox LinkNarrowRoadCheckBox;
        private System.Windows.Forms.CheckBox LinkDontUseForNavCheckBox;
        private System.Windows.Forms.CheckBox LinkNegativeOffsetCheckBox;
        private System.Windows.Forms.Label lblLinkOffset;
        private System.Windows.Forms.NumericUpDown LinkOffsetUpDown;
        private System.Windows.Forms.Label lblLinkFwdLanes;
        private System.Windows.Forms.NumericUpDown LinkFwdLanesUpDown;
        private System.Windows.Forms.Label lblLinkBackLanes;
        private System.Windows.Forms.NumericUpDown LinkBackLanesUpDown;
        private System.Windows.Forms.Label lblLinkDistance;
        private System.Windows.Forms.NumericUpDown LinkDistanceUpDown;
        private System.Windows.Forms.Label lblLinkRawFlags;
        private System.Windows.Forms.Label LinkFlags0HexLabel;
        private System.Windows.Forms.NumericUpDown LinkFlags0UpDown;
        private System.Windows.Forms.Label LinkStatusLabel;
        private System.Windows.Forms.Button LinkSelectPartnerButton;

        // Junction tab
        private System.Windows.Forms.CheckBox JunctionEnableCheckBox;
        private System.Windows.Forms.Panel JunctionPanel;
        private System.Windows.Forms.Label lblJuncMaxZ;
        private System.Windows.Forms.NumericUpDown JunctionMaxZUpDown;
        private System.Windows.Forms.Label lblJuncMinZ;
        private System.Windows.Forms.NumericUpDown JunctionMinZUpDown;
        private System.Windows.Forms.Label lblJuncPosX;
        private System.Windows.Forms.NumericUpDown JunctionPosXUpDown;
        private System.Windows.Forms.Label lblJuncPosY;
        private System.Windows.Forms.NumericUpDown JunctionPosYUpDown;
        private System.Windows.Forms.Label lblJuncDimX;
        private System.Windows.Forms.NumericUpDown JunctionDimXUpDown;
        private System.Windows.Forms.Label lblJuncDimY;
        private System.Windows.Forms.NumericUpDown JunctionDimYUpDown;
        private System.Windows.Forms.Label lblJuncHeightmap;
        private System.Windows.Forms.TextBox JunctionHeightmapTextBox;
        private System.Windows.Forms.Button JunctionGenerateButton;
    }
}
