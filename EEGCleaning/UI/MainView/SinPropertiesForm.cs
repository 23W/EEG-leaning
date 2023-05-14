namespace EEGCleaning.UI.MainView
{
    public partial class SinPropertiesForm : Form
    {
        #region Properties

        public int Amplitude { get; set; } = 40;
        public int Frequency { get; set; } = 10;

        #endregion

        public SinPropertiesForm()
        {
            InitializeComponent();

            m_amplitudeTextBox.Text = Amplitude.ToString();
            m_freqTextBox.Text = Frequency.ToString();
        }

        #region Event Handlers

        void OnOkClick(object sender, EventArgs e)
        {
            int amplitude = Amplitude;
            int freq = Frequency;

            if (int.TryParse(m_amplitudeTextBox.Text, out amplitude))
            {
                Amplitude = amplitude;
            }

            if (int.TryParse(m_freqTextBox.Text, out freq))
            {
                Frequency = freq;
            }
        }

        void OnValueValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !int.TryParse(((TextBox)sender).Text, out int amplitude) ||
                       (amplitude <= 0);
            if (e.Cancel)
            {
                MessageBox.Show("It must be positive, nonzero value", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion
    }
}
