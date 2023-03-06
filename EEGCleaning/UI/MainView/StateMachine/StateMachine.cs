using Accord.Diagnostics;
using EEGCleaning.UI.MainView.StateMachine.Events;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class StateMachine : EEGCore.StateMachine.StatesCluster
    {
        #region Construction

        internal StateMachine(MainForm mainView)
        {
            MainView = mainView;

            EventMouseDown = new StateDownMouseEvent(this);
            EventMouseUp = new StateMouseEvent(this);
            EventMouseMove = new StateMouseEvent(this);

            EventMouseDown.AfterEvent += OnNextState;
            EventMouseUp.AfterEvent += OnNextState;
            EventMouseMove.AfterEvent += OnNextState;

            AddState(EEGRecordState.Name, new EEGRecordState(this));
            AddState(EEGRecordInsertNewRangeState.Name, new EEGRecordInsertNewRangeState(this));
            AddState(EEGRecordRangeContextMenuState.Name, new EEGRecordRangeContextMenuState(this));
            AddState(ICARecordState.Name, new ICARecordState(this));
            AddState(ICAComponentContexMenuState.Name, new ICAComponentContexMenuState(this));
        }

        #endregion

        #region Properties

        internal MainForm MainView { get; init; }

        #endregion

        #region Events

        internal StateDownMouseEvent EventMouseDown { get; init; }
        internal StateMouseEvent EventMouseUp { get; init; }
        internal StateMouseEvent EventMouseMove { get; init; }

        #endregion

        #region Methods

        protected override string Activate()
        {
            Debug.Assert(false, "Should not be executed");
            return string.Empty;
        }

        protected override string Deactivate()
        {
            Debug.Assert(false, "Should not be executed");
            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnNextState(string nextState, bool wasHandled)
        {
            if (wasHandled)
            {
                OnNextState(nextState);
            }
        }

        #endregion

    }
}
