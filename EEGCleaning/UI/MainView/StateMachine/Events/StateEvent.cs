
namespace EEGCleaning.UI.MainView.StateMachine.Events
{
    internal abstract class StateEvent : EEGCore.StateMachine.StateEvent
    {
        internal StateMachine StateMachine { get; init; }

        internal StateEvent(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}
