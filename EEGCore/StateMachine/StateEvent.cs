using MathNet.Numerics.RootFinding;

namespace EEGCore.StateMachine
{
    public abstract class StateEvent
    {
        public delegate void AfterEventHandler(string nextState, bool handled);

        public abstract event AfterEventHandler? AfterEvent;

        /// <summary>
        /// You have to implement firing of event and invoke AfterEvent event
        /// with arguments of next state name (or empty string) and flag
        /// whether the event firing was actually processed.
        /// </summary>
        public abstract void Fire();
    }
}
