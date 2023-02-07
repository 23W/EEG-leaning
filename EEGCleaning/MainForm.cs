using CsvHelper;
using EEGCleaning.Model;
using EEGCleaning.Utilities;
using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.ICA;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EEGCleaning
{
    public partial class MainForm : Form
    {
        RecordViewModel ViewModel { get; set; } = new RecordViewModel();

        public MainForm()
        {
            InitializeComponent();
        }

        void LoadRecord(string path, RecordFactoryOptions options)
        {
            var factory = new RecordFactory();

            ViewModel.SourceRecord = factory.FromFile(path, options);
            ViewModel.RecordOptions = options;
            ViewModel.CurrentRecord = ViewModel.SourceRecord;
        }

        void SaveRecord(string path)
        {
            var factory = new RecordFactory();

            switch (ViewModel.ViewMode)
            {
                case ModelViewMode.Record:
                    factory.ToFile(path, ViewModel.CurrentRecord);
                    break;
                case ModelViewMode.ICA:
                    factory.ToFile(path, ViewModel.IndependentComponents);
                    break;
            }
        }

        void RunICA()
        {
            var ica = new FastICA()
            {
                Estimation = FastICA.NonGaussianityEstimation.LogCosh,
                MaxIterationCount = 10000,
                Tolerance = 1E-06,
            };

            ViewModel.IndependentComponents = ica.Solve(ViewModel.CurrentRecord);
        }

        void UpdatePlot(ModelViewMode viewMode)
        {
            ViewModel.ViewMode = viewMode;
            ViewModel.ScaleX = 1;
            ViewModel.ScaleY = 1;

            var plotModel = new PlotModel();

            switch (ViewModel.ViewMode)
            {
                case ModelViewMode.Record:
                    PopulatedPlotModel(plotModel, ViewModel.CurrentRecord);
                    break;

                case ModelViewMode.ICA:
                    PopulatedPlotModel(plotModel, ViewModel.IndependentComponents);
                    break;
            }

            m_plotView.Model = plotModel;
            m_plotView.ActualController.UnbindAll();
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_xTrackBar.Value = (int)ViewModel.ScaleX - 1;
            m_yTrackBar.Value = (int)(ViewModel.ScaleY * 10) - 10;

            m_icaButton.BackColor = (ViewModel.ViewMode== ModelViewMode.ICA) ? SystemColors.ControlDark : SystemColors.Control;

            UpdateZoom();
        }

        void PopulatedPlotModel(PlotModel plotModel, Record record)
        {
            var xAxis = new TimeSpanAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                FontSize = 9,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = record.Duration / record.SampleRate,
            };
            plotModel.Axes.Add(xAxis);

            var maxSignalAmpl = record.GetMaximumAbsoluteValue();
            var range = Tuple.Create(-maxSignalAmpl, maxSignalAmpl);

            for (var leadIndex = 0; leadIndex < record.Leads.Count; leadIndex++)
            {
                var lead = record.Leads[leadIndex];

                var leadAxisIndex = record.Leads.Count - leadIndex - 1;
                var leadAxis = new LinearAxis()
                {
                    Title = lead.Name,
                    Key = lead.Name,
                    StartPosition = (double)(leadAxisIndex) / record.Leads.Count,
                    EndPosition = (double)(leadAxisIndex + 1) / record.Leads.Count,
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    Minimum = range.Item1,
                    Maximum = range.Item2,
                    AbsoluteMinimum = range.Item1,
                    AbsoluteMaximum = range.Item2,
                    IsPanEnabled = false,
                };

                var leadSeries = new LineSeries()
                {
                    Color = ViewUtilities.GetLeadColor(lead),
                    LineStyle = LineStyle.Solid,
                    YAxisKey = leadAxis.Key,
                    UsePlotModelClipArrea = true,
                };

                var points = lead.Samples.Select((s, index) => new DataPoint(index / record.SampleRate, s));
                leadSeries.Points.AddRange(points);

                plotModel.Axes.Add(leadAxis);
                plotModel.Series.Add(leadSeries);
            }
        }

        void UpdateZoom()
        {
            foreach (var a in m_plotView.Model.Axes)
            {
                switch (a.Position)
                {
                    case AxisPosition.Left:
                    case AxisPosition.Right:
                        a.Reset();
                        a.ZoomAtCenter(ViewModel.ScaleY);
                        break;

                    case AxisPosition.Top:
                    case AxisPosition.Bottom:
                        a.Reset();
                        a.ZoomAtCenter(ViewModel.ScaleX);
                        break;
                }
            }

            m_plotView.Model.InvalidatePlot(false);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            LoadRecord(@".\EEGData\Test1\EEG Eye State.arff", RecordFactoryOptions.DefaultEEG);

            UpdatePlot(ModelViewMode.Record);
        }

        private void OnXScale(object sender, EventArgs e)
        {
            ViewModel.ScaleX = m_xTrackBar.Value + 1;
            UpdateZoom();
        }

        private void OnYScale(object sender, EventArgs e)
        {
            ViewModel.ScaleY = (m_yTrackBar.Value + 10) / 10.0;
            UpdateZoom();
        }

        private void OnRunICA(object sender, EventArgs e)
        {
            var nextMode = ModelViewMode.Record;

            if (ViewModel.ViewMode == ModelViewMode.Record)
            {
                RunICA();

                nextMode = ModelViewMode.ICA;
            }

            UpdatePlot(nextMode);
        }

        private void OnLoadTestData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEmpty);
                UpdatePlot(ModelViewMode.Record);
            }
        }

        private void OnLoadEEGData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEEG);
                UpdatePlot(ModelViewMode.Record);
            }
        }

        private void OnSaveData(object sender, EventArgs e)
        {
            m_saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveRecord(m_saveFileDialog.FileName);
            }
        }
    }
}
