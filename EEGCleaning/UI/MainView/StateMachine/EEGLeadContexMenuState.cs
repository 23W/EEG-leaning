using EEGCore.Data;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class EEGLeadContexMenuState : StateBase
    {
        #region Construction

        internal EEGLeadContexMenuState(StateMachine stateMachine)
            : base(stateMachine)
        {
            Menu.Items.AddRange(new ToolStripItem[]
            {
                MenuHideLead,
                MenuShowAllLeads
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(EEGLeadContexMenuState);

        ContextMenuStrip Menu { get; init; } = new ContextMenuStrip();
        ToolStripMenuItem MenuHideLead { get; init; } = new ToolStripMenuItem("Hide");
        ToolStripMenuItem MenuShowAllLeads { get; init; } = new ToolStripMenuItem("Show All");

        Point Point { get; set; } = Point.Empty;

        Record VisibleRecord => StateMachine.MainView.ViewModel.VisibleRecord;
        IEnumerable<Lead> VisibleLeads => StateMachine.MainView.ViewModel.VisibleLeads.Cast<Lead>();
        List<string> HiddenLeadNames => StateMachine.MainView.ViewModel.HiddenLeadNames;
        Lead EEGLead { get; set; } = new Lead();

        string PrevieousStateName { get; set; } = string.Empty;

        #endregion

        #region Methods

        internal void InitState(Lead lead)
        {
            PrevieousStateName = StateMachine.CurrentStateName;
            Point = StateMachine.MainView.LastPoint;
            EEGLead = lead;
        }

        internal void ResetState() => InitState(new ComponentLead());

        protected override string Activate()
        {
            MenuShowAllLeads.Enabled = HiddenLeadNames.Any(name => VisibleRecord.Leads.Any(c => c.Name == name));

            MenuHideLead.Click += OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click += MenuShowAllLeadsItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuHideLead.Click -= OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click -= MenuShowAllLeadsItemClicked;

            Menu.Closed -= OnMenuClosed;
            Menu.Hide();

            ResetState();

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnMenuHideLeadItemClicked(object? sender, EventArgs e)
        {
            HiddenLeadNames.Add(EEGLead.Name);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void MenuShowAllLeadsItemClicked(object? sender, EventArgs e)
        {
            HiddenLeadNames.Clear();

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
