using System.Windows;
using System.Windows.Input;

namespace MultimediaApp
{
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

            DataContext = new NamingViewModel();
            //_filePath = file;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}