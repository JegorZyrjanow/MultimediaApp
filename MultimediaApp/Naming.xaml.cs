using System.IO;
using System.Windows;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for Naming.xaml
    /// </summary>
    public partial class Naming : Window
    {
        public void ShowDialog(Window owner)
        {
            this.Owner = owner;
            this.ShowDialog();
        }

        public string FileName { get; set; }
        public string FileCategory { get; set; }
        public string FilePath { get; set; }

        public Naming(string file)
        {
            InitializeComponent();

            this.FilePath = file;            
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameForm.Text))
                FileName = MainWindow.GetFileFolderName(FilePath);
            else
                FileName = NameForm.Text + $" ({ FilePath.Substring(FilePath.LastIndexOf('.') + 1).ToUpper() })";

            FileCategory = CategoryForm.Text;

            //File.Copy(FilePath, @"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\images\memes\" + FileName);

            // Close window
            this.Close();
        }
    }
}