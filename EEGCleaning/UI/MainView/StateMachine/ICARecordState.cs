using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Methods

        protected override string Activate()
        {
            return string.Empty;
        }

        protected override string Deactivate()
        {
            return string.Empty;
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
