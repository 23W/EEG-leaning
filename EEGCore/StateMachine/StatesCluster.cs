namespace EEGCore.StateMachine
{
    public abstract class StatesCluster : StateBase
    {
        #region State

        // Override this method and add states to cluster
        internal protected override string Activate() => string.Empty;

        // Override this method and remove states from cluster
        internal protected override string Deactivate() => string.Empty;

        #endregion

        #region Properties

        public StateBase? CurrentState => FindState(CurrentStateName);

        public string CurrentStateName { get; set; } = string.Empty;

        protected Dictionary<string, StateBase> States { get; init; } = new Dictionary<string, StateBase>();

        #endregion

        #region Methods

        public void AddState(string stateName, StateBase state) => States.Add(stateName, state);

        public void RemoveState(string stateName) => States.Remove(stateName);

        public StateBase? FindState(string stateName)
        {
            States.TryGetValue(stateName, out var currentState);
            return currentState;
        }

        public void SwitchState(string stateName) => OnNextState(stateName);

        #endregion

        #region Protected Methods

        protected void OnNextState(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                return;
            }

            if (CurrentStateName != stateName)
            {
                var currentState = FindState(CurrentStateName);
                var nextState = FindState(stateName);

                currentState?.Deactivate();
                if (nextState != default(StateBase))
                {
                    CurrentStateName = stateName;
                    stateName = nextState.Activate();
                    OnNextState(stateName);
                }
            }
        }

        #endregion
    }

}
