using System.Windows;
using System.Windows.Input;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for Naming.xaml
    /// </summary>
    public partial class NamingWindow : Window
    {
        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }

        public NamingWindow()
        {
            InitializeComponent();

            //_filePath = file;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            // Set Name from form
            if (string.IsNullOrEmpty(NameForm.Text))
                _name = Helper.GetFileName(_filePath);
            else
                _name = NameForm.Text + $" ({_filePath.Substring(_filePath.LastIndexOf('.') + 1).ToUpper() })";

            // Set Category form form
            _category = CategoryForm.Text;

            // Create Pic
            _picture = new Picture(_name, _category, _filePath);

            // Close window
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public Picture GetPic()
        {
            return _picture;
        }
    }
}