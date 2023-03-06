using EEGCore.Data;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class ICAComponentContexMenuState : StateBase
    {
        #region Construction

        internal ICAComponentContexMenuState(StateMachine stateMachine)
            : base(stateMachine)
        {
            Menu.Items.AddRange(new ToolStripItem[]
            {
                MenuEyeArtifact,
                MenuSeparator,
                MenuNotSuppress,
                MenuZeroLeadSuppress,
                MenuHiPass30Suppress,
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(ICAComponentContexMenuState);

        ContextMenuStrip Menu { get; init; } = new ContextMenuStrip();
        ToolStripMenuItem MenuEyeArtifact { get; init; } = new ToolStripMenuItem("Eye Artifact");
        ToolStripSeparator MenuSeparator { get; init; } = new ToolStripSeparator();
        ToolStripMenuItem MenuNotSuppress { get; init; } = new ToolStripMenuItem("None");
        ToolStripMenuItem MenuZeroLeadSuppress { get; init; } = new ToolStripMenuItem("Zero Lead");
        ToolStripMenuItem MenuHiPass30Suppress { get; init; } = new ToolStripMenuItem("HiPass 30 Hz");

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
            MenuNotSuppress.Checked = ComponentLead.Suppress == SuppressType.None;
            MenuZeroLeadSuppress.Checked = ComponentLead.Suppress == SuppressType.ZeroLead;
            MenuHiPass30Suppress.Checked = ComponentLead.Suppress == SuppressType.HiPass30;

            MenuEyeArtifact.Click += OnMenuEyeArtifactItemClicked;
            MenuNotSuppress.Click += OnMenuNoneSuppressItemClicked;
            MenuZeroLeadSuppress.Click += OnMenuZeroLeadSuppressItemClicked;
            MenuHiPass30Suppress.Click += OnMenuHiPass30SuppressItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuEyeArtifact.Click -= OnMenuEyeArtifactItemClicked;
            MenuNotSuppress.Click -= OnMenuNoneSuppressItemClicked;
            MenuZeroLeadSuppress.Click -= OnMenuZeroLeadSuppressItemClicked;
            MenuHiPass30Suppress.Click -= OnMenuHiPass30SuppressItemClicked;

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

        void OnMenuNoneSuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.None;

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuZeroLeadSuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.ZeroLead;

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHiPass30SuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.HiPass30;

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
