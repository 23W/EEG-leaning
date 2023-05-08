namespace EEGCleaning.UI.MainView.StateMachine
{
    internal abstract class StateBase : EEGCore.StateMachine.StateBase
    {
        #region Construction

        internal StateBase(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        #endregion

        #region Properties

        internal StateMachine StateMachine { get; init; }

        #endregion
    }
}
