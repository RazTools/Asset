namespace AssetGUI
{
    public partial class InputForm : Form
    {
        public string Input => numericUpDown.Text;
        public InputForm()
        {
            InitializeComponent();
        }
    }
}
