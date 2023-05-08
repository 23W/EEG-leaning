namespace EEGCleaning.UI.Dialogs
{
    partial class RecordRangeForm
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
            m_okButton = new Button();
            m_nameLabel = new Label();
            m_nameTextBox = new TextBox();
            m_fromLabel = new Label();
            m_fromTextBox = new TextBox();
            m_toLabel = new Label();
            m_toTextBox = new TextBox();
            SuspendLayout();
            // 
            // m_okButton
            // 
            m_okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_okButton.DialogResult = DialogResult.OK;
            m_okButton.Location = new Point(339, 195);
            m_okButton.Name = "m_okButton";
            m_okButton.Size = new Size(94, 29);
            m_okButton.TabIndex = 3;
            m_okButton.Text = "Ok";
            m_okButton.UseVisualStyleBackColor = true;
            // 
            // m_nameLabel
            // 
            m_nameLabel.AutoSize = true;
            m_nameLabel.Location = new Point(28, 24);
            m_nameLabel.Name = "m_nameLabel";
            m_nameLabel.Size = new Size(49, 20);
            m_nameLabel.TabIndex = 0;
            m_nameLabel.Text = "Name";
            // 
            // m_nameTextBox
            // 
            m_nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_nameTextBox.Location = new Point(94, 21);
            m_nameTextBox.Name = "m_nameTextBox";
            m_nameTextBox.Size = new Size(321, 27);
            m_nameTextBox.TabIndex = 0;
            m_nameTextBox.Validating += OnNameValidating;
            // 
            // m_fromLabel
            // 
            m_fromLabel.AutoSize = true;
            m_fromLabel.Location = new Point(28, 80);
            m_fromLabel.Name = "m_fromLabel";
            m_fromLabel.Size = new Size(43, 20);
            m_fromLabel.TabIndex = 1;
            m_fromLabel.Text = "From";
            // 
            // m_fromTextBox
            // 
            m_fromTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_fromTextBox.Location = new Point(94, 77);
            m_fromTextBox.Name = "m_fromTextBox";
            m_fromTextBox.ReadOnly = true;
            m_fromTextBox.Size = new Size(321, 27);
            m_fromTextBox.TabIndex = 1;
            // 
            // m_toLabel
            // 
            m_toLabel.AutoSize = true;
            m_toLabel.Location = new Point(28, 120);
            m_toLabel.Name = "m_toLabel";
            m_toLabel.Size = new Size(25, 20);
            m_toLabel.TabIndex = 2;
            m_toLabel.Text = "To";
            // 
            // m_toTextBox
            // 
            m_toTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_toTextBox.Location = new Point(94, 117);
            m_toTextBox.Name = "m_toTextBox";
            m_toTextBox.ReadOnly = true;
            m_toTextBox.Size = new Size(321, 27);
            m_toTextBox.TabIndex = 2;
            // 
            // RecordRangeForm
            // 
            AcceptButton = m_okButton;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(445, 236);
            Controls.Add(m_toTextBox);
            Controls.Add(m_fromTextBox);
            Controls.Add(m_nameTextBox);
            Controls.Add(m_toLabel);
            Controls.Add(m_fromLabel);
            Controls.Add(m_nameLabel);
            Controls.Add(m_okButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RecordRangeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Range";
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button m_okButton;
        private Label m_nameLabel;
        private TextBox m_nameTextBox;
        private Label m_fromLabel;
        private TextBox m_fromTextBox;
        private Label m_toLabel;
        private TextBox m_toTextBox;
    }
}