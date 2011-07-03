namespace Dialog_Editor
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMI = new System.Windows.Forms.ToolStripMenuItem();
            this.newMI = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMI = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMI = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMI = new System.Windows.Forms.ToolStripMenuItem();
            this.dialogMI = new System.Windows.Forms.ToolStripMenuItem();
            this.addNPCPhraseMI = new System.Windows.Forms.ToolStripMenuItem();
            this.addPCPhraseMI = new System.Windows.Forms.ToolStripMenuItem();
            this.removePhraseMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMI = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMI = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newTB = new System.Windows.Forms.ToolStripButton();
            this.openTB = new System.Windows.Forms.ToolStripButton();
            this.saveTB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.addNpcPhraseTB = new System.Windows.Forms.ToolStripButton();
            this.addPcPhraseTB = new System.Windows.Forms.ToolStripButton();
            this.removeTB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.expandCollapseTB = new System.Windows.Forms.ToolStripButton();
            this.npcPhraseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPCPhraseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePhraseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeListView = new BrightIdeasSoftware.TreeListView();
            this.textColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.idColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.linkColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pcPhraseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNPCPhraseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePhraseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.followLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.npcPhraseMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).BeginInit();
            this.pcPhraseMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMI,
            this.dialogMI,
            this.helpMI});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(792, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileMI
            // 
            this.FileMI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMI,
            this.OpenMI,
            this.saveMI,
            this.saveAsMI,
            this.toolStripSeparator1,
            this.exitMI});
            this.FileMI.Name = "FileMI";
            this.FileMI.Size = new System.Drawing.Size(35, 20);
            this.FileMI.Text = "File";
            // 
            // newMI
            // 
            this.newMI.Image = ((System.Drawing.Image)(resources.GetObject("newMI.Image")));
            this.newMI.Name = "newMI";
            this.newMI.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.newMI.Size = new System.Drawing.Size(204, 22);
            this.newMI.Text = "New...";
            // 
            // OpenMI
            // 
            this.OpenMI.Image = ((System.Drawing.Image)(resources.GetObject("OpenMI.Image")));
            this.OpenMI.Name = "OpenMI";
            this.OpenMI.ShortcutKeyDisplayString = "Ctrl+O";
            this.OpenMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenMI.Size = new System.Drawing.Size(204, 22);
            this.OpenMI.Text = "Open...";
            // 
            // saveMI
            // 
            this.saveMI.Image = ((System.Drawing.Image)(resources.GetObject("saveMI.Image")));
            this.saveMI.Name = "saveMI";
            this.saveMI.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMI.Size = new System.Drawing.Size(204, 22);
            this.saveMI.Text = "Save";
            // 
            // saveAsMI
            // 
            this.saveAsMI.Image = ((System.Drawing.Image)(resources.GetObject("saveAsMI.Image")));
            this.saveAsMI.Name = "saveAsMI";
            this.saveAsMI.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAsMI.Size = new System.Drawing.Size(204, 22);
            this.saveAsMI.Text = "Save As...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // exitMI
            // 
            this.exitMI.Image = ((System.Drawing.Image)(resources.GetObject("exitMI.Image")));
            this.exitMI.Name = "exitMI";
            this.exitMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitMI.Size = new System.Drawing.Size(204, 22);
            this.exitMI.Text = "Exit";
            // 
            // dialogMI
            // 
            this.dialogMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNPCPhraseMI,
            this.addPCPhraseMI,
            this.removePhraseMI,
            this.toolStripSeparator2,
            this.propertiesToolStripMenuItem});
            this.dialogMI.Name = "dialogMI";
            this.dialogMI.Size = new System.Drawing.Size(48, 20);
            this.dialogMI.Text = "Dialog";
            // 
            // addNPCPhraseMI
            // 
            this.addNPCPhraseMI.Image = ((System.Drawing.Image)(resources.GetObject("addNPCPhraseMI.Image")));
            this.addNPCPhraseMI.Name = "addNPCPhraseMI";
            this.addNPCPhraseMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addNPCPhraseMI.Size = new System.Drawing.Size(212, 22);
            this.addNPCPhraseMI.Text = "Add NPC Phrase";
            // 
            // addPCPhraseMI
            // 
            this.addPCPhraseMI.Image = ((System.Drawing.Image)(resources.GetObject("addPCPhraseMI.Image")));
            this.addPCPhraseMI.Name = "addPCPhraseMI";
            this.addPCPhraseMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.addPCPhraseMI.Size = new System.Drawing.Size(212, 22);
            this.addPCPhraseMI.Text = "Add PC Phrase";
            // 
            // removePhraseMI
            // 
            this.removePhraseMI.Image = ((System.Drawing.Image)(resources.GetObject("removePhraseMI.Image")));
            this.removePhraseMI.Name = "removePhraseMI";
            this.removePhraseMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.removePhraseMI.Size = new System.Drawing.Size(212, 22);
            this.removePhraseMI.Text = "Remove Phrase";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("propertiesToolStripMenuItem.Image")));
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
            // 
            // helpMI
            // 
            this.helpMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMI});
            this.helpMI.Name = "helpMI";
            this.helpMI.Size = new System.Drawing.Size(40, 20);
            this.helpMI.Text = "Help";
            // 
            // aboutMI
            // 
            this.aboutMI.Image = ((System.Drawing.Image)(resources.GetObject("aboutMI.Image")));
            this.aboutMI.Name = "aboutMI";
            this.aboutMI.Size = new System.Drawing.Size(114, 22);
            this.aboutMI.Text = "About";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "User_icon_1.ico");
            this.imageList1.Images.SetKeyName(1, "User_icon_2.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTB,
            this.openTB,
            this.saveTB,
            this.toolStripSeparator3,
            this.addNpcPhraseTB,
            this.addPcPhraseTB,
            this.removeTB,
            this.toolStripSeparator4,
            this.expandCollapseTB});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newTB
            // 
            this.newTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newTB.Image = ((System.Drawing.Image)(resources.GetObject("newTB.Image")));
            this.newTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newTB.Name = "newTB";
            this.newTB.Size = new System.Drawing.Size(23, 22);
            this.newTB.Text = "New...";
            this.newTB.Click += new System.EventHandler(this.newTB_Click);
            // 
            // openTB
            // 
            this.openTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openTB.Image = ((System.Drawing.Image)(resources.GetObject("openTB.Image")));
            this.openTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openTB.Name = "openTB";
            this.openTB.Size = new System.Drawing.Size(23, 22);
            this.openTB.Text = "Open";
            this.openTB.Click += new System.EventHandler(this.openTB_Click);
            // 
            // saveTB
            // 
            this.saveTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveTB.Image = ((System.Drawing.Image)(resources.GetObject("saveTB.Image")));
            this.saveTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveTB.Name = "saveTB";
            this.saveTB.Size = new System.Drawing.Size(23, 22);
            this.saveTB.Text = "Save";
            this.saveTB.Click += new System.EventHandler(this.saveTB_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // addNpcPhraseTB
            // 
            this.addNpcPhraseTB.Image = ((System.Drawing.Image)(resources.GetObject("addNpcPhraseTB.Image")));
            this.addNpcPhraseTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addNpcPhraseTB.Name = "addNpcPhraseTB";
            this.addNpcPhraseTB.Size = new System.Drawing.Size(105, 22);
            this.addNpcPhraseTB.Text = "Add NPC Phrase";
            this.addNpcPhraseTB.Click += new System.EventHandler(this.addNpcPhraseTB_Click);
            // 
            // addPcPhraseTB
            // 
            this.addPcPhraseTB.Image = ((System.Drawing.Image)(resources.GetObject("addPcPhraseTB.Image")));
            this.addPcPhraseTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPcPhraseTB.Name = "addPcPhraseTB";
            this.addPcPhraseTB.Size = new System.Drawing.Size(98, 22);
            this.addPcPhraseTB.Text = "Add PC Phrase";
            this.addPcPhraseTB.Click += new System.EventHandler(this.addPcPhraseTB_Click);
            // 
            // removeTB
            // 
            this.removeTB.Image = ((System.Drawing.Image)(resources.GetObject("removeTB.Image")));
            this.removeTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeTB.Name = "removeTB";
            this.removeTB.Size = new System.Drawing.Size(102, 22);
            this.removeTB.Text = "Remove Phrase";
            this.removeTB.Click += new System.EventHandler(this.removeTB_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // expandCollapseTB
            // 
            this.expandCollapseTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.expandCollapseTB.Image = ((System.Drawing.Image)(resources.GetObject("expandCollapseTB.Image")));
            this.expandCollapseTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.expandCollapseTB.Name = "expandCollapseTB";
            this.expandCollapseTB.Size = new System.Drawing.Size(105, 22);
            this.expandCollapseTB.Text = "Expand/Collapse All";
            this.expandCollapseTB.Click += new System.EventHandler(this.expandCollapseTB_Click);
            // 
            // npcPhraseMenu
            // 
            this.npcPhraseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPCPhraseToolStripMenuItem,
            this.removePhraseToolStripMenuItem});
            this.npcPhraseMenu.MaximumSize = new System.Drawing.Size(120, 0);
            this.npcPhraseMenu.Name = "contextMenuStrip1";
            this.npcPhraseMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.npcPhraseMenu.Size = new System.Drawing.Size(120, 48);
            // 
            // addPCPhraseToolStripMenuItem
            // 
            this.addPCPhraseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addPCPhraseToolStripMenuItem.Image")));
            this.addPCPhraseToolStripMenuItem.Name = "addPCPhraseToolStripMenuItem";
            this.addPCPhraseToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addPCPhraseToolStripMenuItem.Text = "Add PC Phrase";
            this.addPCPhraseToolStripMenuItem.Click += new System.EventHandler(this.addPCPhraseTSMenuItem_Click);
            // 
            // removePhraseToolStripMenuItem
            // 
            this.removePhraseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removePhraseToolStripMenuItem.Image")));
            this.removePhraseToolStripMenuItem.Name = "removePhraseToolStripMenuItem";
            this.removePhraseToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.removePhraseToolStripMenuItem.Text = "Remove Phrase";
            this.removePhraseToolStripMenuItem.Click += new System.EventHandler(this.removePhraseTSMenuItem_Click);
            // 
            // treeListView
            // 
            this.treeListView.AllColumns.Add(this.textColumn);
            this.treeListView.AllColumns.Add(this.idColumn);
            this.treeListView.AllColumns.Add(this.linkColumn);
            this.treeListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.treeListView.CheckBoxes = false;
            this.treeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.textColumn,
            this.idColumn,
            this.linkColumn});
            this.treeListView.FullRowSelect = true;
            this.treeListView.GridLines = true;
            this.treeListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.treeListView.HideSelection = false;
            this.treeListView.LabelEdit = true;
            this.treeListView.LabelWrap = false;
            this.treeListView.Location = new System.Drawing.Point(0, 49);
            this.treeListView.MultiSelect = false;
            this.treeListView.Name = "treeListView";
            this.treeListView.OwnerDraw = true;
            this.treeListView.SelectAllOnControlA = false;
            this.treeListView.SelectColumnsMenuStaysOpen = false;
            this.treeListView.SelectColumnsOnRightClick = false;
            this.treeListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.treeListView.ShowGroups = false;
            this.treeListView.Size = new System.Drawing.Size(792, 518);
            this.treeListView.SmallImageList = this.imageList1;
            this.treeListView.TabIndex = 1;
            this.treeListView.UseCompatibleStateImageBehavior = false;
            this.treeListView.UseFiltering = true;
            this.treeListView.View = System.Windows.Forms.View.Details;
            this.treeListView.VirtualMode = true;
            this.treeListView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.cellEditStarting);
            this.treeListView.CellEditValidating += new BrightIdeasSoftware.CellEditEventHandler(this.cellEditValidating);
            this.treeListView.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.cellRightClick);
            this.treeListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.afterLabelEdit);
            this.treeListView.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.beforeLabelEdit);
            // 
            // textColumn
            // 
            this.textColumn.AutoCompleteEditor = false;
            this.textColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textColumn.FillsFreeSpace = true;
            this.textColumn.Hideable = false;
            this.textColumn.MinimumWidth = 20;
            this.textColumn.Text = "Text";
            this.textColumn.Width = 525;
            // 
            // idColumn
            // 
            this.idColumn.AutoCompleteEditor = false;
            this.idColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.idColumn.Text = "Id";
            this.idColumn.Width = 130;
            // 
            // linkColumn
            // 
            this.linkColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.linkColumn.Text = "Link";
            this.linkColumn.Width = 130;
            // 
            // pcPhraseMenu
            // 
            this.pcPhraseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNPCPhraseToolStripMenuItem,
            this.removePhraseToolStripMenuItem1,
            this.toolStripSeparator5,
            this.followLinkToolStripMenuItem});
            this.pcPhraseMenu.MaximumSize = new System.Drawing.Size(120, 0);
            this.pcPhraseMenu.Name = "PcPhraseMenu";
            this.pcPhraseMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.pcPhraseMenu.Size = new System.Drawing.Size(120, 98);
            // 
            // addNPCPhraseToolStripMenuItem
            // 
            this.addNPCPhraseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNPCPhraseToolStripMenuItem.Image")));
            this.addNPCPhraseToolStripMenuItem.Name = "addNPCPhraseToolStripMenuItem";
            this.addNPCPhraseToolStripMenuItem.ShowShortcutKeys = false;
            this.addNPCPhraseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNPCPhraseToolStripMenuItem.Text = "Add NPC Phrase";
            this.addNPCPhraseToolStripMenuItem.Click += new System.EventHandler(this.addNPCPhraseTSMenuItem_Click);
            // 
            // removePhraseToolStripMenuItem1
            // 
            this.removePhraseToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("removePhraseToolStripMenuItem1.Image")));
            this.removePhraseToolStripMenuItem1.Name = "removePhraseToolStripMenuItem1";
            this.removePhraseToolStripMenuItem1.ShowShortcutKeys = false;
            this.removePhraseToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.removePhraseToolStripMenuItem1.Text = "Remove Phrase";
            this.removePhraseToolStripMenuItem1.Click += new System.EventHandler(this.removePhraseTSMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(116, 6);
            // 
            // followLinkToolStripMenuItem
            // 
            this.followLinkToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("followLinkToolStripMenuItem.Image")));
            this.followLinkToolStripMenuItem.Name = "followLinkToolStripMenuItem";
            this.followLinkToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.followLinkToolStripMenuItem.Text = "Follow Link";
            this.followLinkToolStripMenuItem.Click += new System.EventHandler(this.followLinkTSMenuItem_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.treeListView);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dialog Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.npcPhraseMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).EndInit();
            this.pcPhraseMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMI;
        private System.Windows.Forms.ToolStripMenuItem saveMI;
        private System.Windows.Forms.ToolStripMenuItem OpenMI;
        private System.Windows.Forms.ToolStripMenuItem exitMI;
        private System.Windows.Forms.ToolStripMenuItem dialogMI;
        private System.Windows.Forms.ToolStripMenuItem addNPCPhraseMI;
        private System.Windows.Forms.ToolStripMenuItem addPCPhraseMI;
        private System.Windows.Forms.ToolStripMenuItem removePhraseMI;
        private System.Windows.Forms.ToolStripMenuItem saveAsMI;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem newMI;
        private System.Windows.Forms.ToolStripMenuItem helpMI;
        private System.Windows.Forms.ToolStripMenuItem aboutMI;
        private System.Windows.Forms.ImageList imageList1;
        private BrightIdeasSoftware.TreeListView treeListView;
        private BrightIdeasSoftware.OLVColumn textColumn;
        private BrightIdeasSoftware.OLVColumn idColumn;
        private BrightIdeasSoftware.OLVColumn linkColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newTB;
        private System.Windows.Forms.ToolStripButton openTB;
        private System.Windows.Forms.ToolStripButton saveTB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton addNpcPhraseTB;
        private System.Windows.Forms.ToolStripButton addPcPhraseTB;
        private System.Windows.Forms.ToolStripButton removeTB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton expandCollapseTB;
        private System.Windows.Forms.ContextMenuStrip npcPhraseMenu;
        private System.Windows.Forms.ToolStripMenuItem addPCPhraseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePhraseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip pcPhraseMenu;
        private System.Windows.Forms.ToolStripMenuItem addNPCPhraseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePhraseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem followLinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

