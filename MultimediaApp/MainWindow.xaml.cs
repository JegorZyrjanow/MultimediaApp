using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Globalization;
using System.Linq;
using MultimediaApp.Properties;
using System.Threading;
using MultimediaApp.Library;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();
        }

        //private XmlFormatter _xmlFormatter = new XmlFormatter();
        //private PictureCollection _collection;
        //private Caretaker _collectionCaretaker;
        //private List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        //private CollectionEnumerator _enumerator = new CollectionEnumerator();
        //private string XmlPath = "../../AppData/memes.xml";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            {
                //var categoriesList = new List<string>();
                //XmlDocument doc = new XmlDocument();
                //doc.Load("../../App_Data/memes.xml");
                //XmlNodeList elementList = doc.GetElementsByTagName("Category");
                //for (int i = 0; i < elementList.Count; i++)
                //{
                //    _Categories.Add(elementList[i].InnerText);
                //}

                //categoriesList = uniqueCategoriesList; //??

                // Add pics to list from common image folder
                {
                    //// Create a blank list for files
                    //var files = new List<string>();

                    //// Try and get files from the folder
                    //// ignoring any issues doing so
                    //try
                    //{
                    //    var fs = Directory.GetFiles("../../images/memes/", "*.jpg");

                    //    if (fs.Length > 0)
                    //        files.AddRange(fs);
                    //}
                    //catch { }

                    //files.ForEach(filePath =>
                    //{
                    //    // Create meme item
                    //    var memePic = new Meme
                    //    {
                    //        Name = GetFileFolderName(filePath),

                    //        Category = "0",

                    //        Uri = Path.GetFullPath(filePath)
                    //    };

                    //    memes.Add(memePic);

                    //});
                }
                // Get every logical drive on the machine
                {
                    //// Get every logical drive on the machine
                    //foreach (var firstFolder in Directory.EnumerateDirectories("../../images/"))
                    //{
                    //    // Create a new item for it
                    //    var item = new TreeViewItem()
                    //    {
                    //        // Set the header
                    //        Header = GetFileFolderName(firstFolder),
                    //        // And the full path
                    //        Tag = firstFolder
                    //    };

                    //    // Add a dummy item
                    //    item.Items.Add(null);

                    //    // Listen out for item being expanded
                    //    item.Expanded += Folder_Expanded;

                    //    // Add it to the main tree-view
                    //    FolderView.Items.Add(item);
                    //}
                }
            }
        }
        
        //private void DisplayImage(TreeViewItem item)
        //{
        //    string fullPath = (string)item.Tag;
        //    //Uri relPath = new Uri(Directory.GetCurrentDirectory()).MakeRelativeUri(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));

        //    if (_imageExtensions.Contains(Path.GetExtension(fullPath).ToUpperInvariant()))
        //        ImageBox.Source = new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
        //    else
        //        return;

        //    // Set checker true to delete method
        //    _itemWasChosen = true;
        //}

        private void DisplayImageAt(object sender)
        {
            TreeViewItem treeItem = sender as TreeViewItem;
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                bitmapImage = new BitmapImage(new Uri(treeItem.Tag.ToString())); //??
            }
            catch (FileNotFoundException)
            {
                bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/Images/notFound.png"));
            }
            ImageBox.Source = bitmapImage;
        }

        #region Folder Expanded (NOT USED)

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
                    //Header = GetFileFolderName(directoryPath),
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
                    //Header = GetFileFolderName(filePath),
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

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) // id сделай чтобы вместо ссылки получал...
        {
            DisplayImageAt(sender);
            //UIElement uIElement = sender as UIElement;
            //int selected = int.Parse(uIElement.Uid);
            //int selectedItem = CollectionView.Items.IndexOf(CollectionView.SelectedItem); // for id only
            //_chosenItemId = CollectionView.Items.IndexOf(CollectionView.SelectedItem);            
            //ItemIdLabel.Content = selected;
            //TVIIdLabel.Content = selectedItem;
        }

        //public static string GetFileName(string path)
        //{
        //    // If we have no path, return empty
        //    if (string.IsNullOrEmpty(path))
        //        return string.Empty;

        //    // Make all slashes back slashes
        //    var normalizedPath = path.Replace('/', '\\');

        //    // Find the last backslash in the path
        //    var lastIndex = normalizedPath.LastIndexOf('\\');

        //    // If we don't find a backslash, return the path itself
        //    if (lastIndex <= 0)
        //        return path;

        //    // Return the name after the last back slash
        //    return path.Substring(lastIndex + 1);
        //}
        
        private void DisplayAll()
        {
            CollectionView.Items.Clear();
            for (int i = 0; i < _collection.GetCollection().Count; i++)
            {
                CollectionView.Items.Add(MakeTreeViewItem(_collection.GetCollection()[i]));
            }            
            foreach (TreeViewItem treeViewItem in CollectionView.Items)
            {
                treeViewItem.Visibility = Visibility.Visible;
            }
        }
        
        private void AddNewPicButton_Click(object sender, RoutedEventArgs e)
        {
            _collectionCaretaker.Backup();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter
            openFileDialog.Filter = "Image Files| *.jpg; *.jpeg; *.png;";

            // If canceled, close FileDialog
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            // Get selected file
            string filePath = openFileDialog.FileName;

            // Check file for correct ext, and exit if invalid
            if (!_imageExtensions.Contains(Path.GetExtension(filePath).ToUpperInvariant()))
            {
                System.Windows.MessageBox.Show("I don\'t get it..");
                return;
            }

            NamingWindow namingWindow = new NamingWindow(filePath);
            // Open Dialog Window
            namingWindow.ShowDialog(this);
            // Add new pic in the CacheList of memes
            _collection.Add(namingWindow.GetMeme());
            if (!CategoriesComboBox.Items.Contains(_collection.GetLastCategory()))
            {
                CategoriesComboBox.Items.Add(_collection.GetLastCategory());
            }
            DisplayAll();
        }

        private void RemovePicture_Click(object sender, RoutedEventArgs e)
        {
            _collectionCaretaker.Backup();

            int selectedId = CollectionView.Items.IndexOf(CollectionView.SelectedItem);

            //if(_collection.GetCollection().Contains(Picture.Category))


            _collection.RemoveAt(selectedId);
            CollectionView.Items.Remove(CollectionView.SelectedItem);

            ImageBox.Source = null;
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            CategoriesComboBox.SelectedItem = 0;
            CategoriesComboBox.SelectedIndex = 0;
            SearchBox.Text = null;
            _collectionCaretaker.Undo();
            DisplayAll();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _collectionCaretaker.Undo();
            DisplayAll();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _xmlFormatter.Serialize(_collection.GetCollection());
            //if (_saveWasPressed)
            //{
            //    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DGM3/memes.xml", "");
            //}
            //_collectionCaretaker.Backup();
            //_saveWasPressed = true;
        }

        #region Search

        List<TreeViewItem> collapsedItems = new List<TreeViewItem>();

        string searchText = string.Empty;
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = SearchBox.Text;

            for (int i = 0; i < _collection.GetCollection().Count; i++)
            {
                if (!_collection.GetCollection()[i].Name.ToLower().Contains(searchText.ToLower()))
                {
                    (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Collapsed;
                }
                else
                    (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Visible;
            }
            
            {
            //if (string.IsNullOrEmpty(SearchBox.Text))
            //{
            //    return;
            //}

            //foreach (var collectionItem in _collection.GetCollection())
            //{
            //    if (!collectionItem.Name.ToLower().Contains(searchText.ToLower()))
            //    {


            //        foreach (TreeViewItem treeViewItem in CollectionView.Items)
            //        {
            //            var uiElement = treeViewItem as UIElement;

            //            if (uiElement.Uid == collectionItem.Id.ToString())
            //                treeViewItem.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //}
            }
        }

        string SelectedCategory = string.Empty;
        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategory = (sender as System.Windows.Controls.ComboBox).SelectedItem as string;

            for (int i = 0; i < _collection.GetCollection().Count; i++)
            {
                if (CategoriesComboBox.SelectedIndex == 0)
                {
                    DisplayAll();
                }
                else if (_collection.GetCollection()[i].Category.ToLower() != SelectedCategory.ToLower())
                {
                    (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Collapsed;
                    collapsedItems.Add(CollectionView.Items.GetItemAt(i) as TreeViewItem);
                }
                else
                    (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Visible;
            }

            {
                //CollectionView.Items.Clear();

                //if (CategoriesComboBox.SelectedIndex == 0)
                //{
                //    DisplayCurrentListInTreeView(); // ???????????????
                //    return;
                //}


                //PictureCollection sortedCollection = _enumerator.SortByCategory(SelectedCategory, _collection);

                //CollectionView.Items.MouseLeftButtonUp += Item_MouseLeftButtonUp;
                //List<TreeViewItem> collection = XmlDataEnumerator.GetItemsBy(comboBoxChosenItem, "Category", _collection.GetMemesCache());            
            }
            {
            //if (string.IsNullOrEmpty(SearchBox.Text))
            //{
            //    Download_Click(sender, e);
            //    return;
            //}

            //var categoriesList = new List<string>();
            //XmlDocument doc = new XmlDocument();
            //doc.Load("../../App_Data/memes.xml");
            //XmlNodeList elementList = doc.GetElementsByTagName("Category");
            //for (int i = 0; i < elementList.Count; i++)
            //{
            //    categoriesList.Add(elementList[i].InnerText.ToLower());
            //}

            //var match = new List<string>();
            
            //
            //foreach (var category in collection)
            //{
            //    if (category.Contains(comboBoxText.ToLower()))
            //    {
            //        var subItem = new TreeViewItem()
            //        {
            //            // Set header as file name
            //            Header = _MemesList[counter].Name,
            //            // And tag as full path
            //            Tag = _MemesList[counter].Uri
            //        };

            //        FolderView.Items.Add(subItem);

            //        subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            //    }
            //    counter++;
            //}

            //counter = 0;
            }
        }

        private TreeViewItem MakeTreeViewItem(Picture ReceivedPicture)
        {
            TreeViewItem treeViewItem = new TreeViewItem()
            {
                // Set header as file name
                Header = ReceivedPicture.Name,
                // And tag as full path
                Tag = ReceivedPicture.Path,
                Uid = ReceivedPicture.Id.ToString(),
            };
            return treeViewItem;
        }

        #endregion

        internal class Display
        {
            private PictureCollection _gallery = new PictureCollection(null);
            private CollectionEnumerator _enumerator = new CollectionEnumerator();

            public Display(PictureCollection memeCollection)
            {
                _gallery = memeCollection;
            }

            public PictureCollection ShowAll()
            {
                return _gallery;
            }

            public PictureCollection ShowByName(string Name)
            {
                PictureCollection filteredCollection = new PictureCollection(null);

                filteredCollection = _enumerator.SortByName(Name, _gallery);

                return filteredCollection;
            }

            
        }


    }
}