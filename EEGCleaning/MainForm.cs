using EEGCore.Data;
using EEGCore.Processing;
using EEGCleaning.Model;
using EEGCleaning.Utilities;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace EEGCleaning
{
    public partial class MainForm : Form
    {
        RecordViewModel ViewModel { get; set; } = new RecordViewModel();

        public MainForm()
        {
            InitializeComponent();
        }

        void LoadRecord()
        {
            var factory = new RecordFactory();
            var factoryOptions = ViewModel.RecordOptions;

            ViewModel.Record = factory.FromFile(@"C:\Users\kysel\Documents\ХИРЭ\Я\Диплом\EEGSets\1\EEG Eye State.arff", factoryOptions);
        }

        void UpdatePlot()
        {
            var plotModel = new PlotModel();

            var xAxis = new TimeSpanAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                FontSize = 9,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = ViewModel.Record.Duration / ViewModel.Record.SampleRate,
            };
            plotModel.Axes.Add(xAxis);

            var maxSignalAmpl = ViewModel.Record.GetMaximumAbsoluteValue();
            var range = Tuple.Create(-maxSignalAmpl, maxSignalAmpl);

            for (var leadIndex = 0; leadIndex<ViewModel.Record.Leads.Count; leadIndex++) 
            {
                var lead = ViewModel.Record.Leads[leadIndex];

                var leadAxis = new LinearAxis()
                {
                    Title = lead.Name,
                    Key = lead.Name,
                    StartPosition = (double)leadIndex / ViewModel.Record.Leads.Count,
                    EndPosition = (double)(leadIndex + 1) / ViewModel.Record.Leads.Count,
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

                var points = lead.Samples.Select((s, index) => new DataPoint(index / ViewModel.Record.SampleRate, s));
                leadSeries.Points.AddRange(points);

                plotModel.Axes.Add(leadAxis);
                plotModel.Series.Add(leadSeries);
            }

            m_plotView.Model = plotModel;
            m_plotView.ActualController.UnbindAll();
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_xTrackBar.Value = (int)ViewModel.ScaleX - 1;
            m_yTrackBar.Value = (int)(ViewModel.ScaleY * 10) - 10;

            UpdateZoom();
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
            LoadRecord();

            UpdatePlot();
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
    }
}