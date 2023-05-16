namespace EEGCleaning.UI.MainView
{
    partial class SinPropertiesForm
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
            m_amplitudeLabel = new Label();
            m_amplitudeTextBox = new TextBox();
            m_freqLabel = new Label();
            m_freqTextBox = new TextBox();
            m_OkButton = new Button();
            SuspendLayout();
            // 
            // m_amplitudeLabel
            // 
            m_amplitudeLabel.AutoSize = true;
            m_amplitudeLabel.Location = new Point(12, 23);
            m_amplitudeLabel.Name = "m_amplitudeLabel";
            m_amplitudeLabel.Size = new Size(98, 15);
            m_amplitudeLabel.TabIndex = 1;
            m_amplitudeLabel.Text = "Amplitude (mkV)";
            // 
            // m_amplitudeTextBox
            // 
            m_amplitudeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_amplitudeTextBox.Location = new Point(116, 20);
            m_amplitudeTextBox.Name = "m_amplitudeTextBox";
            m_amplitudeTextBox.Size = new Size(133, 23);
            m_amplitudeTextBox.TabIndex = 1;
            m_amplitudeTextBox.Validating += OnValueValidating;
            // 
            // m_freqLabel
            // 
            m_freqLabel.AutoSize = true;
            m_freqLabel.Location = new Point(12, 64);
            m_freqLabel.Name = "m_freqLabel";
            m_freqLabel.Size = new Size(87, 15);
            m_freqLabel.TabIndex = 2;
            m_freqLabel.Text = "Frequency (Hz)";
            // 
            // m_freqTextBox
            // 
            m_freqTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_freqTextBox.Location = new Point(116, 61);
            m_freqTextBox.Name = "m_freqTextBox";
            m_freqTextBox.Size = new Size(133, 23);
            m_freqTextBox.TabIndex = 2;
            m_freqTextBox.Validating += OnValueValidating;
            // 
            // m_OkButton
            // 
            m_OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_OkButton.BackColor = Color.Transparent;
            m_OkButton.DialogResult = DialogResult.OK;
            m_OkButton.Location = new Point(164, 114);
            m_OkButton.Name = "m_OkButton";
            m_OkButton.Size = new Size(85, 35);
            m_OkButton.TabIndex = 3;
            m_OkButton.Text = "Ok";
            m_OkButton.UseVisualStyleBackColor = false;
            m_OkButton.Click += OnOkClick;
            // 
            // SinPropertiesForm
            // 
            AcceptButton = m_OkButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(261, 161);
            Controls.Add(m_OkButton);
            Controls.Add(m_freqTextBox);
            Controls.Add(m_amplitudeTextBox);
            Controls.Add(m_freqLabel);
            Controls.Add(m_amplitudeLabel);
            MaximumSize = new Size(800, 200);
            MinimumSize = new Size(277, 200);
            Name = "SinPropertiesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sin Wave Properties";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label m_amplitudeLabel;
        private TextBox m_amplitudeTextBox;
        private Label m_freqLabel;
        private TextBox m_freqTextBox;
        private Button m_OkButton;
    }
}