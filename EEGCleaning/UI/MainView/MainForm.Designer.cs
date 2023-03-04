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
            components = new System.ComponentModel.Container();
            m_plotView = new OxyPlot.WindowsForms.PlotView();
            m_xTrackBar = new TrackBar();
            m_yTrackBar = new TrackBar();
            m_xLabel = new Label();
            m_yLabel = new Label();
            m_icaButton = new Button();
            m_openFileDialog = new OpenFileDialog();
            m_loadButton = new UI.Controls.MenuButton();
            m_loadContextMenuStrip = new ContextMenuStrip(components);
            loadEEGToolStripMenuItem = new ToolStripMenuItem();
            loadTestDataToolStripMenuItem = new ToolStripMenuItem();
            m_saveFileDialog = new SaveFileDialog();
            m_saveButton = new Button();
            m_splitContainer = new SplitContainer();
            m_plotWeightsView = new OxyPlot.WindowsForms.PlotView();
            ((System.ComponentModel.ISupportInitialize)m_xTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)m_yTrackBar).BeginInit();
            m_loadContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)m_splitContainer).BeginInit();
            m_splitContainer.Panel1.SuspendLayout();
            m_splitContainer.Panel2.SuspendLayout();
            m_splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // m_plotView
            // 
            m_plotView.BackColor = SystemColors.Window;
            m_plotView.Dock = DockStyle.Fill;
            m_plotView.Location = new Point(0, 0);
            m_plotView.Name = "m_plotView";
            m_plotView.PanCursor = Cursors.Hand;
            m_plotView.Size = new Size(799, 580);
            m_plotView.TabIndex = 0;
            m_plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            m_plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            m_plotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // m_xTrackBar
            // 
            m_xTrackBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            m_xTrackBar.LargeChange = 2;
            m_xTrackBar.Location = new Point(56, 595);
            m_xTrackBar.Maximum = 100;
            m_xTrackBar.Name = "m_xTrackBar";
            m_xTrackBar.Size = new Size(463, 56);
            m_xTrackBar.TabIndex = 2;
            m_xTrackBar.TickFrequency = 5;
            m_xTrackBar.TickStyle = TickStyle.TopLeft;
            m_xTrackBar.Value = 1;
            m_xTrackBar.Scroll += OnXScale;
            // 
            // m_yTrackBar
            // 
            m_yTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            m_yTrackBar.LargeChange = 2;
            m_yTrackBar.Location = new Point(1052, 32);
            m_yTrackBar.Maximum = 100;
            m_yTrackBar.Name = "m_yTrackBar";
            m_yTrackBar.Orientation = Orientation.Vertical;
            m_yTrackBar.Size = new Size(56, 307);
            m_yTrackBar.TabIndex = 2;
            m_yTrackBar.TickFrequency = 5;
            m_yTrackBar.TickStyle = TickStyle.TopLeft;
            m_yTrackBar.Value = 1;
            m_yTrackBar.Scroll += OnYScale;
            // 
            // m_xLabel
            // 
            m_xLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            m_xLabel.AutoSize = true;
            m_xLabel.Location = new Point(8, 608);
            m_xLabel.Name = "m_xLabel";
            m_xLabel.Size = new Size(42, 20);
            m_xLabel.TabIndex = 1;
            m_xLabel.Text = "Time";
            // 
            // m_yLabel
            // 
            m_yLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            m_yLabel.AutoSize = true;
            m_yLabel.Location = new Point(1052, 9);
            m_yLabel.Name = "m_yLabel";
            m_yLabel.Size = new Size(45, 20);
            m_yLabel.TabIndex = 2;
            m_yLabel.Text = "Ampl";
            // 
            // m_icaButton
            // 
            m_icaButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_icaButton.BackColor = SystemColors.Control;
            m_icaButton.Location = new Point(827, 599);
            m_icaButton.Name = "m_icaButton";
            m_icaButton.Size = new Size(70, 29);
            m_icaButton.TabIndex = 3;
            m_icaButton.Text = "ICA";
            m_icaButton.UseVisualStyleBackColor = false;
            // 
            // m_openFileDialog
            // 
            m_openFileDialog.Filter = "ARFF files|*.arff|All files|*.*";
            // 
            // m_loadButton
            // 
            m_loadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_loadButton.BackColor = SystemColors.Control;
            m_loadButton.Location = new Point(903, 599);
            m_loadButton.Menu = m_loadContextMenuStrip;
            m_loadButton.Name = "m_loadButton";
            m_loadButton.Size = new Size(94, 29);
            m_loadButton.TabIndex = 5;
            m_loadButton.Text = "Load";
            m_loadButton.UseVisualStyleBackColor = false;
            // 
            // m_loadContextMenuStrip
            // 
            m_loadContextMenuStrip.ImageScalingSize = new Size(20, 20);
            m_loadContextMenuStrip.Items.AddRange(new ToolStripItem[] { loadEEGToolStripMenuItem, loadTestDataToolStripMenuItem });
            m_loadContextMenuStrip.Name = "m_loadContextMenuStrip";
            m_loadContextMenuStrip.Size = new Size(178, 52);
            // 
            // loadEEGToolStripMenuItem
            // 
            loadEEGToolStripMenuItem.Name = "loadEEGToolStripMenuItem";
            loadEEGToolStripMenuItem.Size = new Size(177, 24);
            loadEEGToolStripMenuItem.Text = "Load EEG";
            loadEEGToolStripMenuItem.Click += OnLoadEEGData;
            // 
            // loadTestDataToolStripMenuItem
            // 
            loadTestDataToolStripMenuItem.Name = "loadTestDataToolStripMenuItem";
            loadTestDataToolStripMenuItem.Size = new Size(177, 24);
            loadTestDataToolStripMenuItem.Text = "Load Test Data";
            loadTestDataToolStripMenuItem.Click += OnLoadTestData;
            // 
            // m_saveFileDialog
            // 
            m_saveFileDialog.DefaultExt = "arff";
            m_saveFileDialog.Filter = "ARFF files|*.arff|All files|*.*";
            // 
            // m_saveButton
            // 
            m_saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_saveButton.BackColor = SystemColors.Control;
            m_saveButton.Location = new Point(1003, 599);
            m_saveButton.Name = "m_saveButton";
            m_saveButton.Size = new Size(94, 29);
            m_saveButton.TabIndex = 6;
            m_saveButton.Text = "Save";
            m_saveButton.UseVisualStyleBackColor = false;
            m_saveButton.Click += OnSaveData;
            // 
            // m_splitContainer
            // 
            m_splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            m_splitContainer.Location = new Point(8, 9);
            m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            m_splitContainer.Panel1.Controls.Add(m_plotView);
            m_splitContainer.Panel1MinSize = 600;
            // 
            // m_splitContainer.Panel2
            // 
            m_splitContainer.Panel2.Controls.Add(m_plotWeightsView);
            m_splitContainer.Panel2MinSize = 100;
            m_splitContainer.Size = new Size(1038, 580);
            m_splitContainer.SplitterDistance = 799;
            m_splitContainer.TabIndex = 7;
            // 
            // m_plotWeightsView
            // 
            m_plotWeightsView.BackColor = SystemColors.Window;
            m_plotWeightsView.Dock = DockStyle.Fill;
            m_plotWeightsView.Location = new Point(0, 0);
            m_plotWeightsView.Name = "m_plotWeightsView";
            m_plotWeightsView.PanCursor = Cursors.Hand;
            m_plotWeightsView.Size = new Size(235, 580);
            m_plotWeightsView.TabIndex = 1;
            m_plotWeightsView.ZoomHorizontalCursor = Cursors.SizeWE;
            m_plotWeightsView.ZoomRectangleCursor = Cursors.SizeNWSE;
            m_plotWeightsView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(1109, 653);
            Controls.Add(m_splitContainer);
            Controls.Add(m_saveButton);
            Controls.Add(m_loadButton);
            Controls.Add(m_icaButton);
            Controls.Add(m_yLabel);
            Controls.Add(m_xLabel);
            Controls.Add(m_yTrackBar);
            Controls.Add(m_xTrackBar);
            Name = "MainForm";
            Text = "EEG Cleaner";
            Load += OnLoad;
            ((System.ComponentModel.ISupportInitialize)m_xTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)m_yTrackBar).EndInit();
            m_loadContextMenuStrip.ResumeLayout(false);
            m_splitContainer.Panel1.ResumeLayout(false);
            m_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)m_splitContainer).EndInit();
            m_splitContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private SplitContainer m_splitContainer;
        private OxyPlot.WindowsForms.PlotView m_plotWeightsView;
    }
}