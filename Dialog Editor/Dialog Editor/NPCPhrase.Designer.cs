namespace Dialog_Editor
{
    partial class NPCPhrase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCPhrase));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textLabel = new System.Windows.Forms.Label();
            this.IDLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.IDtextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.voiceFileTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.dialogTextBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.textLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.IDLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.voiceFileTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.dialogTextBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(542, 281);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textLabel
            // 
            this.textLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textLabel.AutoSize = true;
            this.textLabel.Location = new System.Drawing.Point(13, 89);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(28, 13);
            this.textLabel.TabIndex = 0;
            this.textLabel.Text = "Text";
            // 
            // IDLabel
            // 
            this.IDLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.IDLabel.AutoSize = true;
            this.IDLabel.Location = new System.Drawing.Point(18, 189);
            this.IDLabel.Name = "IDLabel";
            this.IDLabel.Size = new System.Drawing.Size(18, 13);
            this.IDLabel.TabIndex = 2;
            this.IDLabel.Text = "ID";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel2.Controls.Add(this.IDtextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.generateButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(58, 184);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(471, 23);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // IDtextBox
            // 
            this.IDtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.IDtextBox.Location = new System.Drawing.Point(0, 1);
            this.IDtextBox.Margin = new System.Windows.Forms.Padding(0);
            this.IDtextBox.Name = "IDtextBox";
            this.IDtextBox.Size = new System.Drawing.Size(386, 20);
            this.IDtextBox.TabIndex = 3;
            // 
            // generateButton
            // 
            this.generateButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.generateButton.Location = new System.Drawing.Point(396, 0);
            this.generateButton.Margin = new System.Windows.Forms.Padding(0);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Voice filename";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // voiceFileTextBox
            // 
            this.voiceFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.voiceFileTextBox.Location = new System.Drawing.Point(58, 214);
            this.voiceFileTextBox.Name = "voiceFileTextBox";
            this.voiceFileTextBox.Size = new System.Drawing.Size(471, 20);
            this.voiceFileTextBox.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Controls.Add(this.cancelButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.OkButton, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(58, 242);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(471, 36);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(394, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(74, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(308, 5);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // dialogTextBox
            // 
            this.dialogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dialogTextBox.Location = new System.Drawing.Point(58, 14);
            this.dialogTextBox.Name = "dialogTextBox";
            this.dialogTextBox.Size = new System.Drawing.Size(471, 164);
            this.dialogTextBox.TabIndex = 0;
            this.dialogTextBox.Text = "";
            // 
            // NPCPhrase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 281);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "NPCPhrase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add NPC Phrase";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.Label IDLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox IDtextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox voiceFileTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.RichTextBox dialogTextBox;


    }
}