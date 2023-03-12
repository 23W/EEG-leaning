using EEGCleaning.Model;
using EEGCore.Data;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class ICARecordState : StateBase
    {
        #region Construction

        internal ICARecordState(StateMachine stateMachine)
            : base(stateMachine)
        {
        }

        #endregion

        internal static string Name => nameof(ICARecordState);
        internal double? LeftClickTime { get; set; }
        internal double? RightClickTime { get; set; }

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
            StateMachine.MainView.ICAControl.Click += OnAbolishCA;
            StateMachine.MainView.ICAComposeControl.Click += OnRunICACompose;
            StateMachine.EventMouseDown.Event += OnMouseDown;
            StateMachine.EventMouseUp.Event += OnMouseUp;
         
            StateMachine.MainView.UpdatePlot(ModelViewMode.ICA);

            return string.Empty;

        }

        protected override string Deactivate()
        {
            StateMachine.MainView.ICAControl.Click -= OnAbolishCA;
            StateMachine.MainView.ICAComposeControl.Click -= OnRunICACompose;
            StateMachine.EventMouseDown.Event -= OnMouseDown;
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
                if (args.Sender is ComponentLead lead)
                {
                    nextState = ICAComponentContexMenuState.Name;

                    var contextMenu = StateMachine.FindState(nextState) as ICAComponentContexMenuState;
                    contextMenu?.InitState(lead);

                    handled = true;
                }
            }
            else if (LeftClickTime.HasValue)
            {
                // nothing to do
            }

            ResetActions();

            args.Handled = handled;
            return nextState;
        }

        void OnAbolishCA(object? sender, EventArgs e)
        {
            StateMachine.SwitchState(EEGRecordState.Name);
        }

        void OnRunICACompose(object? sender, EventArgs e)
        {
            StateMachine.MainView.RunICACompose();

            StateMachine.SwitchState(EEGRecordState.Name);
        }

        #endregion
    }
}
