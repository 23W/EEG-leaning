using EEGCore.Data;

namespace EEGCleaning.UI.Dialogs
{
    public partial class RecordRangeForm : Form
    {
        internal Record Record { get; set; } = new Record();

        internal RecordRange Range { get; set; } = new RecordRange();

        public RecordRangeForm()
        {
            InitializeComponent();
        }

        void OnRangeTimeFromFormat(object? sender, ConvertEventArgs e)
        {
            e.Value = TimeSpan.FromSeconds(Range.From / Record.SampleRate).ToString();
        }

        void OnRangeTimeToFormat(object? sender, ConvertEventArgs e)
        {
            e.Value = TimeSpan.FromSeconds((Range.From + Range.Duration) / Record.SampleRate).ToString();
        }

        void OnNameValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(m_nameTextBox.Text.Trim()))
            {
                MessageBox.Show("Empty name is not allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        void OnLoad(object sender, EventArgs e)
        {
            var nameBinding = new Binding(nameof(m_nameTextBox.Text), Range, nameof(Range.Name), false, DataSourceUpdateMode.OnValidation);
            var fromBinding = new Binding(nameof(m_nameTextBox.Text), Range, nameof(Range.From), false, DataSourceUpdateMode.Never);
            var toBinding = new Binding(nameof(m_nameTextBox.Text), Range, nameof(Range.Duration), false, DataSourceUpdateMode.Never);
            fromBinding.Format += OnRangeTimeFromFormat;
            toBinding.Format += OnRangeTimeToFormat;

            m_nameTextBox.DataBindings.Add(nameBinding);
            m_fromTextBox.DataBindings.Add(fromBinding);
            m_toTextBox.DataBindings.Add(toBinding);
        }
    }
}
