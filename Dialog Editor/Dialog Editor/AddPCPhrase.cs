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
    partial class AddPCPhrase : Form
    {
        DialogResponse pcResponse;

        public AddPCPhrase(ref DialogResponse pcResponse)
        {
            InitializeComponent();

            this.pcResponse = pcResponse;

            this.CancelButton = cancelButton;
            setupForm();
        }

        private void setupForm()
        {
            String[] ids = GUI.NpcIdGenerator.getIds();
            String[] links = new String[ids.Length + 2];
            for (int i = 0; i < ids.Length; i++)
                links[i] = ids[i];
            links[links.Length - 1] = "End conversation";
            links[links.Length - 2] = "End & switch conversation";
            linkComboBox.Items.AddRange(links);
            linkComboBox.SelectedIndexChanged += 
                new EventHandler(linkCBChanged);

            String[] linksToSwitchTo = new String[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                linksToSwitchTo[i] = ids[i];
            switchComboBox.Items.AddRange(linksToSwitchTo);
            switchComboBox.Enabled = false;
        }

        private void linkCBChanged(object sender, EventArgs e)
        {
            if (((String)(linkComboBox.SelectedItem)).
                Equals("End & switch conversation"))
                switchComboBox.Enabled = true;
            else
                switchComboBox.Enabled = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (dialogTextBox.Text.Length == 0)
            {
                GUI.showError("PC response text must be entered.", "Error");
                return;
            }

            pcResponse.response = dialogTextBox.Text;

            if (linkComboBox.SelectedItem != null)
                pcResponse.link = (String)linkComboBox.SelectedItem;

            if (!pcResponse.link.Equals("") && 
                pcResponse.link.Equals("End & switch conversation"))
            {
                pcResponse.linkType =ResponseLinkType.endAndChangeConversation;
                pcResponse.switchConversation = (String)switchComboBox.
                    SelectedItem;
            }
            else if (!pcResponse.link.Equals("") &&
                pcResponse.link.Equals("End conversation"))
                pcResponse.linkType = ResponseLinkType.endConversation;

            pcResponse.onlyAllowOnce = allowOnceCheckBox.Checked;
            this.Close();
        }
    }
}
