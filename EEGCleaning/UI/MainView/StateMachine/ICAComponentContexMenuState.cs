using EEGCore.Data;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class ICAComponentContexMenuState : StateBase
    {
        #region Construction

        internal ICAComponentContexMenuState(StateMachine stateMachine)
            : base(stateMachine)
        {
            Menu = new ContextMenuStrip();
            Menu.Items.AddRange(new ToolStripItem[]
            {
                MenuEyeArtifact,
                MenuSuppress,
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(ICAComponentContexMenuState);

        ContextMenuStrip Menu { get; init; }
        ToolStripMenuItem MenuEyeArtifact { get; init; } = new ToolStripMenuItem("Eye Artifact");
        ToolStripMenuItem MenuSuppress { get; init; } = new ToolStripMenuItem("Suppress");

        Point Point { get; set; } = Point.Empty;

        ComponentLead ComponentLead { get; set; } = new ComponentLead();

        string PrevieousStateName { get; set; } = string.Empty;

        #endregion

        #region Methods

        internal void InitState(ComponentLead lead)
        {
            PrevieousStateName = StateMachine.CurrentStateName;
            Point = StateMachine.MainView.LastPoint;
            ComponentLead = lead;
        }

        internal void ResetState() => InitState(new ComponentLead());

        protected override string Activate()
        {
            MenuEyeArtifact.Checked = ComponentLead.ComponentType == ComponentType.EyeArtifact;
            MenuSuppress.Checked = ComponentLead.Suppress;

            MenuEyeArtifact.Click += OnMenuEyeArtifactItemClicked;
            MenuSuppress.Click += OnMenuSuppressItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuEyeArtifact.Click -= OnMenuEyeArtifactItemClicked;
            MenuSuppress.Click -= OnMenuSuppressItemClicked;

            Menu.Closed -= OnMenuClosed;
            Menu.Hide();

            ResetState();

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnMenuEyeArtifactItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.ComponentType = ComponentLead.IsArtifact ? ComponentType.Unknown : ComponentType.EyeArtifact;

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuSuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = !ComponentLead.Suppress;

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuClosed(object? sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason != ToolStripDropDownCloseReason.ItemClicked)
            {
                StateMachine.SwitchState(PrevieousStateName);
            }
        }

        #endregion
    }
}
