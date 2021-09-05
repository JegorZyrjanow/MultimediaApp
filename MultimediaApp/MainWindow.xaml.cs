using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

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

        // Create the main memes list
        private List<Meme> _MemesList = new List<Meme>();

        // Create Cache list
        private List<Meme> _MemesListCache = new List<Meme>();

        // Create Categories list
        private List<string> _Categories= new List<string>();

        // Create pic parameters containers ??
        private string _pictureName;
        private string _picturePath;

        private int _chosenItemId;

        // Checkers
        private bool itemWasChosen = false;

        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Load Current List

            // Get List of Memes from XML file
            XmlSerializer formatter = new XmlSerializer(typeof(List<Meme>));
            try
            {
                using (FileStream fs = new FileStream("../../App_Data/memes.xml", FileMode.OpenOrCreate))
                {
                    _MemesList = (List<Meme>)formatter.Deserialize(fs);
                }
            }
            catch { }

            // Display current list in the TreeView
            DisplayTreeView();

            #endregion

            // Duplicate it
            _MemesListCache = _MemesList;

            #region Initialize Categories ()

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
            // Create Categories List
            _Categories = GetCollectionByTag("Category");

            // Add common category at 0th index
            CategoriesComboBox.Items.Insert(0, "All");

            // Keep only unique Categories
            var uniqueCategoriesList = _Categories.Distinct().ToList();
            uniqueCategoriesList.ForEach(category => CategoriesComboBox.Items.Add(category));

            #endregion

        }

        #endregion

        #region Display TreeView

        private void DisplayTreeView()
        {
            // For each file...
            _MemesList.ForEach(meme =>
            {
                // Create file item
                var subItem = new TreeViewItem()
                {
                    // Set header as file name
                    Header = meme.Name,
                    // And tag as full path
                    Tag = meme.Uri
                };

                // Add this item to the parent
                FolderView.Items.Add(subItem);

                subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            });
        }

        #endregion

        #region Display Image

        private void DisplayImageClickReaction(TreeViewItem item)
        {
            var fullPath = (string)item.Tag;

            List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            if (imageExtensions.Contains(Path.GetExtension(fullPath).ToUpperInvariant()))
            {
                ImageBox.Source = new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
            }
            else
            {
                return;
            }

            // Set checker true to delete method
            itemWasChosen = true;
        }

        #endregion

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

        #region Select Picture Actions

        /// <summary>
        /// Show selected picture in the ImageBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) // id сделай чтобы вместо ссылки получал...
        {
            #region Get ID
            {
                // Get XmlDox

                //var titlesList = new List<string>();

                //XmlDocument doc = new XmlDocument();
                //doc.Load("../../App_Data/memes.xml");
                //XmlNodeList elementList = doc.GetElementsByTagName("Name");
                //for (int i = 0; i < elementList.Count; i++)
                //{
                //    titlesList.Add(elementList[i].InnerText.ToLower());
                //}
            }
            // Get the name of chosen meme
            var item = (TreeViewItem)sender;
            var nameOfChosenItem = (string)item.Header;

            //FolderView.Items.Clear();
            // Counter for id
            var counter = 0;
            foreach (var titles in GetCollectionByTag("Name"))
            {
                // If there's match with a name in the list . . .
                if (titles.Contains(nameOfChosenItem.ToLower()))
                {
                    // get id of matched element
                    _chosenItemId = counter;
                    {
                        //id.Add(counter);

                        //sovp.Add(titles);
                        //var subItem = new TreeViewItem()
                        //{
                        //    // Set header as file name
                        //    Header = memes[counter].Name,
                        //    // And tag as full path
                        //    Tag = memes[counter].Uri
                        //};

                        //// Add this item to the parent
                        //FolderView.Items.Add(subItem);

                        //subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
                    }
                }
                {
                    //else
                    //{
                    //    id.Add(0);
                    //}
                    //else
                    //{
                    //    FolderView.Items.Add(null);
                    //}
                }
                counter++;
            }
            counter = 0; //??

            #endregion

            #region Display Image ( separate method )

            DisplayImageClickReaction(item);

            {
                //var fullPath = (string)item.Tag;

                //List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

                //if (imageExtensions.Contains(Path.GetExtension(fullPath).ToUpperInvariant()))
                //{
                //    ImageBox.Source = new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
                //}
                //else return;

                //// Set checker true for delete method
                //itemWasChosen = true;
            }

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

        /// <summary>
        /// Find the list of inner texts by tagname from the XML file
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private List<string> GetCollectionByTag(string tagName)
        {
            List<string> list = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load("../../App_Data/memes.xml");
            XmlNodeList elementList = doc.GetElementsByTagName(tagName);
            for (int i = 0; i < elementList.Count; i++)
            {
                list.Add(elementList[i].InnerText.ToLower());
            }
            return list;
        }

        #endregion

        #region XML

        #region Add Picture ( or not picture btw )

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
            _pictureName = namingWindow.FileName;
            _picturePath = namingWindow.FilePath;

            // Add new pic in the list of memes
            _MemesListCache.Add(new Meme
            {
                Name = _pictureName,
                Category = "0",
                Uri = _picturePath
            });

            FolderView.Items.Clear();
            DisplayTreeView();
            {//File.Copy(fileName, @"C:\Users\User\Desktop\MultimediaApp\MultimediaApp\images\memes\" + GetFileFolderName(fileName));
            }// Copy selected file into common images directory
        }

        #endregion

        #region Delete Picture

        /// <summary>
        /// Delete the selected picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePicture_Click(object sender, RoutedEventArgs e)
        {
            if (!itemWasChosen)
                return;
            else
            {
                // Удаляем выбранный мем из списка и убираем его из дерева
                var selected = FolderView.Items.IndexOf(FolderView.SelectedItem);

                //memes.IndexOf
                //try
                //{
                _MemesListCache.RemoveAt(_chosenItemId);
                //}
                //catch { }
                _chosenItemId = -1;

                FolderView.Items.RemoveAt(selected);

                itemWasChosen = false;

                ImageBox.Source = null;
            }
        }

        #endregion

        #region Download The List

        /// <summary>
        /// Download memes list from XML file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Download_Click(object sender, RoutedEventArgs e)
        {
            CategoriesComboBox.SelectedItem = 0;
            CategoriesComboBox.SelectedIndex = 0;

            XmlSerializer formatter = new XmlSerializer(typeof(List<Meme>));

            try
            {
                using (FileStream fs = new FileStream("../../App_Data/memes.xml", FileMode.OpenOrCreate))
                {
                    _MemesList = (List<Meme>)formatter.Deserialize(fs);
                }
            }
            catch { }

            _MemesListCache = _MemesList;

            FolderView.Items.Clear();
            DisplayTreeView();            
            {
                //// For each file...
                //memes.ForEach(meme =>
                //{
                //    // Create file item
                //    var subItem = new TreeViewItem()
                //    {
                //        // Set header as file name
                //        Header = GetFileFolderName(meme.Uri),
                //        // And tag as full path
                //        Tag = meme.Uri
                //    };

                //    // Add this item to the parent
                //    FolderView.Items.Add(subItem);

                //    subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
                //});
            }// moved out
            SearchBox.Text = null;
        }

        #endregion

        #region Save Current List

        bool saveWasPressed = false;
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (saveWasPressed)
            {
                File.WriteAllText("../../App_Data/memes.xml", "");
            }

            _MemesList = _MemesListCache;

            XmlSerializer formatter = new XmlSerializer(typeof(List<Meme>));
            using (FileStream fileStream = new FileStream("../../App_Data/memes.xml", FileMode.Create))
            {
                formatter.Serialize(fileStream, _MemesList);
            }
            saveWasPressed = true;
        }

        #endregion

        #endregion

        #region Search by Name

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                //var text = SearchBox.Text;

                //string fileName = "c:\\users\\xxxxx\\documents\\visual studio 2010\\Projects\\WpfApplication2\\WpfApplication2\\XML.xml";

                //var doc = XDocument.Load(fileName);
                //var findString = "text";

                //var results = doc.Element("Servernames").Descendants("Servername").Where(d => d.Value.Contains(findString)).Select(d => d.Value);

                ////getName = namingWindow.FileName;
                ////getPath = namingWindow.FilePath;

                //// Add new pic in the list of memes
                //memes.Add(new Meme
                //{
                //    Name = getName,
                //    Category = "0",
                //    Uri = getPath
                //});

                //DisplayTreeView();
                //var titlesList = new List<string>();

                ////Create the XmlDocument.
                //XmlDocument doc = new XmlDocument();

                //doc.Load("../../App_Data/memes.xml");
                ////Display all the book titles.
                //XmlNodeList elemList = doc.GetElementsByTagName("Name");
                //for (int i = 0; i < elemList.Count; i++)
                //{
                //    titlesList.Add(elemList[i].InnerText);
                //}

                //foreach (var titles in titlesList)
                //{
                //    if (titles.StartsWith(SearchBox.Text))
                //    {
                //        FolderView.Items.Clear();
                //        //FolderView.Items.Add(memes[]);
                //        FolderView.Items.Add(memes[titlesList.FindIndex(a => a.Contains(SearchBox.Text))]);
                //    }

                //}

                //XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            }
            {
                //var titlesList = new List<string>();

                //XmlDocument doc = new XmlDocument();
                //doc.Load("../../App_Data/memes.xml");
                //XmlNodeList elementList = doc.GetElementsByTagName("Name");
                //for (int i = 0; i < elementList.Count; i++)
                //{
                //    titlesList.Add(elementList[i].InnerText.ToLower());
                //}

                //var match = new List<string>();
            }
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                Download_Click(sender, e);
                return;
            }

            FolderView.Items.Clear();

            int counter = 0;
            foreach (var titles in GetCollectionByTag("Name"))
            {
                if (titles.Contains(SearchBox.Text.ToLower()))
                {//match.Add(titles);                    
                    var subItem = new TreeViewItem()
                    {
                        // Set header as file name
                        Header = _MemesList[counter].Name,
                        // And tag as full path
                        Tag = _MemesList[counter].Uri
                    };
                    // Add this item to the parent
                    FolderView.Items.Add(subItem);
                    {
                    //FolderView.Items.Clear();
                    //FolderView.Items.Add(memes[titles.IndexOf(SearchBox.Text)]);
                    //FolderView.Items.Add(memes[titlesList.FindIndex(a => a.Contains(SearchBox.Text))]);
                    }
                    subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
                }
                counter++;
            }
            counter = 0;
            {
                //XmlSerializer formatter = new XmlSerializer(typeof(List<string>));


                //using (FileStream fileStream = new FileStream("../../App_Data/memeslist.xml", FileMode.Create))
                //{
                //    formatter.Serialize(fileStream, titlesList);
                //}
            }
        }

        #endregion

        #region Filter by category ()

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedIndex == 0)
            {
                Download_Click(sender, e);
                return;
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
            }
            FolderView.Items.Clear();

            int counter = 0;
            var collection = GetCollectionByTag("Category");
            string comboBoxText = (sender as System.Windows.Controls.ComboBox).SelectedItem as string;
            foreach (var category in collection)
            {
                if (category.Contains(comboBoxText.ToLower()))
                {
                    var subItem = new TreeViewItem()
                    {
                        // Set header as file name
                        Header = _MemesList[counter].Name,
                        // And tag as full path
                        Tag = _MemesList[counter].Uri
                    };

                    FolderView.Items.Add(subItem);

                    subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
                }
                counter++;
            }
            //counter = 0;
        }

        #endregion

    }
}