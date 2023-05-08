namespace EEGCore.StateMachine
{
    public abstract class StateBase
    {
        internal protected abstract string Activate();

        internal protected abstract string Deactivate();
    }
}
