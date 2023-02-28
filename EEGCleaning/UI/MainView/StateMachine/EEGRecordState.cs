using EEGCleaning.Model;
using EEGCore.Data;
using System.Windows.Forms;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class EEGRecordState : StateBase
    {
        #region Construction

        internal EEGRecordState(StateMachine stateMachine)
            : base(stateMachine)
        {
        }

        #endregion

        #region Properties

        internal static string Name => nameof(EEGRecordState);
        internal double? LeftClickTime { get; set; }
        internal double? RightClickTime { get; set; }

        #endregion

        #region Methods

        void LeftClickAction(double time)
        {
            ResetActions();
            LeftClickTime = time;
        }
        void RightClickAction(double time)
        {
            ResetActions();
            RightClickTime = time;
        }

        void ResetActions()
        {
            LeftClickTime = null;
            RightClickTime = null;
        }


        protected override string Activate()
        {
            StateMachine.MainView.ICAButton.Click += OnRunICA;
            StateMachine.EventMouseDown.Event += OnMouseDown;
            StateMachine.EventMouseMove.Event += OnMouseMove;
            StateMachine.EventMouseUp.Event += OnMouseUp;

            StateMachine.MainView.UpdatePlot(ModelViewMode.Record);

            return string.Empty;
        }

        protected override string Deactivate()
        {
            StateMachine.MainView.ICAButton.Click -= OnRunICA;
            StateMachine.EventMouseDown.Event -= OnMouseDown;
            StateMachine.EventMouseMove.Event -= OnMouseMove;
            StateMachine.EventMouseUp.Event -= OnMouseUp;

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        string OnMouseDown(Events.StateDownMouseEvent.Arguments args)
        {
            var nextState = string.Empty;

            ResetActions();

            if (args.Button == Events.StateDownMouseEvent.MouseButton.Left)
            {
                LeftClickAction(args.Time);
            }
            else if (args.Button == Events.StateDownMouseEvent.MouseButton.Right && 
                     args.Sender is not Record)
            {
                RightClickAction(args.Time);
            }
            else
            {
                args.Handled = false;
            }

            return nextState;
        }

        string OnMouseUp(Events.StateMouseEvent.Arguments args)
        {
            var nextState = string.Empty;

            bool handled = false;

            if (RightClickTime.HasValue)
            {
                if (args.Sender is RecordRange range)
                {
                    nextState = EEGRecordRangeContextMenuState.Name;

                    var insertState = StateMachine.FindState(nextState) as EEGRecordRangeContextMenuState;
                    insertState?.InitState(range);

                    handled = true;
                }
            }
            else if (LeftClickTime.HasValue)
            {
                if (args.Sender is RecordRange range)
                {
                    handled = true;

                    var view = StateMachine.MainView;
                    var record = view.ViewModel.CurrentRecord;

                    using (var dialog = new Dialogs.RecordRangeForm() { Record = record, Range = range })
                    {
                        if (dialog.ShowDialog(StateMachine.MainView) == DialogResult.OK)
                        {
                            view.UpdatePlot();
                        }
                    }
                }
            }

            ResetActions();

            args.Handled = handled;
            return nextState;
        }

        string OnMouseMove(Events.StateMouseEvent.Arguments args)
        {
            var nextState = string.Empty;

            if (LeftClickTime.HasValue &&
                StateMachine.MainView.IsPanDistance(LeftClickTime.Value, args.Time))
            {
                nextState = EEGRecordInsertNewRangeState.Name;

                var insertState = StateMachine.FindState(nextState) as EEGRecordInsertNewRangeState;
                insertState?.InitState(LeftClickTime.Value);

                ResetActions();
            }
            else
            {
                args.Handled = false;
            }

            return nextState;
        }

        void OnRunICA(object? sender, EventArgs e)
        {
            StateMachine.MainView.RunICA();
            StateMachine.SwitchState(ICARecordState.Name);
        }

        #endregion
    }
}
