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
            this.components = new System.ComponentModel.Container();
            this.m_plotView = new OxyPlot.WindowsForms.PlotView();
            this.m_xTrackBar = new System.Windows.Forms.TrackBar();
            this.m_yTrackBar = new System.Windows.Forms.TrackBar();
            this.m_xLabel = new System.Windows.Forms.Label();
            this.m_yLabel = new System.Windows.Forms.Label();
            this.m_icaButton = new System.Windows.Forms.Button();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_loadButton = new EEGCleaning.UI.Controls.MenuButton();
            this.m_loadContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadEEGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_xTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_yTrackBar)).BeginInit();
            this.m_loadContextMenuStrip.SuspendLayout();
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
            // m_icaButton
            // 
            this.m_icaButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_icaButton.BackColor = System.Drawing.SystemColors.Control;
            this.m_icaButton.Location = new System.Drawing.Point(534, 599);
            this.m_icaButton.Name = "m_icaButton";
            this.m_icaButton.Size = new System.Drawing.Size(70, 29);
            this.m_icaButton.TabIndex = 3;
            this.m_icaButton.Text = "ICA";
            this.m_icaButton.UseVisualStyleBackColor = false;
            this.m_icaButton.Click += new System.EventHandler(this.OnRunICA);
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.Filter = "ARFF files|*.arff|All files|*.*";
            // 
            // m_loadButton
            // 
            this.m_loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_loadButton.BackColor = System.Drawing.SystemColors.Control;
            this.m_loadButton.Location = new System.Drawing.Point(903, 599);
            this.m_loadButton.Menu = this.m_loadContextMenuStrip;
            this.m_loadButton.Name = "m_loadButton";
            this.m_loadButton.Size = new System.Drawing.Size(94, 29);
            this.m_loadButton.TabIndex = 5;
            this.m_loadButton.Text = "Load";
            this.m_loadButton.UseVisualStyleBackColor = false;
            // 
            // m_loadContextMenuStrip
            // 
            this.m_loadContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.m_loadContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadEEGToolStripMenuItem,
            this.loadTestDataToolStripMenuItem});
            this.m_loadContextMenuStrip.Name = "m_loadContextMenuStrip";
            this.m_loadContextMenuStrip.Size = new System.Drawing.Size(178, 52);
            // 
            // loadEEGToolStripMenuItem
            // 
            this.loadEEGToolStripMenuItem.Name = "loadEEGToolStripMenuItem";
            this.loadEEGToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.loadEEGToolStripMenuItem.Text = "Load EEG";
            this.loadEEGToolStripMenuItem.Click += new System.EventHandler(this.OnLoadEEGData);
            // 
            // loadTestDataToolStripMenuItem
            // 
            this.loadTestDataToolStripMenuItem.Name = "loadTestDataToolStripMenuItem";
            this.loadTestDataToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.loadTestDataToolStripMenuItem.Text = "Load Test Data";
            this.loadTestDataToolStripMenuItem.Click += new System.EventHandler(this.OnLoadTestData);
            // 
            // m_saveFileDialog
            // 
            this.m_saveFileDialog.DefaultExt = "arff";
            this.m_saveFileDialog.Filter = "ARFF files|*.arff|All files|*.*";
            // 
            // m_saveButton
            // 
            this.m_saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_saveButton.BackColor = System.Drawing.SystemColors.Control;
            this.m_saveButton.Location = new System.Drawing.Point(1003, 599);
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.Size = new System.Drawing.Size(94, 29);
            this.m_saveButton.TabIndex = 6;
            this.m_saveButton.Text = "Save";
            this.m_saveButton.UseVisualStyleBackColor = false;
            this.m_saveButton.Click += new System.EventHandler(this.OnSaveData);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1109, 653);
            this.Controls.Add(this.m_saveButton);
            this.Controls.Add(this.m_loadButton);
            this.Controls.Add(this.m_icaButton);
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
            this.m_loadContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView m_plotView;
        private TrackBar m_xTrackBar;
        private TrackBar m_yTrackBar;
        private Label m_xLabel;
        private Label m_yLabel;
        private Button m_icaButton;
        private OpenFileDialog m_openFileDialog;
        private UI.Controls.MenuButton m_loadButton;
        private ContextMenuStrip m_loadContextMenuStrip;
        private ToolStripMenuItem loadEEGToolStripMenuItem;
        private ToolStripMenuItem loadTestDataToolStripMenuItem;
        private SaveFileDialog m_saveFileDialog;
        private Button m_saveButton;
    }
}