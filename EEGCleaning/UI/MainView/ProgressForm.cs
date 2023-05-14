namespace EEGCleaning.UI.MainView
{
    public partial class ProgressForm : Form
    {
        public delegate void StartEventHandler(ProgressForm form);
        public event StartEventHandler? Start;

        public ProgressForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Start?.Invoke(this);
        }
    }
}
