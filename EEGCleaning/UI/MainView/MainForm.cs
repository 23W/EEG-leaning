using EEGCleaning.Model;
using EEGCleaning.UI.MainView.StateMachine;
using EEGCleaning.Utilities;
using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.ICA;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EEGCleaning
{
    public partial class MainForm : Form
    {
        internal RecordViewModel ViewModel { get; set; } = new RecordViewModel();

        internal StateMachine StateMachine { get; init; }

        internal PlotModel PlotModel => m_plotView.Model;

        internal Point LastPoint { get; set; } = Point.Empty;

        internal Button ICAButton => m_icaButton;

        internal Button ICAComposeButton => m_icaComposeButton;

        public MainForm()
        {
            InitializeComponent();

            StateMachine = new StateMachine(this);
        }

        internal void UpdatePlot()
        {
            UpdatePlot(ViewModel.ViewMode);
        }

        internal object GetViewModelObject(object? sender)
        {
            var res = PlotModel as object;

            if (sender is Record record)
            {
                res = record;
            }
            else if (sender is Lead lead)
            {
                res = lead;
            }
            else if (sender is RecordRange range)
            {
                res = range;
            }
            else if ((sender is PlotModel) ||
                     (sender is PlotController))
            {
                res = ViewModel.ProcessedRecord;
            }
            else if (sender is PlotElement element)
            {
                res = element.Tag;
            }

            return res;
        }

        internal bool IsPanDistance(double time1, double time2)
        {
            bool res = Math.Abs(time2 - time1) * ViewModel.ProcessedRecord.SampleRate > 10;
            return res;
        }

        internal void RunICADecompose(RecordRange? range = default)
        {
            var ica = new FastICA()
            {
                MaxIterationCount = 10000,
                Tolerance = 1E-06,
            };

            ViewModel.IndependentComponents = ica.Decompose(ViewModel.ProcessedRecord, range);
        }

        internal void RunICACompose()
        {
            var ica = new FastICA();

            ViewModel.ProcessedRecord = ica.Compose(ViewModel.IndependentComponents, true);
        }

        internal void UpdatePlot(ModelViewMode viewMode)
        {
            ViewModel.ViewMode = viewMode;
            ViewModel.ScaleX = 1;
            ViewModel.ScaleY = 1;

            UnsubsribePlotEvents(m_plotView.Model);

            var plotModel = /*m_plotView.Model ?? */new PlotModel();
            var plotWeightsModel = new PlotModel();
            switch (ViewModel.ViewMode)
            {
                case ModelViewMode.Record:
                    PopulatedPlotModel(plotModel, ViewModel.ProcessedRecord);
                    break;

                case ModelViewMode.ICA:
                    PopulatedPlotModel(plotModel, ViewModel.IndependentComponents);
                    PopulatedPlotWeightsModel(plotWeightsModel, ViewModel.IndependentComponents);
                    break;
            }

            SubsribePlotEvents(plotModel);

            m_plotView.Model = plotModel;
            m_plotView.ActualController.UnbindAll();
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_plotWeightsView.Model = plotWeightsModel;
            m_plotWeightsView.ActualController.UnbindAll();
            m_plotWeightsView.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
            m_plotWeightsView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_splitContainer.Panel2Collapsed = (ViewModel.ViewMode != ModelViewMode.ICA);

            m_xTrackBar.Value = (int)ViewModel.ScaleX - 1;
            m_yTrackBar.Value = (int)(ViewModel.ScaleY * 10) - 10;

            m_icaButton.BackColor = (ViewModel.ViewMode == ModelViewMode.ICA) ? SystemColors.ControlDark : SystemColors.Control;
            m_icaComposeButton.Visible = ViewModel.ViewMode == ModelViewMode.ICA;

            UpdateZoom();
        }

        void LoadRecord(string path, RecordFactoryOptions options)
        {
            var factory = new RecordFactory();

            ViewModel.SourceRecord = factory.FromFile(path, options);
            ViewModel.RecordOptions = options;
            ViewModel.ProcessedRecord = ViewModel.SourceRecord;
        }

        void SaveRecord(string path)
        {
            var factory = new RecordFactory();

            switch (ViewModel.ViewMode)
            {
                case ModelViewMode.Record:
                    factory.ToFile(path, ViewModel.ProcessedRecord);
                    break;
                case ModelViewMode.ICA:
                    factory.ToFile(path, ViewModel.IndependentComponents);
                    break;
            }
        }

        void PopulatedPlotModel(PlotModel plotModel, Record record)
        {
            plotModel.Axes.ToList().ForEach(a => UnsubsribePlotEvents(a));
            plotModel.Series.ToList().ForEach(a => UnsubsribePlotEvents(a));
            plotModel.Annotations.ToList().ForEach(a => UnsubsribePlotEvents(a));

            plotModel.Axes.Clear();
            plotModel.Series.Clear();
            plotModel.Annotations.Clear();

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
                    Tag = lead,
                };

                var leadSeries = new LineSeries()
                {
                    Color = ViewUtilities.GetLeadColor(lead),
                    LineStyle = LineStyle.Solid,
                    YAxisKey = leadAxis.Key,
                    UsePlotModelClipArrea = true,
                    Tag = lead,
                };

                var leadAnnotation = default(Annotation);
                if (lead is ComponentLead componentLead)
                {
                    leadAnnotation = new PointAnnotation()
                    {
                        Shape = componentLead.IsArtifact ? MarkerType.Square : MarkerType.Diamond,
                        Fill = componentLead.IsArtifact ? OxyColors.DarkRed : OxyColors.DarkGreen,
                        Size = 8,
                        X = 0,
                        Y = 0,
                        XAxisKey = xAxis.Key,
                        YAxisKey = leadAxis.Key,
                        ClipByXAxis = false,
                        ClipByYAxis = false,
                        Tag = lead,
                    };

                    SubsribePlotEvents(leadAnnotation);
                }

                var points = lead.Samples.Select((s, index) => new DataPoint(index / record.SampleRate, s));
                leadSeries.Points.AddRange(points);

                plotModel.Axes.Add(leadAxis);
                plotModel.Series.Add(leadSeries);

                if (leadAnnotation != default)
                {
                    plotModel.Annotations.Add(leadAnnotation);
                }
            }

            if (record.Ranges.Any())
            {
                // hidden annotation axis
                var annotationAxis = new LinearAxis()
                {
                    Key = nameof(record.Ranges),
                    StartPosition = 0,
                    EndPosition = 0 + 0.001,
                    Position = AxisPosition.Left,
                    IsPanEnabled = false,
                    IsAxisVisible = false,
                };
                plotModel.Axes.Add(annotationAxis);

                foreach (var recordRange in record.Ranges)
                {
                    var from = recordRange.From / record.SampleRate;
                    var to = (recordRange.From + recordRange.Duration) / record.SampleRate;

                    var rangeAnnotation = new RectangleAnnotation()
                    {
                        Fill = OxyColor.FromAColor(30, OxyColors.SeaGreen),
                        MinimumX = from,
                        MaximumX = to,
                        ClipByYAxis = false,
                        Tag = recordRange,
                    };

                    var rangePointAnnotation = new PointAnnotation()
                    {
                        ToolTip = string.Join("\n", recordRange.Name,
                                                    $"From: {xAxis.FormatValue(from)}",
                                                    $"To: {xAxis.FormatValue(from)}"),
                        Shape = MarkerType.Diamond,
                        Fill = OxyColors.OrangeRed,
                        Size = 8,
                        X = (from + to) / 2,
                        YAxisKey = nameof(record.Ranges),
                        ClipByYAxis = false,
                        Tag = recordRange,
                    };

                    SubsribePlotEvents(rangePointAnnotation);

                    plotModel.Annotations.Add(rangeAnnotation);
                    plotModel.Annotations.Add(rangePointAnnotation);
                }
            }
        }

        void PopulatedPlotWeightsModel(PlotModel plotModel, ICARecord record)
        {
            plotModel.Axes.Clear();
            plotModel.Series.Clear();
            plotModel.Annotations.Clear();

            var xAxis = new CategoryAxis()
            {
                AxislineStyle = LineStyle.Solid,
                GapWidth = 0.1,
                Position = AxisPosition.Bottom,
                Key = nameof(record.X)
            };

            if (record.X != default)
            {
                xAxis.Labels.AddRange(record.X.Leads.Select(l => l.Name));
            }
            else
            {
                xAxis.Labels.AddRange(Enumerable.Range(1, record.LeadsCount).Select(n => $"X{n}"));
            }

            plotModel.Axes.Add(xAxis);

            for (var componentIndex = 0; componentIndex < record.Leads.Count; componentIndex++)
            {
                var component = record.Leads[componentIndex];

                var componentAxisIndex = record.Leads.Count - componentIndex - 1;
                var componentAxis = new LinearAxis()
                {
                    Title = component.Name,
                    Key = component.Name,
                    StartPosition = (double)(componentAxisIndex) / record.Leads.Count,
                    EndPosition = (double)(componentAxisIndex + 1) / record.Leads.Count,
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    IsPanEnabled = false,
                    Tag = component,
                };
                plotModel.Axes.Add(componentAxis);

                var componentSeries = new BarSeries()
                {
                    XAxisKey = componentAxis.Key,
                    YAxisKey = nameof(record.X),
                    Tag = component,
                };
                componentSeries.Items.AddRange(record.GetMixingVector(componentIndex)
                                                     .Select((w, i) => new BarItem()
                                                     {
                                                         Value = w,
                                                         Color = record.X == default ? ViewUtilities.DefaultLeadColor :
                                                                                       ViewUtilities.GetLeadColor(record.X.Leads[i])
                                                     }));

                plotModel.Series.Add(componentSeries);
            }
        }

        void SubsribePlotEvents(PlotModel plotModel)
        {
            if (plotModel == default)
            {
                return;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            plotModel.MouseDown += OnPlotMouseDown;
            plotModel.MouseUp += OnPlotMouseUp;
            plotModel.MouseMove += OnPlotMouseMove;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        void SubsribePlotEvents(PlotElement plotElement)
        {
            if (plotElement == default)
            {
                return;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            plotElement.MouseDown += OnPlotMouseDown;
            plotElement.MouseUp += OnPlotMouseUp;
            plotElement.MouseMove += OnPlotMouseMove;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        void UnsubsribePlotEvents(PlotModel plotModel)
        {
            if (plotModel == default)
            {
                return;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            plotModel.MouseDown -= OnPlotMouseDown;
            plotModel.MouseUp -= OnPlotMouseUp;
            plotModel.MouseMove -= OnPlotMouseMove;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        void UnsubsribePlotEvents(PlotElement plotElement)
        {
            if (plotElement == default)
            {
                return;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            plotElement.MouseDown -= OnPlotMouseDown;
            plotElement.MouseUp -= OnPlotMouseUp;
            plotElement.MouseMove -= OnPlotMouseMove;
#pragma warning restore CS0618 // Type or member is obsolete
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

        #region Input Events

        void OnPlotMouseDown(object? sender, OxyMouseDownEventArgs e)
        {
            void wasHandled(string _, bool handled) { e.Handled = handled; }

            LastPoint = new Point((int)e.Position.X, (int)e.Position.Y);

            var mouseEvent = StateMachine.EventMouseDown;
            mouseEvent.AfterEvent += wasHandled;
            mouseEvent.Init(this, sender, e);
            mouseEvent.Fire();
            mouseEvent.AfterEvent -= wasHandled;
        }

        void OnPlotMouseUp(object? sender, OxyMouseEventArgs e)
        {
            void wasHandled(string _, bool handled) { e.Handled = handled; }

            LastPoint = new Point((int)e.Position.X, (int)e.Position.Y);

            var mouseEvent = StateMachine.EventMouseUp;
            mouseEvent.AfterEvent += wasHandled;
            mouseEvent.Init(this, sender, e);
            mouseEvent.Fire();
            mouseEvent.AfterEvent -= wasHandled;
        }

        void OnPlotMouseMove(object? sender, OxyMouseEventArgs e)
        {
            void wasHandled(string _, bool handled) { e.Handled = handled; }

            LastPoint = new Point((int)e.Position.X, (int)e.Position.Y);

            var mouseEvent = StateMachine.EventMouseMove;
            mouseEvent.AfterEvent += wasHandled;
            mouseEvent.Init(this, sender, e);
            mouseEvent.Fire();
            mouseEvent.AfterEvent -= wasHandled;
        }

        #endregion

        #region Controls Events

        private void OnLoad(object sender, EventArgs e)
        {
            LoadRecord(@".\EEGData\Test1\EEG Eye State.arff", RecordFactoryOptions.DefaultEEG);

            UpdatePlot(ModelViewMode.Record);
            StateMachine.SwitchState(EEGRecordState.Name);
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

        private void OnLoadTestData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEmpty);

                UpdatePlot(ModelViewMode.Record);
                StateMachine.SwitchState(EEGRecordState.Name);
            }
        }

        private void OnLoadEEGData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEEG);

                UpdatePlot(ModelViewMode.Record);
                StateMachine.SwitchState(EEGRecordState.Name);
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

        #endregion
    }
}
