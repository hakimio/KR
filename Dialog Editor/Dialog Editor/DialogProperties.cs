using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dialog_Editor.dialog_classes;

namespace Dialog_Editor
{
    partial class DialogProperties : Form
    {
        private Conversation conversation;
        private bool errorsOccured = false;

        public DialogProperties(ref Conversation conversation)
        {
            this.conversation = conversation;
            InitializeComponent();
            nameTextBox.Text = conversation.npcName;
            resetCheckBox.Checked = conversation.resetConversationOnEnd;

            this.CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
            this.AcceptButton = OkButton;
            OkButton.DialogResult = DialogResult.OK;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Equals(""))
            {
                GUI.showError("Name must be entered.", "Error");
                errorsOccured = true;
                return;
            }
            errorsOccured = false;
            conversation.npcName = nameTextBox.Text;
            conversation.resetConversationOnEnd = resetCheckBox.Checked;

            //this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (errorsOccured)
                e.Cancel = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            errorsOccured = false;
            //this.Close();
        }
    }
}
