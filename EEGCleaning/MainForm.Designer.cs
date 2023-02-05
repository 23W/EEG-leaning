namespace EEGCleaning
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_plotView = new OxyPlot.WindowsForms.PlotView();
            this.m_xTrackBar = new System.Windows.Forms.TrackBar();
            this.m_yTrackBar = new System.Windows.Forms.TrackBar();
            this.m_xLabel = new System.Windows.Forms.Label();
            this.m_yLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_xTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_yTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // m_plotView
            // 
            this.m_plotView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_plotView.BackColor = System.Drawing.Color.White;
            this.m_plotView.Location = new System.Drawing.Point(8, 9);
            this.m_plotView.Name = "m_plotView";
            this.m_plotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.m_plotView.Size = new System.Drawing.Size(1038, 580);
            this.m_plotView.TabIndex = 0;
            this.m_plotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.m_plotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.m_plotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // m_xTrackBar
            // 
            this.m_xTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_xTrackBar.LargeChange = 2;
            this.m_xTrackBar.Location = new System.Drawing.Point(56, 595);
            this.m_xTrackBar.Maximum = 100;
            this.m_xTrackBar.Name = "m_xTrackBar";
            this.m_xTrackBar.Size = new System.Drawing.Size(463, 56);
            this.m_xTrackBar.TabIndex = 2;
            this.m_xTrackBar.TickFrequency = 5;
            this.m_xTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.m_xTrackBar.Value = 1;
            this.m_xTrackBar.Scroll += new System.EventHandler(this.OnXScale);
            // 
            // m_yTrackBar
            // 
            this.m_yTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_yTrackBar.LargeChange = 2;
            this.m_yTrackBar.Location = new System.Drawing.Point(1052, 32);
            this.m_yTrackBar.Maximum = 100;
            this.m_yTrackBar.Name = "m_yTrackBar";
            this.m_yTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.m_yTrackBar.Size = new System.Drawing.Size(56, 307);
            this.m_yTrackBar.TabIndex = 2;
            this.m_yTrackBar.TickFrequency = 5;
            this.m_yTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.m_yTrackBar.Value = 1;
            this.m_yTrackBar.Scroll += new System.EventHandler(this.OnYScale);
            // 
            // m_xLabel
            // 
            this.m_xLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_xLabel.AutoSize = true;
            this.m_xLabel.Location = new System.Drawing.Point(8, 608);
            this.m_xLabel.Name = "m_xLabel";
            this.m_xLabel.Size = new System.Drawing.Size(42, 20);
            this.m_xLabel.TabIndex = 1;
            this.m_xLabel.Text = "Time";
            // 
            // m_yLabel
            // 
            this.m_yLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_yLabel.AutoSize = true;
            this.m_yLabel.Location = new System.Drawing.Point(1052, 9);
            this.m_yLabel.Name = "m_yLabel";
            this.m_yLabel.Size = new System.Drawing.Size(45, 20);
            this.m_yLabel.TabIndex = 2;
            this.m_yLabel.Text = "Ampl";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1109, 653);
            this.Controls.Add(this.m_yLabel);
            this.Controls.Add(this.m_xLabel);
            this.Controls.Add(this.m_yTrackBar);
            this.Controls.Add(this.m_xTrackBar);
            this.Controls.Add(this.m_plotView);
            this.Name = "MainForm";
            this.Text = "EEG Cleaner";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.m_xTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_yTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView m_plotView;
        private TrackBar m_xTrackBar;
        private TrackBar m_yTrackBar;
        private Label m_xLabel;
        private Label m_yLabel;
    }
}