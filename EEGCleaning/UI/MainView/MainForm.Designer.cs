using EEGCleaning.UI.Controls;

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
            m_icaButton = new MenuButton();
            m_icaContextMenuStrip = new ContextMenuStrip(components);
            m_standradICAToolStripMenuItem = new ToolStripMenuItem();
            m_normalizedICAToolStripMenuItem = new ToolStripMenuItem();
            m_openFileDialog = new OpenFileDialog();
            m_loadButton = new MenuButton();
            m_loadContextMenuStrip = new ContextMenuStrip(components);
            resetDataToolStripMenuItem = new ToolStripMenuItem();
            loadEEGToolStripMenuItem = new ToolStripMenuItem();
            loadTestDataToolStripMenuItem = new ToolStripMenuItem();
            m_saveFileDialog = new SaveFileDialog();
            m_saveButton = new Button();
            m_splitContainer = new SplitContainer();
            m_plorViewControlsLayoutPanel = new TableLayoutPanel();
            m_plotViewHScrollBar = new HScrollBar();
            m_plotWeightsView = new OxyPlot.WindowsForms.PlotView();
            m_amplComboBox = new ComboBox();
            m_speedComboBox = new ComboBox();
            m_icaComposeButton = new Button();
            m_bottomControlsLayoutPanel = new TableLayoutPanel();
            m_filterLowCutOffComboBox = new ComboBox();
            m_filterHighCutOffComboBox = new ComboBox();
            m_autoButton = new MenuButton();
            m_autoContextMenuStrip = new ContextMenuStrip(components);
            autoCleanToolStripMenuItem = new ToolStripMenuItem();
            autoRangesToolStripMenuItem = new ToolStripMenuItem();
            m_icaContextMenuStrip.SuspendLayout();
            m_loadContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)m_splitContainer).BeginInit();
            m_splitContainer.Panel1.SuspendLayout();
            m_splitContainer.Panel2.SuspendLayout();
            m_splitContainer.SuspendLayout();
            m_plorViewControlsLayoutPanel.SuspendLayout();
            m_bottomControlsLayoutPanel.SuspendLayout();
            m_autoContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // m_plotView
            // 
            m_plotView.BackColor = SystemColors.Window;
            m_plotView.Dock = DockStyle.Fill;
            m_plotView.Location = new Point(3, 2);
            m_plotView.Margin = new Padding(3, 2, 3, 2);
            m_plotView.Name = "m_plotView";
            m_plotView.PanCursor = Cursors.Hand;
            m_plotView.Size = new Size(963, 581);
            m_plotView.TabIndex = 0;
            m_plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            m_plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            m_plotView.ZoomVerticalCursor = Cursors.SizeNS;
            m_plotView.Resize += OnPlotViewResized;
            // 
            // m_icaButton
            // 
            m_icaButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_icaButton.BackColor = Color.Transparent;
            m_icaButton.Location = new Point(933, 3);
            m_icaButton.Margin = new Padding(5, 3, 5, 3);
            m_icaButton.Menu = m_icaContextMenuStrip;
            m_icaButton.Name = "m_icaButton";
            m_icaButton.Size = new Size(61, 30);
            m_icaButton.TabIndex = 3;
            m_icaButton.Text = "ICA";
            m_icaButton.UseVisualStyleBackColor = false;
            // 
            // m_icaContextMenuStrip
            // 
            m_icaContextMenuStrip.ImageScalingSize = new Size(20, 20);
            m_icaContextMenuStrip.Items.AddRange(new ToolStripItem[] { m_standradICAToolStripMenuItem, m_normalizedICAToolStripMenuItem });
            m_icaContextMenuStrip.Name = "m_icaContextMenuStrip";
            m_icaContextMenuStrip.Size = new Size(165, 48);
            // 
            // m_standradICAToolStripMenuItem
            // 
            m_standradICAToolStripMenuItem.Name = "m_standradICAToolStripMenuItem";
            m_standradICAToolStripMenuItem.Size = new Size(164, 22);
            m_standradICAToolStripMenuItem.Text = "Standrad ICA";
            // 
            // m_normalizedICAToolStripMenuItem
            // 
            m_normalizedICAToolStripMenuItem.Name = "m_normalizedICAToolStripMenuItem";
            m_normalizedICAToolStripMenuItem.Size = new Size(164, 22);
            m_normalizedICAToolStripMenuItem.Text = "Normalize Power";
            // 
            // m_openFileDialog
            // 
            m_openFileDialog.Filter = "ARFF files|*.arff|EDF files (*.edf)|*.edf";
            // 
            // m_loadButton
            // 
            m_loadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_loadButton.BackColor = Color.Transparent;
            m_loadButton.Location = new Point(1082, 3);
            m_loadButton.Margin = new Padding(5, 3, 5, 3);
            m_loadButton.Menu = m_loadContextMenuStrip;
            m_loadButton.Name = "m_loadButton";
            m_loadButton.Size = new Size(82, 30);
            m_loadButton.TabIndex = 5;
            m_loadButton.Text = "Load";
            m_loadButton.UseVisualStyleBackColor = false;
            // 
            // m_loadContextMenuStrip
            // 
            m_loadContextMenuStrip.ImageScalingSize = new Size(20, 20);
            m_loadContextMenuStrip.Items.AddRange(new ToolStripItem[] { resetDataToolStripMenuItem, loadEEGToolStripMenuItem, loadTestDataToolStripMenuItem });
            m_loadContextMenuStrip.Name = "m_loadContextMenuStrip";
            m_loadContextMenuStrip.Size = new Size(180, 70);
            // 
            // resetDataToolStripMenuItem
            // 
            resetDataToolStripMenuItem.Name = "resetDataToolStripMenuItem";
            resetDataToolStripMenuItem.Size = new Size(179, 22);
            resetDataToolStripMenuItem.Text = "Reset Data to Origin";
            resetDataToolStripMenuItem.Click += OnResetDataToOrigin;
            // 
            // loadEEGToolStripMenuItem
            // 
            loadEEGToolStripMenuItem.Name = "loadEEGToolStripMenuItem";
            loadEEGToolStripMenuItem.Size = new Size(179, 22);
            loadEEGToolStripMenuItem.Text = "Load EEG";
            loadEEGToolStripMenuItem.Click += OnLoadEEGData;
            // 
            // loadTestDataToolStripMenuItem
            // 
            loadTestDataToolStripMenuItem.Name = "loadTestDataToolStripMenuItem";
            loadTestDataToolStripMenuItem.Size = new Size(179, 22);
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
            m_saveButton.BackColor = Color.RoyalBlue;
            m_saveButton.ForeColor = Color.White;
            m_saveButton.Location = new Point(1174, 3);
            m_saveButton.Margin = new Padding(5, 3, 5, 3);
            m_saveButton.Name = "m_saveButton";
            m_saveButton.Size = new Size(82, 30);
            m_saveButton.TabIndex = 6;
            m_saveButton.Text = "Save";
            m_saveButton.UseVisualStyleBackColor = false;
            m_saveButton.Click += OnSaveData;
            // 
            // m_splitContainer
            // 
            m_splitContainer.Dock = DockStyle.Fill;
            m_splitContainer.Location = new Point(0, 0);
            m_splitContainer.Margin = new Padding(3, 2, 3, 2);
            m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            m_splitContainer.Panel1.Controls.Add(m_plorViewControlsLayoutPanel);
            m_splitContainer.Panel1MinSize = 600;
            // 
            // m_splitContainer.Panel2
            // 
            m_splitContainer.Panel2.Controls.Add(m_plotWeightsView);
            m_splitContainer.Panel2MinSize = 100;
            m_splitContainer.Size = new Size(1261, 611);
            m_splitContainer.SplitterDistance = 969;
            m_splitContainer.TabIndex = 0;
            // 
            // m_plorViewControlsLayoutPanel
            // 
            m_plorViewControlsLayoutPanel.ColumnCount = 1;
            m_plorViewControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_plorViewControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            m_plorViewControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            m_plorViewControlsLayoutPanel.Controls.Add(m_plotViewHScrollBar, 0, 1);
            m_plorViewControlsLayoutPanel.Controls.Add(m_plotView, 0, 0);
            m_plorViewControlsLayoutPanel.Dock = DockStyle.Fill;
            m_plorViewControlsLayoutPanel.Location = new Point(0, 0);
            m_plorViewControlsLayoutPanel.Margin = new Padding(3, 2, 3, 2);
            m_plorViewControlsLayoutPanel.Name = "m_plorViewControlsLayoutPanel";
            m_plorViewControlsLayoutPanel.RowCount = 2;
            m_plorViewControlsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            m_plorViewControlsLayoutPanel.RowStyles.Add(new RowStyle());
            m_plorViewControlsLayoutPanel.Size = new Size(969, 611);
            m_plorViewControlsLayoutPanel.TabIndex = 0;
            // 
            // m_plotViewHScrollBar
            // 
            m_plotViewHScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            m_plotViewHScrollBar.Location = new Point(0, 585);
            m_plotViewHScrollBar.Name = "m_plotViewHScrollBar";
            m_plotViewHScrollBar.Size = new Size(969, 26);
            m_plotViewHScrollBar.TabIndex = 3;
            m_plotViewHScrollBar.Scroll += OnHScroll;
            // 
            // m_plotWeightsView
            // 
            m_plotWeightsView.BackColor = SystemColors.Window;
            m_plotWeightsView.Dock = DockStyle.Fill;
            m_plotWeightsView.Location = new Point(0, 0);
            m_plotWeightsView.Margin = new Padding(3, 2, 3, 2);
            m_plotWeightsView.Name = "m_plotWeightsView";
            m_plotWeightsView.PanCursor = Cursors.Hand;
            m_plotWeightsView.Size = new Size(288, 611);
            m_plotWeightsView.TabIndex = 1;
            m_plotWeightsView.ZoomHorizontalCursor = Cursors.SizeWE;
            m_plotWeightsView.ZoomRectangleCursor = Cursors.SizeNWSE;
            m_plotWeightsView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // m_amplComboBox
            // 
            m_amplComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_amplComboBox.FormattingEnabled = true;
            m_amplComboBox.Location = new Point(117, 2);
            m_amplComboBox.Margin = new Padding(3, 2, 3, 2);
            m_amplComboBox.Name = "m_amplComboBox";
            m_amplComboBox.Size = new Size(108, 23);
            m_amplComboBox.TabIndex = 2;
            m_amplComboBox.SelectedIndexChanged += OnAmplSelected;
            // 
            // m_speedComboBox
            // 
            m_speedComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_speedComboBox.FormattingEnabled = true;
            m_speedComboBox.Location = new Point(3, 2);
            m_speedComboBox.Margin = new Padding(3, 2, 3, 2);
            m_speedComboBox.Name = "m_speedComboBox";
            m_speedComboBox.Size = new Size(108, 23);
            m_speedComboBox.TabIndex = 1;
            m_speedComboBox.SelectedIndexChanged += OnSpeedSelected;
            // 
            // m_icaComposeButton
            // 
            m_icaComposeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_icaComposeButton.AutoSize = true;
            m_icaComposeButton.BackColor = Color.Transparent;
            m_icaComposeButton.Location = new Point(1004, 3);
            m_icaComposeButton.Margin = new Padding(5, 3, 5, 3);
            m_icaComposeButton.Name = "m_icaComposeButton";
            m_icaComposeButton.Size = new Size(68, 30);
            m_icaComposeButton.TabIndex = 3;
            m_icaComposeButton.Text = "Compose";
            m_icaComposeButton.UseVisualStyleBackColor = false;
            // 
            // m_bottomControlsLayoutPanel
            // 
            m_bottomControlsLayoutPanel.AutoSize = true;
            m_bottomControlsLayoutPanel.ColumnCount = 9;
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            m_bottomControlsLayoutPanel.Controls.Add(m_saveButton, 8, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_loadButton, 7, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_icaComposeButton, 6, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_speedComboBox, 0, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_amplComboBox, 1, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_filterLowCutOffComboBox, 2, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_filterHighCutOffComboBox, 3, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_icaButton, 5, 0);
            m_bottomControlsLayoutPanel.Controls.Add(m_autoButton, 4, 0);
            m_bottomControlsLayoutPanel.Dock = DockStyle.Bottom;
            m_bottomControlsLayoutPanel.Location = new Point(0, 611);
            m_bottomControlsLayoutPanel.Name = "m_bottomControlsLayoutPanel";
            m_bottomControlsLayoutPanel.RowCount = 1;
            m_bottomControlsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            m_bottomControlsLayoutPanel.Size = new Size(1261, 36);
            m_bottomControlsLayoutPanel.TabIndex = 1;
            // 
            // m_filterLowCutOffComboBox
            // 
            m_filterLowCutOffComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_filterLowCutOffComboBox.FormattingEnabled = true;
            m_filterLowCutOffComboBox.Location = new Point(231, 3);
            m_filterLowCutOffComboBox.Name = "m_filterLowCutOffComboBox";
            m_filterLowCutOffComboBox.Size = new Size(108, 23);
            m_filterLowCutOffComboBox.TabIndex = 7;
            m_filterLowCutOffComboBox.SelectedIndexChanged += OnFilterLowCutOffSelected;
            // 
            // m_filterHighCutOffComboBox
            // 
            m_filterHighCutOffComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_filterHighCutOffComboBox.FormattingEnabled = true;
            m_filterHighCutOffComboBox.Location = new Point(345, 3);
            m_filterHighCutOffComboBox.Name = "m_filterHighCutOffComboBox";
            m_filterHighCutOffComboBox.Size = new Size(108, 23);
            m_filterHighCutOffComboBox.TabIndex = 8;
            m_filterHighCutOffComboBox.SelectedIndexChanged += OnFilterHighCutOffSelected;
            // 
            // m_autoButton
            // 
            m_autoButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_autoButton.AutoSize = true;
            m_autoButton.BackColor = Color.Transparent;
            m_autoButton.Location = new Point(855, 3);
            m_autoButton.Margin = new Padding(5, 3, 5, 3);
            m_autoButton.Menu = m_autoContextMenuStrip;
            m_autoButton.Name = "m_autoButton";
            m_autoButton.Size = new Size(68, 30);
            m_autoButton.TabIndex = 3;
            m_autoButton.Text = "Auto";
            m_autoButton.UseVisualStyleBackColor = false;
            // 
            // m_autoContextMenuStrip
            // 
            m_autoContextMenuStrip.Items.AddRange(new ToolStripItem[] { autoCleanToolStripMenuItem, autoRangesToolStripMenuItem });
            m_autoContextMenuStrip.Name = "m_autoContextMenuStrip";
            m_autoContextMenuStrip.Size = new Size(142, 48);
            // 
            // autoCleanToolStripMenuItem
            // 
            autoCleanToolStripMenuItem.Name = "autoCleanToolStripMenuItem";
            autoCleanToolStripMenuItem.Size = new Size(141, 22);
            autoCleanToolStripMenuItem.Text = "Auto Clean";
            autoCleanToolStripMenuItem.Click += OnAutoClean;
            // 
            // autoRangesToolStripMenuItem
            // 
            autoRangesToolStripMenuItem.Name = "autoRangesToolStripMenuItem";
            autoRangesToolStripMenuItem.Size = new Size(141, 22);
            autoRangesToolStripMenuItem.Text = "Auto Ranges";
            autoRangesToolStripMenuItem.Click += OnAutoRanges;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1261, 647);
            Controls.Add(m_splitContainer);
            Controls.Add(m_bottomControlsLayoutPanel);
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(1024, 600);
            Name = "MainForm";
            Text = "EEG Cleaner";
            Load += OnLoad;
            m_icaContextMenuStrip.ResumeLayout(false);
            m_loadContextMenuStrip.ResumeLayout(false);
            m_splitContainer.Panel1.ResumeLayout(false);
            m_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)m_splitContainer).EndInit();
            m_splitContainer.ResumeLayout(false);
            m_plorViewControlsLayoutPanel.ResumeLayout(false);
            m_bottomControlsLayoutPanel.ResumeLayout(false);
            m_bottomControlsLayoutPanel.PerformLayout();
            m_autoContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView m_plotView;
        private MenuButton m_icaButton;
        private OpenFileDialog m_openFileDialog;
        private MenuButton m_loadButton;
        private ContextMenuStrip m_loadContextMenuStrip;
        private ToolStripMenuItem loadEEGToolStripMenuItem;
        private ToolStripMenuItem loadTestDataToolStripMenuItem;
        private SaveFileDialog m_saveFileDialog;
        private Button m_saveButton;
        private SplitContainer m_splitContainer;
        private OxyPlot.WindowsForms.PlotView m_plotWeightsView;
        private Button m_icaComposeButton;
        private ContextMenuStrip m_icaContextMenuStrip;
        private ToolStripMenuItem m_standradICAToolStripMenuItem;
        private ToolStripMenuItem m_normalizedICAToolStripMenuItem;
        private TableLayoutPanel m_plorViewControlsLayoutPanel;
        private HScrollBar m_plotViewHScrollBar;
        private ComboBox m_amplComboBox;
        private ComboBox m_speedComboBox;
        private TableLayoutPanel m_bottomControlsLayoutPanel;
        private ComboBox m_filterLowCutOffComboBox;
        private ComboBox m_filterHighCutOffComboBox;
        private MenuButton m_autoButton;
        private ContextMenuStrip m_autoContextMenuStrip;
        private ToolStripMenuItem autoCleanToolStripMenuItem;
        private ToolStripMenuItem autoRangesToolStripMenuItem;
        private ToolStripMenuItem resetDataToolStripMenuItem;
    }
}