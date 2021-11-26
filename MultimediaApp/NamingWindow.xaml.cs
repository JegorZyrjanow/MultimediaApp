using System.Windows;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for Naming.xaml
    /// </summary>
    public partial class NamingWindow : Window
    {
        private Picture _picture;
        private string FilePath;

        public void ShowDialog(Window owner)
        {
            this.Owner = owner;
            this.ShowDialog();
        }

        public NamingWindow(string file)
        {
            InitializeComponent();

            this.FilePath = file;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            _picture = new Picture();

            if (string.IsNullOrEmpty(NameForm.Text))
                _picture.Name = Helper.GetFileName(FilePath);
            else
                _picture.Name = NameForm.Text + $" ({ FilePath.Substring(FilePath.LastIndexOf('.') + 1).ToUpper() })";

            _picture.Category = CategoryForm.Text;

            _picture.Path = FilePath;

            //File.Copy(FilePath, @"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\images\memes\" + FileName);

            // Close window
            this.Close();
        }

        public Picture GetPic()
        {
            return _picture;
        }
    }
}