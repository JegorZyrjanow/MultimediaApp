using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get every logical drive on the machine
            foreach (var firstFolder in Directory.EnumerateDirectories("../../images/"))
            {
                // Create a new item for it
                var item = new TreeViewItem()
                {
                    // Set the header
                    Header = GetFileFolderName(firstFolder),
                    // And the full path
                    Tag = firstFolder
                };

                // Add a dummy item
                item.Items.Add(null);

                // Listen out for item being expanded
                item.Expanded += Folder_Expanded;

                // Add it to the main tree-view
                FolderView.Items.Add(item);
            }
        }

        #endregion

        #region Folder Expanded

        /// <summary>
        /// When a folder is expanded, find the sub folders/files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks

            var item = (TreeViewItem)sender;

            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            // Clear dummy data
            item.Items.Clear();

            // Get full path
            var fullPath = (string)item.Tag;

            #endregion

            #region Get Folders

            // Create a blank list for directories
            var directories = new List<string>();

            // Try and get directories from the folder
            // ignoring any issues doing so
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            // For each directory...
            directories.ForEach(directoryPath =>
            {
                // Create directory item
                var subItem = new TreeViewItem()
                {
                    // Set header as folder name
                    Header = GetFileFolderName(directoryPath),
                    // And tag as full path
                    Tag = directoryPath
                };

                // Add dummy item so we can expand folder
                subItem.Items.Add(null);

                // Handle expanding
                subItem.Expanded += Folder_Expanded;

                // Add this item to the parent
                item.Items.Add(subItem);
            });

            #endregion

            #region Get Files

            // Create a blank list for files
            var files = new List<string>();

            // Try and get files from the folder
            // ignoring any issues doing so
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            // For each file...
            files.ForEach(filePath =>
            {
                // Create file item
                var subItem = new TreeViewItem()
                {
                    // Set header as file name
                    Header = GetFileFolderName(filePath),
                    // And tag as full path
                    Tag = filePath
                };

                // Add this item to the parent
                item.Items.Add(subItem);

                subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            });

            #endregion

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        #endregion

        #region Item Selected

        /// <summary>
        /// Show selected picture in the ImageBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (TreeViewItem)sender;
            
            var fullPath = (string)item.Tag;

            List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

            if (imageExtensions.Contains(Path.GetExtension(fullPath).ToUpperInvariant()))
            {
                ImageBox.Source = new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
            }
            else return;
        }

        #endregion

        #region Add Picture

        /// <summary>
        /// Add selected picture to the common folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter
            openFileDialog.Filter = "Image Files| *.jpg; *.jpeg; *.png;";

            // If canceled, close FileDialog
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            // Get selected file
            var fileName = openFileDialog.FileName;

            Naming namingWindow = new Naming(fileName);
            namingWindow.ShowDialog(this);

            // Copy selected file into common images directory
            //File.Copy(fileName, @"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\images\memes\" + GetFileFolderName(fileName));
        }

        #endregion

        #region XML

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public List<Meme> memes;

        #region Download list from xml

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Meme>));

            using (FileStream fs = new FileStream("../../App_Data/memes.xml", FileMode.OpenOrCreate))
            {
                memes = (List<Meme>)formatter.Deserialize(fs);

            }
        }

        #endregion

        #region Save сurrent list in xml

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            #region memes check and write a list from it

            // Create a blank list for files
            var files = new List<string>();

            // Try and get files from the folder
            // ignoring any issues doing so
            try
            {
                var fs = Directory.GetFiles("../../images/memes/", "*.jpg");

                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            files.ForEach(filePath =>
            {
                // Create meme item
                var memePic = new Meme
                {
                    Name = GetFileFolderName(filePath),

                    Category = "0",

                    Uri = filePath
                };

                memes.Add(memePic);

            });

            #endregion

            XmlSerializer formatter = new XmlSerializer(typeof(List<Meme>));

            using (FileStream fs = new FileStream("../../App_Data/memes.xml", FileMode.Create))
            {
                formatter.Serialize(fs, memes);
            }

            //File.WriteAllText(@"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\App_Data\data1.json", json);

        }

        #endregion

        #endregion
    }
}