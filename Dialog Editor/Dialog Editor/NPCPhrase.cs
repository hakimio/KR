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
    partial class NPCPhrase : Form
    {
        private DialogNode npcMsg;

        public NPCPhrase(ref DialogNode npcMsg)
        {
            InitializeComponent();

            this.npcMsg = npcMsg;

            this.CancelButton = cancelButton;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (dialogTextBox.Text.Length == 0)
                GUI.showError("Text must be entered first", "Error");
            else
                IDtextBox.Text = IdGenerator.getNpcId(dialogTextBox.Text);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (IDtextBox.Text.Length == 0 || dialogTextBox.Text.Length == 0)
            {
                GUI.showError("Text & id must be entered", "Error");
                return;
            }
            
            if (!GUI.NpcIdGenerator.addId(IDtextBox.Text))
            {
                GUI.showError("Id must be unique", "Error");
                return;
            }

            npcMsg.npcDialog = dialogTextBox.Text;
            npcMsg.id = IDtextBox.Text;
            npcMsg.npcVoiceFile = voiceFileTextBox.Text;

            this.Close();
        }
    }
}
