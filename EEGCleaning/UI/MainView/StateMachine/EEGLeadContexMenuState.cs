using EEGCore.Data;
using EEGCore.Processing.Generators;

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
                MenuModelLeadArtifact,
                MenuModelReferenceArtifact,
                new ToolStripSeparator(),
                MenuShowOnlyLead,
                MenuHideLead,
                MenuShowAllLeads
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(EEGLeadContexMenuState);

        ContextMenuStrip Menu { get; init; } = new ContextMenuStrip();
        ToolStripMenuItem MenuModelLeadArtifact { get; init; } = new ToolStripMenuItem("Add Electrode Artifact");
        ToolStripMenuItem MenuModelReferenceArtifact { get; init; } = new ToolStripMenuItem("Add Reference Artifact");
        ToolStripMenuItem MenuShowOnlyLead { get; init; } = new ToolStripMenuItem("Show Only");
        ToolStripMenuItem MenuHideLead { get; init; } = new ToolStripMenuItem("Hide");
        ToolStripMenuItem MenuShowAllLeads { get; init; } = new ToolStripMenuItem("Show All");

        Point Point { get; set; } = Point.Empty;

        Record EEGRecord => StateMachine.MainView.ViewModel.ProcessedRecord;
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

        internal void ResetState() => InitState(new Lead());

        protected override string Activate()
        {
            MenuShowAllLeads.Enabled = HiddenLeadNames.Any(name => VisibleRecord.Leads.Any(c => c.Name == name));

            MenuModelLeadArtifact.Click += OnMenuLeadArtifactItemClicked;
            MenuModelReferenceArtifact.Click += OnMenuReferenceArtifactItemClicked;
            MenuShowOnlyLead.Click += OnMenuShowOnlyLeadItemClicked;
            MenuHideLead.Click += OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click += OnMenuShowAllLeadsItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuModelLeadArtifact.Click -= OnMenuLeadArtifactItemClicked;
            MenuModelReferenceArtifact.Click -= OnMenuReferenceArtifactItemClicked;
            MenuShowOnlyLead.Click -= OnMenuShowOnlyLeadItemClicked;
            MenuHideLead.Click -= OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click -= OnMenuShowAllLeadsItemClicked;

            Menu.Closed -= OnMenuClosed;
            Menu.Hide();

            ResetState();

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnMenuLeadArtifactItemClicked(object? sender, EventArgs e)
        {
            var lead = EEGLead;

            StateMachine.SwitchState(PrevieousStateName);

            using (var propForm = new SinPropertiesForm())
            {
                if (propForm.ShowDialog(StateMachine.MainView) == DialogResult.OK)
                {
                    var generator = new SinGenerator() 
                    { 
                        Amplitude = propForm.Amplitude,
                        Frequence = propForm.Frequency,
                        SampleRate = VisibleRecord.SampleRate
                    };
                    generator.Generate(lead.Samples, true);

                    StateMachine.MainView.UpdatePlot();
                }
            }
        }

        void OnMenuReferenceArtifactItemClicked(object? sender, EventArgs e)
        {
            var record = EEGRecord;

            StateMachine.SwitchState(PrevieousStateName);

            using (var propForm = new SinPropertiesForm())
            {
                if (propForm.ShowDialog(StateMachine.MainView) == DialogResult.OK)
                {
                    var generator = new SinGenerator()
                    {
                        Amplitude = propForm.Amplitude,
                        Frequence = propForm.Frequency,
                        SampleRate = VisibleRecord.SampleRate
                    };
                    record.Leads.ForEach(l => generator.Generate(l.Samples, true));

                    StateMachine.MainView.UpdatePlot();
                }
            }
        }

        void OnMenuShowOnlyLeadItemClicked(object? sender, EventArgs e)
        {
            HiddenLeadNames.Clear();
            HiddenLeadNames.AddRange(VisibleLeads.Where(l => l.Name != EEGLead.Name).Select(l => l.Name));

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHideLeadItemClicked(object? sender, EventArgs e)
        {
            HiddenLeadNames.Add(EEGLead.Name);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuShowAllLeadsItemClicked(object? sender, EventArgs e)
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
