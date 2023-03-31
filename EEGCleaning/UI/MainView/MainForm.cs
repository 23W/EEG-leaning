using EEGCleaning.Model;
using EEGCleaning.UI.MainView.StateMachine;
using EEGCleaning.Utilities;
using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.Analysis;
using EEGCore.Processing.ICA;
using EEGCore.Utilities;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;
using System.Text.Json;

namespace EEGCleaning
{
    public partial class MainForm : Form
    {
        #region Properties

        internal RecordViewModel ViewModel { get; set; } = new RecordViewModel();

        internal StateMachine StateMachine { get; init; }

        internal RecordPlotModel PlotModel => (RecordPlotModel)m_plotView.Model;
        internal TimeSpanAxis? PlotModelXAxis => (TimeSpanAxis?)PlotModel.Axes.FirstOrDefault(a => a.IsHorizontal() && a is TimeSpanAxis);
        internal IEnumerable<LinearAxis> PlotModelYAxes => PlotModel.Axes.Where(a => a.IsVertical() && a is LinearAxis && a.Tag is Lead).Cast<LinearAxis>();

        internal Point LastPoint { get; set; } = Point.Empty;

        internal Button ICAControl => m_icaButton;
        internal ToolStripItem StandardICAControl => m_standradICAToolStripMenuItem;
        internal ToolStripItem NormalizedICAControl => m_normalizedICAToolStripMenuItem;
        internal Button ICAComposeControl => m_icaComposeButton;

        #endregion

        #region Internal Properties

        bool NeedPlotRescale { get; set; } = false;
        bool InPlotRescaleExecution { get; set; } = false;

        SpeedItem[] SpeedItems => new[]
        {
            new SpeedItem() { Value = -1 },
            new SpeedItem() { Value = 7.5 },
            new SpeedItem() { Value = 15 },
            new SpeedItem() { Value = 30 },
            new SpeedItem() { Value = 60 },
            new SpeedItem() { Value = 120 },
        };

        AmplItem[] AmplItems => new[]
        {
            new AmplItem() { Value = -1 },
            new AmplItem() { Value = 10 },
            new AmplItem() { Value = 25 },
            new AmplItem() { Value = 50 },
            new AmplItem() { Value = 100 },
            new AmplItem() { Value = 250 },
            new AmplItem() { Value = 500 },
            new AmplItem() { Value = 1000 },
            new AmplItem() { Value = 1500 },
            new AmplItem() { Value = 2000 },
        };

        #endregion


        #region Nested classes

        internal class RecordPlotModel : PlotModel
        {
            internal event EventHandler? BeforeRendering;
            internal event EventHandler? AfterRendering;

            IRenderContext? CurrentRenderContext { get; set; } = default;
            IDisposable? ClipToken { get; set; } = default;

            internal void LockRenderingContext()
            {
                ClipToken = CurrentRenderContext?.AutoResetClip(new OxyRect());
            }

            internal void UnlockRenderingContext()
            {
                ClipToken?.Dispose();
                ClipToken = default;
                CurrentRenderContext = default;
            }

            protected override void RenderOverride(IRenderContext rc, OxyRect rect)
            {
                CurrentRenderContext = rc;
                BeforeRendering?.Invoke(this, EventArgs.Empty);

                base.RenderOverride(rc, rect);

                AfterRendering?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();

            m_speedComboBox.Items.AddRange(SpeedItems);
            m_amplComboBox.Items.AddRange(AmplItems);

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

        internal void RunICADecompose(RecordRange? range = default, bool normalizePower = false, bool analyzeComponents = true)
        {
            var ica = new FastICA()
            {
                MaxIterationCount = 10000,
                Tolerance = 1E-06,
                NormalizePower = normalizePower,
            };

            ViewModel.IndependentComponents = ica.Decompose(ViewModel.ProcessedRecord, range);

            if (analyzeComponents)
            {
                foreach (var analyzer in BuildICAAnalyzers(ViewModel.IndependentComponents))
                {
                    analyzer.Analyze();
                }
            }

#if DEBUG
            var currentPath = Directory.GetCurrentDirectory();
            File.WriteAllText(Path.Combine(currentPath, "ICA.json"), ViewModel.IndependentComponents.ToJson());

            foreach(var (lead, componentIndex) in ViewModel.IndependentComponents.Leads.Cast<ComponentLead>().WithIndex())
            {
                var componentWeights = ViewModel.IndependentComponents.GetMixingVector(componentIndex);
                File.WriteAllText(Path.Combine(currentPath, $"ICA-{lead.Name}.json"), JsonSerializer.Serialize(componentWeights));
            }
#endif

        }

        internal void RunICACompose()
        {
            var ica = new FastICA();

            ViewModel.ProcessedRecord = ica.Compose(ViewModel.IndependentComponents, SuppressComponents.MatrixAndComponents);
        }

        internal void UpdatePlot(ModelViewMode viewMode)
        {
            NeedPlotRescale = true;

            var oldViewMode = ViewModel.ViewMode;
            ViewModel.ViewMode = viewMode;

            if (oldViewMode != viewMode)
            {
                ViewModel.Position = TimePositionItem.Default;
                ViewModel.Amplitude = AmplItem.Default;
            }

            UnsubsribePlotEvents(m_plotView.Model);

            var plotModel = new RecordPlotModel();
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
            //m_plotView.ActualController.BindMouseEnter(PlotCommands.HoverSnapTrack);
            m_plotView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_plotWeightsView.Model = plotWeightsModel;
            m_plotWeightsView.ActualController.UnbindAll();
            //m_plotWeightsView.ActualController.BindMouseEnter(PlotCommands.HoverSnapTrack);
            m_plotWeightsView.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            m_splitContainer.Panel2Collapsed = (ViewModel.ViewMode != ModelViewMode.ICA);

            switch (ViewModel.ViewMode)
            {
                case ModelViewMode.Record:
                    m_icaButton.Menu = m_icaContextMenuStrip;
                    m_icaButton.BackColor = SystemColors.Control;
                    m_icaComposeButton.Visible = false;
                    break;
                case ModelViewMode.ICA:
                    m_icaButton.Menu = default;
                    m_icaButton.BackColor = SystemColors.ControlDark;
                    m_icaComposeButton.Visible = true;
                    break;
            }

            m_plotView.Model.ResetAllAxes();

            UpdateSpeedBar();
            UpdateAmplBar();
            UpdateHScrollBar();
        }

        static IEnumerable<AnalyzerBase<ComponentArtifactResult>> BuildICAAnalyzers(ICARecord input)
        {
            var res = new List<AnalyzerBase<ComponentArtifactResult>>()
            {
                new ElectrodeArtifactDetector() { Input = input },
                new EyeArtifactDetector() { Input = input },
            };
            return res;
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
                Minimum = 0,
                Maximum = record.Duration / record.SampleRate,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = record.Duration / record.SampleRate,
                MaximumPadding = 0,
                MinimumPadding = 0,
            };
            SubsribePlotEvents(xAxis);
            plotModel.Axes.Add(xAxis);

            var maxSignalAmpl = record.GetMaximumAbsoluteValue();
            var signalRange = Tuple.Create(-maxSignalAmpl, maxSignalAmpl);

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
                    Minimum = signalRange.Item1,
                    Maximum = signalRange.Item2,
                    AbsoluteMinimum = signalRange.Item1,
                    AbsoluteMaximum = signalRange.Item2,
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
                var points = lead.Samples.Select((s, index) => new DataPoint(index / record.SampleRate, s));
                leadSeries.Points.AddRange(points);

                var leadAnnotation = default(Annotation);
                var leadAlternativeSeries = default(LineSeries);

                if (lead is ComponentLead componentLead)
                {
                    if (componentLead.Alternative.Any())
                    {
                        leadAlternativeSeries = new LineSeries()
                        {
                            Color = ViewUtilities.GetAlternativeLeadColor(lead),
                            LineStyle = LineStyle.Solid,
                            YAxisKey = leadAxis.Key,
                            UsePlotModelClipArrea = true,
                            Tag = lead,
                        };
                        var alternativePoints = componentLead.Alternative.Select((s, index) => new DataPoint(index / record.SampleRate, s));
                        leadAlternativeSeries.Points.AddRange(alternativePoints);
                    }

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

                plotModel.Axes.Add(leadAxis);
                plotModel.Series.Add(leadSeries);

                if (leadAlternativeSeries != default)
                {
                    plotModel.Series.Add(leadAlternativeSeries);
                }

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

            if (plotModel is RecordPlotModel recordPlotModel)
            {
                recordPlotModel.BeforeRendering += OnBeforeRecordPlotModelRendering;
                recordPlotModel.AfterRendering += OnAfterRecordPlotModelRendering;
            }
        }

        void SubsribePlotEvents(PlotElement plotElement)
        {
            if (plotElement == default)
            {
                return;
            }
#pragma warning disable CS0618 // Type or member is obsolete
            if (plotElement is TimeSpanAxis xAxis)
            {
                xAxis.AxisChanged += OnXAxisChanged;
            }
            else
            {
                plotElement.MouseDown += OnPlotMouseDown;
                plotElement.MouseUp += OnPlotMouseUp;
                plotElement.MouseMove += OnPlotMouseMove;
            }
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

            if (plotModel is RecordPlotModel recordPlotModel)
            {
                recordPlotModel.BeforeRendering -= OnBeforeRecordPlotModelRendering;
                recordPlotModel.AfterRendering -= OnAfterRecordPlotModelRendering;
            }
        }

        void UnsubsribePlotEvents(PlotElement plotElement)
        {
            if (plotElement == default)
            {
                return;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            if (plotElement is TimeSpanAxis xAxis)
            {
                xAxis.AxisChanged -= OnXAxisChanged;
            }
            else
            {
                plotElement.MouseDown -= OnPlotMouseDown;
                plotElement.MouseUp -= OnPlotMouseUp;
                plotElement.MouseMove -= OnPlotMouseMove;
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        void UpdateHScrollBar()
        {
            var xAxis = PlotModelXAxis;
            if (xAxis != default)
            {
                var wholeRange = ViewModel.VisibleRecord.Duration;
                var viewportRange = (int)((xAxis.ActualMaximum - xAxis.ActualMinimum) * ViewModel.VisibleRecord.SampleRate);
                var position = (int)(xAxis.ActualMinimum * ViewModel.VisibleRecord.SampleRate);
                var visible = wholeRange != viewportRange;

                m_plotViewHScrollBar.Minimum = 0;
                m_plotViewHScrollBar.SmallChange = viewportRange / 50;
                m_plotViewHScrollBar.LargeChange = viewportRange / 2;
                m_plotViewHScrollBar.Maximum = (wholeRange - viewportRange) + m_plotViewHScrollBar.LargeChange;
                m_plotViewHScrollBar.Value = position;
                m_plotViewHScrollBar.Visible = visible;
            }
        }

        void UpdateSpeedBar()
        {
            var item = m_speedComboBox.Items
                                      .Cast<SpeedItem>()
                                      .FirstOrDefault(s => s.Value == ViewModel.Speed.Value);
            m_speedComboBox.SelectedItem = item;
        }

        void UpdateAmplBar()
        {
            var item = m_amplComboBox.Items
                                     .Cast<AmplItem>()
                                     .FirstOrDefault(a => a.Value == ViewModel.Amplitude.Value);
            m_amplComboBox.SelectedItem = item;
        }

        void ScrollPlot(int samplePosition)
        {
            Debug.Assert(samplePosition >= 0);

            ScrollPlot(new TimePositionItem() { Value = samplePosition / ViewModel.VisibleRecord.SampleRate });
        }

        void ScrollPlot(TimePositionItem position)
        {
            Debug.Assert(position.Value >= 0);

            var xAxis = PlotModelXAxis;
            if ((xAxis != default) &&
                !InPlotRescaleExecution)
            {
                InPlotRescaleExecution = true;

                var viewportRange = xAxis.ActualMaximum - xAxis.ActualMinimum;
                var newPosition = position.Value;

                ViewModel.Position = position;

                xAxis.Minimum = newPosition;
                xAxis.Maximum = newPosition + viewportRange;
                xAxis.Reset();
                PlotModel.InvalidatePlot(false);

                InPlotRescaleExecution = false;
            }
        }

        void SpeedPlot(SpeedItem speed)
        {
            var xAxis = PlotModelXAxis;

            if ((xAxis != default) &&
                !InPlotRescaleExecution)
            {
                InPlotRescaleExecution = true;

                ViewModel.Speed = speed;

                if (speed.Value > 0)
                {
                    var ptPerMm = ViewUtilities.GetDPMM(this).X;
                    var viewportRange_mm = PlotModel.PlotArea.Width / ptPerMm;
                    var viewportRange_sec = viewportRange_mm / speed.Value;

                    xAxis.Minimum = ViewModel.Position.Value;
                    xAxis.Maximum = ViewModel.Position.Value + viewportRange_sec;
                }
                else
                {
                    ViewModel.Position = TimePositionItem.Default;

                    xAxis.Minimum = xAxis.AbsoluteMinimum;
                    xAxis.Maximum = xAxis.AbsoluteMaximum;
                }

                xAxis.Reset();
                PlotModel.InvalidatePlot(false);

                UpdateHScrollBar();

                InPlotRescaleExecution = false;
            }
        }

        void AmplifirePlot(AmplItem amplitude)
        {
            var xAxis = PlotModelXAxis;

            if ((xAxis != default) &&
                !InPlotRescaleExecution)
            {
                InPlotRescaleExecution = true;

                ViewModel.Amplitude = amplitude;

                var maxSignalAmpl = new Lazy<double>(ViewModel.VisibleRecord.GetMaximumAbsoluteValue);

                foreach (var yAxis in PlotModelYAxes)
                {
                    if (amplitude.Value > 0)
                    {
                        var ptPerMm = ViewUtilities.GetDPMM(this).Y;

                        var viewportRange_pt = Math.Abs(yAxis.Transform(yAxis.ActualMaximum) - yAxis.Transform(0));
                        var viewportRange_mm = viewportRange_pt / ptPerMm;
                        var viewportRange_mkV = viewportRange_mm * (amplitude.Value / 10);

                        yAxis.Minimum = -viewportRange_mkV;
                        yAxis.Maximum = viewportRange_mkV;
                        yAxis.AbsoluteMinimum = -viewportRange_mkV;
                        yAxis.AbsoluteMaximum = viewportRange_mkV;
                    }
                    else
                    {
                        yAxis.Minimum = -maxSignalAmpl.Value;
                        yAxis.Maximum = maxSignalAmpl.Value;
                        yAxis.AbsoluteMinimum = -maxSignalAmpl.Value;
                        yAxis.AbsoluteMaximum = maxSignalAmpl.Value;
                    }

                    yAxis.Reset();
                }

                PlotModel.InvalidatePlot(false);
                UpdateHScrollBar();

                InPlotRescaleExecution = false;
            }
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

        #region Plot Model Events

        void OnBeforeRecordPlotModelRendering(object? sender, EventArgs e)
        {
            if (NeedPlotRescale)
            {
                PlotModel.LockRenderingContext();
            }
        }

        void OnAfterRecordPlotModelRendering(object? sender, EventArgs e)
        {
            if (NeedPlotRescale)
            {
                NeedPlotRescale = false;

                PlotModel.UnlockRenderingContext();

                ScrollPlot(ViewModel.Position);
                SpeedPlot(ViewModel.Speed);
                AmplifirePlot(ViewModel.Amplitude);
            }
        }

        void OnXAxisChanged(object? sender, AxisChangedEventArgs e)
        {
            if (!NeedPlotRescale && sender is TimeSpanAxis xAxis)
            {
                ViewModel.Position = new TimePositionItem() { Value = xAxis.ActualMinimum };

                UpdateHScrollBar();
            }
        }

        void OnPlotViewResized(object sender, EventArgs e)
        {
            NeedPlotRescale = true;
        }

        #endregion

        #region Controls Events

        void OnLoad(object sender, EventArgs e)
        {
            LoadRecord(@".\EEGData\Test1\EEG Eye State.arff", RecordFactoryOptions.DefaultEEGNoFilter);

            UpdatePlot(ModelViewMode.Record);
            StateMachine.SwitchState(EEGRecordState.Name);
        }

        void OnSpeedSelected(object sender, EventArgs e)
        {
            if (m_speedComboBox.SelectedItem is SpeedItem selectedItem)
            {
                SpeedPlot(selectedItem);
            }
        }

        void OnAmplSelected(object sender, EventArgs e)
        {
            if (m_amplComboBox.SelectedItem is AmplItem selectedItem)
            {
                AmplifirePlot(selectedItem);
            }
        }

        void OnHScroll(object sender, ScrollEventArgs e)
        {
            ScrollPlot(m_plotViewHScrollBar.Value);
        }

        void OnLoadTestData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEmpty);

                UpdatePlot(ModelViewMode.Record);
                StateMachine.SwitchState(EEGRecordState.Name);
            }
        }

        void OnLoadEEGData(object sender, EventArgs e)
        {
            m_openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (m_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadRecord(m_openFileDialog.FileName, RecordFactoryOptions.DefaultEEGNoFilter);

                UpdatePlot(ModelViewMode.Record);
                StateMachine.SwitchState(EEGRecordState.Name);
            }
        }

        void OnSaveData(object sender, EventArgs e)
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
