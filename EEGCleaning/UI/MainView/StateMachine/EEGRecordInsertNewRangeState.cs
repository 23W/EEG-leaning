using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class EEGRecordInsertNewRangeState : StateBase
    {
        #region Construction

        internal EEGRecordInsertNewRangeState(StateMachine stateMachine)
            : base(stateMachine)
        {
        }

        #endregion

        #region Properties

        internal static string Name => nameof(EEGRecordInsertNewRangeState);

        internal double InitialTime { get; set; } = 0;
        internal double CurrentTime { get; set; } = 0;
        internal RectangleAnnotation? Range { get; set; } = default;

        #endregion

        #region Methods

        internal void InitState(double startTime)
        {
            InitialTime = startTime;
            CurrentTime = startTime;
            Range = default;
        }

        internal void ResetState() => InitState(0);

        protected override string Activate()
        {
            StateMachine.EventMouseMove.Event += OnMouseMove;
            StateMachine.EventMouseUp.Event += OnMouseUp;

            Range = new RectangleAnnotation
            {
                Fill = OxyColor.FromAColor(120, OxyColors.SkyBlue),
                MinimumX = InitialTime,
                MaximumX = CurrentTime,
                ClipByYAxis = false,
            };

            var plotModel = StateMachine.MainView.PlotModel;
            plotModel.Annotations.Add(Range);
            plotModel.InvalidatePlot(false);

            return string.Empty;
        }

        protected override string Deactivate()
        {
            StateMachine.EventMouseMove.Event -= OnMouseMove;
            StateMachine.EventMouseUp.Event -= OnMouseUp;

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        string OnMouseUp(Events.StateMouseEvent.Arguments args)
        {
            var view = StateMachine.MainView;
            var plotModel = view.PlotModel;
            var xAxis = plotModel.Axes.FirstOrDefault(a => a.IsHorizontal());

            var record = view.ViewModel.CurrentRecord;
            var from = Math.Min(InitialTime, CurrentTime);
            var to = Math.Max(InitialTime, CurrentTime);

            var updateView = false;

            var newRange = new EEGCore.Data.RecordRange()
            {
                From = Convert.ToInt32(Math.Floor(from * record.SampleRate)),
                Duration = Convert.ToInt32(Math.Ceiling((to - from) * record.SampleRate)),
            };

            using (var dialog = new Dialogs.RecordRangeForm() { Record = record, Range = newRange })
            {
                if (dialog.ShowDialog(StateMachine.MainView) == DialogResult.OK)
                {
                    record.Ranges.Add(newRange);
                    updateView = true;
                }
            }

            plotModel.Annotations.Remove(Range);
            plotModel.InvalidatePlot(false);

            ResetState();

            if (updateView)
            {
                view.UpdatePlot();
            }

            return EEGRecordState.Name;
        }

        string OnMouseMove(Events.StateMouseEvent.Arguments args)
        {
            CurrentTime = args.Time;

            var plotModel = StateMachine.MainView.PlotModel;
            var xAxis = plotModel.Axes.FirstOrDefault(a => a.IsHorizontal());
            var newStart = Math.Min(InitialTime, CurrentTime);
            var newEnd = Math.Max(InitialTime, CurrentTime);

            Range!.MinimumX = Math.Min(InitialTime, CurrentTime); ;
            Range!.MaximumX = Math.Max(InitialTime, CurrentTime);
            Range!.Text = $"From: {xAxis!.FormatValue(InitialTime)}\nTo: {xAxis!.FormatValue(CurrentTime)}";

            plotModel.InvalidatePlot(false);

            return string.Empty;
        }

        #endregion
    }
}
