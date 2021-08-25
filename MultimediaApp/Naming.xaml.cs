using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileCategory { get; set; }

        public Naming(string file)
        {
            InitializeComponent();

            this.FilePath = file;            
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            FileName = MainWindow.GetFileFolderName(FilePath);

            FileName = NameForm.Text + FilePath.Substring(FilePath.LastIndexOf('.'));

            FileCategory = CategoryForm.Text;

            File.Copy(FilePath, @"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\images\memes\" + FileName);

            // Close window
            this.Close();
        }
    }
}