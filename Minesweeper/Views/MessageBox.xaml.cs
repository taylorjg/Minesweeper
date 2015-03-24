namespace Minesweeper.Views
{
    public partial class MessageBox
    {
        public MessageBox()
        {
            InitializeComponent();
            DataContext = this;

            OkButton.Click += (_, __) =>
                {
                    DialogResult = true;
                    Close();
                };
        }

        public string MessageText { get; set; }
    }
}
