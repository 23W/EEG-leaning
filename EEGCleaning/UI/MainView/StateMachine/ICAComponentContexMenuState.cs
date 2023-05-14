using EEGCore.Data;
using EEGCore.Processing;

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
                MenuReferenceElectrodeArtifact,
                MenuSingleElectrodeArtifact,
                MenuSeparator,
                MenuNotSuppress,
                MenuZeroLeadSuppress,
                MenuHiPass10Suppress,
                MenuHiPass20Suppress,
                MenuHiPass30Suppress,
                MenuSeparator,
                MenuHideLead,
                MenuShowAllLeads
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(ICAComponentContexMenuState);

        ContextMenuStrip Menu { get; init; } = new ContextMenuStrip();
        ToolStripMenuItem MenuEyeArtifact { get; init; } = new ToolStripMenuItem("Eye Artifact");
        ToolStripMenuItem MenuReferenceElectrodeArtifact { get; init; } = new ToolStripMenuItem("Reference Electrode Artifact");
        ToolStripMenuItem MenuSingleElectrodeArtifact { get; init; } = new ToolStripMenuItem("Single Electrode Artifact");
        ToolStripSeparator MenuSeparator { get; init; } = new ToolStripSeparator();
        ToolStripMenuItem MenuNotSuppress { get; init; } = new ToolStripMenuItem("None");
        ToolStripMenuItem MenuZeroLeadSuppress { get; init; } = new ToolStripMenuItem("Zero Lead");
        ToolStripMenuItem MenuHiPass10Suppress { get; init; } = new ToolStripMenuItem("HiPass 10 Hz");
        ToolStripMenuItem MenuHiPass20Suppress { get; init; } = new ToolStripMenuItem("HiPass 20 Hz");
        ToolStripMenuItem MenuHiPass30Suppress { get; init; } = new ToolStripMenuItem("HiPass 30 Hz");
        ToolStripMenuItem MenuHideLead { get; init; } = new ToolStripMenuItem("Hide");
        ToolStripMenuItem MenuShowAllLeads { get; init; } = new ToolStripMenuItem("Show All");

        Point Point { get; set; } = Point.Empty;

        ICARecord IndependentComponents => StateMachine.MainView.ViewModel.IndependentComponents;
        IEnumerable<ComponentLead> VisibleComponents => StateMachine.MainView.ViewModel.VisibleLeads.Cast<ComponentLead>();
        List<string> HiddenComponentNames => StateMachine.MainView.ViewModel.HiddenLeadNames;
        ComponentLead ComponentLead { get; set; } = new ComponentLead();
        int ComponentIndex => IndependentComponents.Leads.IndexOf(ComponentLead);

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
            MenuEyeArtifact.Checked = ComponentLead.IsEyeArtifact;
            MenuReferenceElectrodeArtifact.Checked = ComponentLead.IsReferenceElectrodeArtifact;
            MenuSingleElectrodeArtifact.Checked = ComponentLead.IsSingleElectrodeArtifact;
            MenuNotSuppress.Checked = ComponentLead.Suppress == SuppressType.None;
            MenuZeroLeadSuppress.Checked = ComponentLead.Suppress == SuppressType.ZeroLead;
            MenuHiPass10Suppress.Checked = ComponentLead.Suppress == SuppressType.HiPass10;
            MenuHiPass20Suppress.Checked = ComponentLead.Suppress == SuppressType.HiPass20;
            MenuHiPass30Suppress.Checked = ComponentLead.Suppress == SuppressType.HiPass30;
            MenuShowAllLeads.Enabled = HiddenComponentNames.Any(name => IndependentComponents.Leads.Any(c => c.Name == name));

            MenuEyeArtifact.Click += OnMenuEyeArtifactItemClicked;
            MenuReferenceElectrodeArtifact.Click += OnMenuReferenceElectrodeArtifactItemClicked;
            MenuSingleElectrodeArtifact.Click += OnMenuSingleElectrodeArtifactItemClicked;
            MenuNotSuppress.Click += OnMenuNoneSuppressItemClicked;
            MenuZeroLeadSuppress.Click += OnMenuZeroLeadSuppressItemClicked;
            MenuHiPass10Suppress.Click += OnMenuHiPass10SuppressItemClicked;
            MenuHiPass20Suppress.Click += OnMenuHiPass20SuppressItemClicked;
            MenuHiPass30Suppress.Click += OnMenuHiPass30SuppressItemClicked;
            MenuHideLead.Click += OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click += MenuShowAllLeadsItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuEyeArtifact.Click -= OnMenuEyeArtifactItemClicked;
            MenuReferenceElectrodeArtifact.Click -= OnMenuReferenceElectrodeArtifactItemClicked;
            MenuSingleElectrodeArtifact.Click -= OnMenuSingleElectrodeArtifactItemClicked;
            MenuNotSuppress.Click -= OnMenuNoneSuppressItemClicked;
            MenuZeroLeadSuppress.Click -= OnMenuZeroLeadSuppressItemClicked;
            MenuHiPass10Suppress.Click -= OnMenuHiPass10SuppressItemClicked;
            MenuHiPass20Suppress.Click -= OnMenuHiPass20SuppressItemClicked;
            MenuHiPass30Suppress.Click -= OnMenuHiPass30SuppressItemClicked;
            MenuHideLead.Click -= OnMenuHideLeadItemClicked;
            MenuShowAllLeads.Click -= MenuShowAllLeadsItemClicked;

            Menu.Closed -= OnMenuClosed;
            Menu.Hide();

            ResetState();

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnMenuEyeArtifactItemClicked(object? sender, EventArgs e)
        {
            if (ComponentLead.IsEyeArtifact)
            {
                ComponentLead.RemoveArtifactInfo(ArtifactType.EyeArtifact);

                OnMenuNoneSuppressItemClicked(sender, e);
            }
            else
            {
                ComponentLead.AddArtifactInfo(ArtifactType.EyeArtifact);

                StateMachine.MainView.UpdatePlot();
                StateMachine.SwitchState(PrevieousStateName);
            }
        }

        void OnMenuReferenceElectrodeArtifactItemClicked(object? sender, EventArgs e)
        {
            if (ComponentLead.IsReferenceElectrodeArtifact)
            {
                ComponentLead.RemoveArtifactInfo(ArtifactType.ReferenceElectrodeArtifact);

                OnMenuNoneSuppressItemClicked(sender, e);
            }
            else
            {
                ComponentLead.AddArtifactInfo(ArtifactType.ReferenceElectrodeArtifact);

                StateMachine.MainView.UpdatePlot();
                StateMachine.SwitchState(PrevieousStateName);
            }
        }

        void OnMenuSingleElectrodeArtifactItemClicked(object? sender, EventArgs e)
        {
            if (ComponentLead.IsSingleElectrodeArtifact)
            {
                ComponentLead.RemoveArtifactInfo(ArtifactType.SingleElectrodeArtifact);

                OnMenuNoneSuppressItemClicked(sender, e);
            }
            else
            {
                ComponentLead.AddArtifactInfo(ArtifactType.SingleElectrodeArtifact);

                StateMachine.MainView.UpdatePlot();
                StateMachine.SwitchState(PrevieousStateName);
            }
        }

        void OnMenuNoneSuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.None;
            IndependentComponents.BuildLeadAlternativeSuppress(ComponentIndex);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuZeroLeadSuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.ZeroLead;
            IndependentComponents.BuildLeadAlternativeSuppress(ComponentIndex);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHiPass10SuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.HiPass10;
            IndependentComponents.BuildLeadAlternativeSuppress(ComponentIndex);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHiPass20SuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.HiPass20;
            IndependentComponents.BuildLeadAlternativeSuppress(ComponentIndex);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHiPass30SuppressItemClicked(object? sender, EventArgs e)
        {
            ComponentLead.Suppress = SuppressType.HiPass30;
            IndependentComponents.BuildLeadAlternativeSuppress(ComponentIndex);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuHideLeadItemClicked(object? sender, EventArgs e)
        {
            HiddenComponentNames.Add(ComponentLead.Name);

            StateMachine.MainView.UpdatePlot();
            StateMachine.SwitchState(PrevieousStateName);
        }

        void MenuShowAllLeadsItemClicked(object? sender, EventArgs e)
        {
            HiddenComponentNames.Clear();

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
