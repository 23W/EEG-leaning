using OxyPlot;
using EEGCore.StateMachine;

namespace EEGCleaning.UI.MainView.StateMachine.Events
{
    internal class StateMouseEvent : StateEvent
    {
        #region Nested classes

        internal class Arguments
        {
            internal object Sender { get; set; } = new object();

            internal double Time { get; set; } = 0;

            internal bool Handled { get; set; } = true;
        }

        #endregion

        #region Properties

        internal delegate string EventHandler(Arguments args);

        internal event EventHandler? Event;
        public override event AfterEventHandler? AfterEvent;

        Arguments EventArguments { get; init; } = new Arguments();

        #endregion

        #region Construction

        internal StateMouseEvent(StateMachine stateMachine)
            : base(stateMachine)
        {
        }
        
        #endregion

        #region Methods

        internal void Init(MainForm mainView, object? sender, OxyMouseEventArgs e)
        {
            var model = StateMachine.MainView.PlotModel;
            var xAxis = model.Axes.FirstOrDefault(a => a.IsHorizontal());

            EventArguments.Sender = mainView.GetViewModelObject(sender);
            EventArguments.Time = (xAxis?.InverseTransform(e.Position.X) ?? 0);
        }

        public override void Fire()
        {
            var handler = Event?.GetInvocationList()
                                .FindTarget<EventHandler>(StateMachine.CurrentState);

            EventArguments.Handled = handler != default;
            var nextSate = handler?.Invoke(EventArguments) ?? string.Empty;

            AfterEvent?.Invoke(nextSate, EventArguments.Handled);
        }

        #endregion
    }


    internal class StateDownMouseEvent : StateMouseEvent
    {
        #region Nested classes

        internal enum MouseButton
        {
            Left,
            Right,
        }

        internal new class Arguments : StateMouseEvent.Arguments
        {
            internal MouseButton Button { get; set; } = MouseButton.Left;
        }

        #endregion

        #region Properties

        internal new delegate string EventHandler(Arguments args);

        internal new event EventHandler? Event;
        public override event AfterEventHandler? AfterEvent;

        Arguments EventArguments { get; init; } = new Arguments();

        #endregion

        #region Construction

        internal StateDownMouseEvent(StateMachine stateMachine)
            : base(stateMachine)
        {
        }

        #endregion

        #region Methods

        internal void Init(MainForm mainView, object? sender, OxyMouseDownEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case OxyMouseButton.Left: EventArguments.Button = MouseButton.Left; break;
                case OxyMouseButton.Right: EventArguments.Button = MouseButton.Right; break;
            }

            var model = StateMachine.MainView.PlotModel;
            var xAxis = model.Axes.FirstOrDefault(a => a.IsHorizontal());

            EventArguments.Sender = mainView.GetViewModelObject(sender);
            EventArguments.Time = (xAxis?.InverseTransform(e.Position.X) ?? 0);
        }

        public override void Fire()
        {
            var handler = Event?.GetInvocationList()
                                .FindTarget<EventHandler>(StateMachine.CurrentState);
         
            EventArguments.Handled = handler != default;
            var nextSate = handler?.Invoke(EventArguments) ?? string.Empty;

            AfterEvent?.Invoke(nextSate, EventArguments.Handled);
        }

        #endregion
    }
}
