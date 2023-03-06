using EEGCore.Data;

namespace EEGCleaning.UI.MainView.StateMachine
{
    internal class EEGRecordRangeContextMenuState : StateBase
    {
        #region Construction

        internal EEGRecordRangeContextMenuState(StateMachine stateMachine)
            : base(stateMachine)
        {
            Menu = new ContextMenuStrip();
            Menu.Items.AddRange(new ToolStripItem[]
            {
                MenuICAItem,
                MenuEditItem,
                MenuDeleteItem,
            });
        }

        #endregion

        #region Properties

        internal static string Name => nameof(EEGRecordRangeContextMenuState);

        ContextMenuStrip Menu { get; init; }
        ToolStripMenuItem MenuICAItem { get; init; } = new ToolStripMenuItem("ICA");
        ToolStripMenuItem MenuEditItem { get; init; } = new ToolStripMenuItem("Edit");
        ToolStripMenuItem MenuDeleteItem { get; init; } = new ToolStripMenuItem("Delete");

        Point Point { get; set; } = Point.Empty;

        RecordRange Range { get; set; } = new RecordRange();

        string PrevieousStateName { get; set; } = string.Empty;

        #endregion

        #region Methods

        internal void InitState(RecordRange range)
        {
            PrevieousStateName = StateMachine.CurrentStateName;
            Point = StateMachine.MainView.LastPoint;
            Range = range;
        }

        internal void ResetState() => InitState(new RecordRange());

        protected override string Activate()
        {
            MenuICAItem.Click += OnMenuICAItemClicked;
            MenuEditItem.Click += OnMenuEditItemClicked;
            MenuDeleteItem.Click += OnMenuDeleteItemClicked;

            Menu.Show(StateMachine.MainView, Point);
            Menu.Closed += OnMenuClosed;

            return string.Empty;
        }

        protected override string Deactivate()
        {
            MenuICAItem.Click -= OnMenuICAItemClicked;
            MenuEditItem.Click -= OnMenuEditItemClicked;
            MenuDeleteItem.Click -= OnMenuDeleteItemClicked;

            Menu.Closed -= OnMenuClosed;
            Menu.Hide();

            ResetState();

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        void OnMenuICAItemClicked(object? sender, EventArgs e)
        {
            var view = StateMachine.MainView;

            view.RunICADecompose(Range);
            StateMachine.SwitchState(ICARecordState.Name);
        }

        void OnMenuEditItemClicked(object? sender, EventArgs e)
        {
            var view = StateMachine.MainView;
            var record = view.ViewModel.ProcessedRecord;

            using (var dialog = new Dialogs.RecordRangeForm() { Record = record, Range = Range })
            {
                if (dialog.ShowDialog(StateMachine.MainView) == DialogResult.OK)
                {
                    view.UpdatePlot();
                }
            }

            StateMachine.SwitchState(PrevieousStateName);
        }

        void OnMenuDeleteItemClicked(object? sender, EventArgs e)
        {
            var view = StateMachine.MainView;
            var record = view.ViewModel.ProcessedRecord;

            record.Ranges.Remove(Range);
            view.UpdatePlot();

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
