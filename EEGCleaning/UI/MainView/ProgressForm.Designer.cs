namespace EEGCleaning.UI.MainView
{
    partial class ProgressForm
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
            m_progressBar = new ProgressBar();
            m_Label = new Label();
            SuspendLayout();
            // 
            // m_progressBar
            // 
            m_progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_progressBar.Location = new Point(12, 52);
            m_progressBar.MarqueeAnimationSpeed = 25;
            m_progressBar.Name = "m_progressBar";
            m_progressBar.Size = new Size(700, 23);
            m_progressBar.Style = ProgressBarStyle.Marquee;
            m_progressBar.TabIndex = 0;
            // 
            // m_Label
            // 
            m_Label.AutoSize = true;
            m_Label.Location = new Point(12, 21);
            m_Label.Name = "m_Label";
            m_Label.Size = new Size(74, 15);
            m_Label.TabIndex = 1;
            m_Label.Text = "Please wait...";
            // 
            // ProgressForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(724, 109);
            ControlBox = false;
            Controls.Add(m_Label);
            Controls.Add(m_progressBar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProgressForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Wait";
            Load += OnLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar m_progressBar;
        private Label m_Label;
    }
}