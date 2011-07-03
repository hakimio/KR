using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Dialog_Editor.dialog_classes;
using BrightIdeasSoftware;
using Dialog_Editor.utils;
using System.Collections;

namespace Dialog_Editor
{
    partial class GUI : Form
    {
        private Conversation conversation;
        private const String VERSION = "0.01 Alpha";
        public static IdGenerator NpcIdGenerator = new IdGenerator();
        private String oldLink;
        private String fileName = "";
        private bool changesMade = false;

        public GUI(String file): this()
        {
            fileName = file;
            if (!file.Equals(""))
            {
                conversation = MyXml.load(fileName);
                this.Text = "Dialog Manager - " + conversation.npcName;
                treeListView.Roots = new ArrayList(conversation.
                    getRootNodes());
                treeListView.ExpandAll();
                changesMade = false;
            }
        }

        public GUI()
        {
            Application.EnableVisualStyles();
            InitializeComponent();

            conversation = new Conversation("NoName");
            this.Text = "Dialog Manager - " + conversation.npcName;   

            setupMenuItems();
            setupTreeListView();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!changesMade)
                return;
            DialogResult result = discardChangesDialog();
            if (result == DialogResult.No)
            {
                saveMiClicked(new object(), new EventArgs());
            }
            else if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        public static void showError(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        #region treelistview stuff
        private void setupTreeListView()
        {
            treeListView.CanExpandGetter = new TreeListView.
                CanExpandGetterDelegate(canExpand);
            treeListView.ChildrenGetter = new TreeListView.
                ChildrenGetterDelegate(childrenGetter);

            textColumn.AspectGetter = new AspectGetterDelegate(textColumnAG);
            textColumn.ImageGetter = new ImageGetterDelegate(imageGetter);
            idColumn.AspectGetter = new AspectGetterDelegate(idColumnAG);
            linkColumn.AspectGetter = new AspectGetterDelegate(linkColumnAG);

            treeListView.Roots = new ArrayList(conversation.getRootNodes());
        }

        private object imageGetter(object x)
        {
            if (x is DialogResponse)
                return 0;
            else
                return 1;
        }

        private String textColumnAG(object x)
        {
            if (x is DialogNode)
                return ((DialogNode)x).npcDialog;
            else
                return ((DialogResponse)x).response;
        }

        private String idColumnAG(object x)
        {
            if (x is DialogNode)
                return ((DialogNode)x).id;
            else
                return "";
        }

        private String linkColumnAG(object x)
        {
            if (x is DialogResponse)
                return ((DialogResponse)x).link;
            else
                return "";
        }

        private bool canExpand(object x)
        {
            if (x is DialogNode)
                return (((DialogNode)x).getResponses().Length > 0);
            else
                return (((DialogResponse)x).childNode != null);
        }

        private ArrayList childrenGetter(object x)
        {
            if (x is DialogNode)
                return new ArrayList(((DialogNode)x).getResponses());
            
            ArrayList a = new ArrayList();

            if (((DialogResponse)x).childNode != null)
                a.Add(((DialogResponse)x).childNode);

            return a;
        }

        #endregion

        #region menubar stuff
        private void setupMenuItems()
        {
            FileMI.DropDown.MaximumSize = new Size(180, 500);
            dialogMI.DropDown.MaximumSize = new Size(190, 500);
            helpMI.DropDown.MaximumSize = new Size(90, 500);

            //Callbacks
            //File menu
            newMI.Click += new EventHandler(newMiClicked);
            OpenMI.Click += new EventHandler(openMiClicked);
            saveMI.Click += new EventHandler(saveMiClicked);
            saveAsMI.Click += new EventHandler(saveAsMiClicked);
            exitMI.Click += new EventHandler(exitMiClicked);
            //Dialog menu
            addNPCPhraseMI.Click += new EventHandler(addNPCPhraseMiClicked);
            addPCPhraseMI.Click += new EventHandler(addPCPhraseMiClicked);
            removePhraseMI.Click += new EventHandler(removePhraseMiClicked);
            //Help menu
            aboutMI.Click += new EventHandler(aboutMiClicked);
        }

        private void aboutMiClicked(Object sender, EventArgs e)
        {
            MessageBox.Show("Dialog Editor for Kalos Reclamation\n"+
                VERSION + "\nTomas Rimkus",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region File menu
        private void newMiClicked(Object sender, EventArgs e)
        {
            DialogResult result;
            if (changesMade)
            {
                result = discardChangesDialog();
                if (result == DialogResult.No)
                {
                    saveMiClicked(new object(), new EventArgs());
                }
                else if (result == DialogResult.Cancel)
                    return;
            }
            Conversation newCon = new Conversation("");
            DialogProperties dp = new DialogProperties(ref newCon);
            dp.Text = "New Dialog";
            result = dp.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            conversation = newCon;
            treeListView.ClearObjects();
            NpcIdGenerator = new IdGenerator();
            fileName = "";
            this.Text = "Dialog Manager - " + conversation.npcName;
            changesMade = false;
        }

        private void openMiClicked(Object sender, EventArgs e)
        {
            if (changesMade)
            {
                DialogResult result = discardChangesDialog();
                if (result == DialogResult.No)
                {
                    saveMiClicked(new object(), new EventArgs());
                }
                else if (result == DialogResult.Cancel)
                    return;
            }
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "Dialog (*.xml)|*.xml";
            openFD.RestoreDirectory = true;
            DialogResult dialogResult = openFD.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                fileName = openFD.FileName.ToString();
                conversation = MyXml.load(fileName);
                NpcIdGenerator = new IdGenerator();
                DialogNode[] nodes = conversation.getDialog();
                for (int i = 0; i < nodes.Length; i++)
                    NpcIdGenerator.addId(nodes[i].id);

                this.Text = "Dialog Manager - " + conversation.npcName;
                treeListView.Roots = new ArrayList(conversation.
                    getRootNodes());
                treeListView.ExpandAll();
                changesMade = false;
            }
        }

        private void saveMiClicked(Object sender, EventArgs e)
        {
            if (fileName == "")
            {
                SaveFileDialog saveFD = new SaveFileDialog();
                saveFD.Filter = "Dialog (*.xml)|*.xml";
                saveFD.RestoreDirectory = true;

                if (saveFD.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFD.FileName.ToString();
                }
                else
                    return;
            }

            MyXml.save(conversation, fileName);
            this.Text = "Dialog Editor - " + conversation.npcName;
            changesMade = false;
        }

        private void saveAsMiClicked(Object sender, EventArgs e)
        {
            SaveFileDialog saveFD = new SaveFileDialog();
            saveFD.Filter = "Dialog (*.xml)|*.xml";
            saveFD.RestoreDirectory = true;

            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFD.FileName.ToString();
                MyXml.save(conversation, fileName);
                this.Text = "Dialog Editor - " + conversation.npcName;
                changesMade = false;
            }
        }

        private void exitMiClicked(Object sender, EventArgs e)
        {
            this.Close();
        }

        private DialogResult discardChangesDialog()
        {
            return MessageBox.Show("Discard changes?", "Changes",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        #endregion

        #region Dialog menu
        private void addNPCPhraseMiClicked(Object sender, EventArgs e)
        {
            DialogResponse selectedResponse = null;

            if (treeListView.SelectedObject != null
                && treeListView.SelectedObject is DialogResponse)
            {
                selectedResponse = (DialogResponse)treeListView.SelectedObject;
                
                if (selectedResponse.childNode != null)
                {
                    DialogResult result;
                    result = MessageBox.Show("PC phrase already contains NPC"+
                    " phrase. Do you want to replace it?", "Replacement", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return;
                }
            }
            DialogNode npcMsg = new DialogNode("", "");
            Form addNPCPhraseForm = new NPCPhrase(ref npcMsg);
            addNPCPhraseForm.ShowDialog(this);

            if (npcMsg.npcDialog.Equals(""))
                return;

            if (selectedResponse == null)
            {
                conversation.addRootNode(npcMsg);
                treeListView.AddObject(npcMsg);
            }
            else
            {
                selectedResponse.childNode = npcMsg;
                selectedResponse.link = npcMsg.id;
                conversation.addDialogNode(npcMsg);
                treeListView.RefreshObject(selectedResponse);
                treeListView.Expand(selectedResponse);
            }

            if (!changesMade)
                this.Text = this.Text + "*";
            changesMade = true;
        }

        private void addPCPhraseMiClicked(Object sender, EventArgs e)
        {
            if (!(treeListView.SelectedObject != null
                && treeListView.SelectedObject is DialogNode))
            {
                showError("Please select some NPC phrase.", "Error");
                return;
            }

            DialogNode selectedNode = (DialogNode)treeListView.SelectedObject;
            DialogResponse pcResponse = new DialogResponse("", "", false);
            Form addPCPhraseForm = new AddPCPhrase(ref pcResponse);
            addPCPhraseForm.ShowDialog(this);

            if (pcResponse.response.Equals(""))
                return;

            selectedNode.addResponse(pcResponse);
            treeListView.RefreshObject(selectedNode);
            treeListView.Expand(selectedNode);

            if (!changesMade)
                this.Text = this.Text + "*";
            changesMade = true;
        }

        private void removePhraseMiClicked(Object sender, EventArgs e)
        {
            if (treeListView.SelectedObject == null)
            {
                showError("Some phrase must be selected.", "Error");
                return;
            }

            if (treeListView.SelectedObject is DialogNode)
            {
                DialogNode node = (DialogNode)treeListView.SelectedObject;
                DialogResponse response = (DialogResponse)treeListView.
                    GetParent(node);
                if (response == null)
                {
                    treeListView.RemoveObject(node);
                    conversation.removeRootNode(node);
                }
                else
                {
                    conversation.removeNode(node);
                    response.childNode = null;
                    response.link = "";
                    treeListView.RefreshObject(response);
                }
            }
            else
            {
                DialogResponse response = (DialogResponse)treeListView.
                    SelectedObject;
                DialogNode node = (DialogNode)treeListView.GetParent(response);
                node.removeResponse(response);
                treeListView.RefreshObject(node);
            }

            if (!changesMade)
                this.Text = this.Text + "*";
            changesMade = true;
        }

        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            DialogProperties prop = new DialogProperties(ref conversation);
            DialogResult result = prop.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            this.Text = "Dialog Editor - " + conversation.npcName;

            if (!changesMade)
                this.Text = this.Text + "*";
            changesMade = true;
        }
        #endregion

        #endregion

        #region treeListView callbacks

        private void beforeLabelEdit(object sender, LabelEditEventArgs e)
        {

        }

        private void afterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null || e.Label.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }

            if (treeListView.SelectedObject is DialogNode)
            {
                DialogNode node = (DialogNode)treeListView.SelectedObject;
                node.npcDialog = e.Label;
            }
            else
            {
                DialogResponse r = (DialogResponse)treeListView.SelectedObject;
                r.response = e.Label;
            }
        }

        private void cellEditStarting(object sender, CellEditEventArgs e)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            String[] ids = GUI.NpcIdGenerator.getIds();
            //Don't allow editing response link if there are any child NPC 
            //phrases.
            if (e.ListViewItem.RowObject is DialogResponse &&
                e.SubItemIndex == 2 &&
                !(((DialogResponse)e.RowObject).childNode != null))
            {
                oldLink = ((DialogResponse)e.RowObject).link;
                String[] links = new String[ids.Length + 2];
                for (int i = 0; i < ids.Length; i++)
                    links[i] = ids[i];
                links[links.Length - 1] = "End conversation";
                links[links.Length - 2] = "End & switch conversation";
                comboBox.Items.AddRange(links);
                comboBox.SelectedIndexChanged +=
                    new EventHandler(linkCBChanged);
                if (oldLink.Equals(""))
                    comboBox.SelectedIndex = -1;
                else
                    comboBox.SelectedItem = oldLink;
                comboBox.Bounds = e.CellBounds;
                e.Control = comboBox;
            }
            else if (!(e.RowObject is DialogNode && e.SubItemIndex == 1))
                e.Cancel = true;
        }

        private void linkCBChanged(object sender, EventArgs e)
        {
            String selected = (String)((ComboBox)sender).SelectedItem;
            if (selected == null ||
                !selected.Equals("End & switch conversation"))
                return;

            Object link = "";
            DialogResult result = ComboboxInputBox("Dialog to switch to",
              "Select dialog to switch to", ref link, NpcIdGenerator.getIds());
            if (result == DialogResult.OK)
            {
                DialogResponse r = (DialogResponse)treeListView.SelectedObject;
                r.switchConversation = (String)link;
                r.linkType = ResponseLinkType.endAndChangeConversation;
            }
            else
            {
                if (oldLink.Equals(""))
                    ((ComboBox)sender).SelectedIndex = -1;
                else
                    ((ComboBox)sender).SelectedItem = oldLink;
            }
        }

        private DialogResult ComboboxInputBox(string title, string promptText,
            ref Object value, Object[] items)
        {
            Form form = new Form();
            Label label = new Label();
            ComboBox comboBox = new ComboBox();
            comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            comboBox.Items.AddRange(items);
            comboBox.SelectedIndex = 0;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            comboBox.SetBounds(12, 40, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            comboBox.Anchor = comboBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, comboBox, buttonOk, 
				buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10),
                form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = comboBox.SelectedItem;
            return dialogResult;
        }

        private void cellEditValidating(object sender, CellEditEventArgs e)
        {
            if (e.SubItemIndex == 2 && e.RowObject is DialogResponse)
            {
                DialogResponse r = (DialogResponse)e.RowObject;
                if (((ComboBox)e.Control).SelectedItem == null)
                {
                    treeListView.RefreshObject(r);
                    return;
                }
                r.link = (String)((ComboBox)e.Control).SelectedItem;
                if (r.link.Equals("End conversation"))
                    r.linkType = ResponseLinkType.endConversation;
                else
                    r.linkType = ResponseLinkType.dialogNode;

                treeListView.RefreshObject(r);
            }
            else if (e.SubItemIndex == 1 && e.RowObject is DialogNode)
            {
                String id = (String)e.NewValue;
                DialogNode node = (DialogNode)e.RowObject;
                if (NpcIdGenerator.contains(id) && !id.Equals(node.id))
                {
                    showError("String is not unique", "Error");
                    e.NewValue = node.id;
                    e.Cancel = true;
                    return;
                }
                if (id.Equals(""))
                {
                    showError("Id can not be empty.", "Error");
                    e.NewValue = node.id;
                    e.Cancel = true;
                    return;
                }

                NpcIdGenerator.removeId(node.id);
                node.id = (String)e.NewValue;
                NpcIdGenerator.addId(node.id);
                treeListView.RefreshObject(node);

                //change links of responses where link == oldId
                List<DialogResponse> responses = new List<DialogResponse>();
                for (int i = 0; i < conversation.getRootNodes().Length; i++)
                {
                    DialogNode n = conversation.getRootNode(i);
                    responses.AddRange(getResponses(n));
                }
                for (int i = 0; i < responses.Count; i++)
                {
                    DialogResponse r = responses[i];
                    if (r.link.Equals((String)e.Value))
                    {
                        r.link = node.id;
                        treeListView.RefreshObject(r);
                    }
                }
            }

            if (!changesMade)
                this.Text = this.Text + "*";
            changesMade = true;
        }

        private List<DialogResponse> getResponses(DialogNode node)
        {
            List<DialogResponse> responses = new List<DialogResponse>();

            foreach (DialogResponse r in node.getResponses())
            {
                responses.Add(r);
                if (r.childNode != null)
                    responses.AddRange(getResponses(r.childNode));
            }

            return responses;
        }

        private void cellRightClick(object sender, CellRightClickEventArgs e)
        {
            if (treeListView.SelectedObject == null)
            {
                pcPhraseMenu.Items[1].Visible = false;
                pcPhraseMenu.Items[2].Visible = false;
                pcPhraseMenu.Items[3].Visible = false;
                e.MenuStrip = pcPhraseMenu;
            }
            else if (treeListView.SelectedObject is DialogNode)
                e.MenuStrip = npcPhraseMenu;
            else if (treeListView.SelectedObject is DialogResponse)
            {
                pcPhraseMenu.Items[1].Visible = true;
                pcPhraseMenu.Items[2].Visible = true;
                pcPhraseMenu.Items[3].Visible = true;
                e.MenuStrip = pcPhraseMenu;
            }
        }

        #endregion

        #region toolBar
        private void newTB_Click(object sender, EventArgs e)
        {
            newMiClicked(sender, e);
        }

        private void openTB_Click(object sender, EventArgs e)
        {
            openMiClicked(sender, e);
        }

        private void saveTB_Click(object sender, EventArgs e)
        {
            saveMiClicked(sender, e);
        }

        private void addNpcPhraseTB_Click(object sender, EventArgs e)
        {
            addNPCPhraseMiClicked(sender, e);
        }

        private void addPcPhraseTB_Click(object sender, EventArgs e)
        {
            addPCPhraseMiClicked(sender, e);
        }

        private void removeTB_Click(object sender, EventArgs e)
        {
            removePhraseMiClicked(sender, e);
        }

        private void expandCollapseTB_Click(object sender, EventArgs e)
        {
            if (treeListView.Items.Count == 0)
                return;

            ListViewItem item = treeListView.Items[0];
            Object obj = treeListView.GetModelObject(item.Index);
            
            if (treeListView.IsExpanded(obj))
                treeListView.CollapseAll();
            else
                treeListView.ExpandAll();
        }
        #endregion

        #region right-click menu callbacks
        private void addPCPhraseTSMenuItem_Click(object sender, EventArgs e)
        {
            addPCPhraseMiClicked(sender, e);
        }

        private void removePhraseTSMenuItem_Click(object sender, EventArgs e)
        {
            removePhraseMiClicked(sender, e);
        }

        private void addNPCPhraseTSMenuItem_Click(object sender, EventArgs e)
        {
            addNPCPhraseMiClicked(sender, e);
        }

        private void followLinkTSMenuItem_Click(object sender, EventArgs e)
        {
            DialogResponse resp = (DialogResponse)treeListView.SelectedObject;
            DialogNode node = conversation.getNodeById(resp.link);
            if (node == null)
            {
                showError("Linked NPC Phrase was not found.", "Not found");
                return;
            }
            treeListView.SelectedObject = node;
            treeListView.EnsureModelVisible(node);
        }
        #endregion

        private void GUI_Load(object sender, EventArgs e)
        {
            if (!File.Exists("settings.xml"))
                return;

            Settings settings = MyXml.loadSettings("settings.xml");
            this.Location = settings.windowLocation;
            this.Size = settings.windowSize;

            idColumn.Width = settings.idColumnWidth;
            linkColumn.Width = settings.linkColumnWidth;
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings settings = new Settings(idColumn.Width, linkColumn.Width,
                this.Size, this.Location);

            if (this.WindowState == FormWindowState.Minimized)
            {
                settings.windowSize = this.RestoreBounds.Size;
                settings.windowLocation = this.RestoreBounds.Location;
            }
            MyXml.saveSettings(settings, "settings.xml");
        }
    }
}
