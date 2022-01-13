using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region Search

        string searchText = string.Empty;
        //private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    searchText = SearchBox.Text;

        //    for (int i = 0; i < _collection.GetCollection().Count; i++)
        //    {
        //        if (!_collection.GetCollection()[i].Name.ToLower().Contains(searchText.ToLower()))
        //        {
        //            (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Collapsed;
        //        }
        //        else
        //            (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Visible;
        //    }

        //    {
        //    //if (string.IsNullOrEmpty(SearchBox.Text))
        //    //{
        //    //    return;
        //    //}

        //    //foreach (var collectionItem in _collection.GetCollection())
        //    //{
        //    //    if (!collectionItem.Name.ToLower().Contains(searchText.ToLower()))
        //    //    {


        //    //        foreach (TreeViewItem treeViewItem in CollectionView.Items)
        //    //        {
        //    //            var uiElement = treeViewItem as UIElement;

        //    //            if (uiElement.Uid == collectionItem.Id.ToString())
        //    //                treeViewItem.Visibility = Visibility.Collapsed;
        //    //        }
        //    //    }
        //    //}
        //    }
        //}

        //string SelectedCategory = string.Empty;
        //private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SelectedCategory = (sender as System.Windows.Controls.ComboBox).SelectedItem as string;

        //    for (int i = 0; i < _collection.GetCollection().Count; i++)
        //    {
        //        if (CategoriesComboBox.SelectedIndex == 0)
        //        {
        //            DisplayAll();
        //        }
        //        else if (_collection.GetCollection()[i].Category.ToLower() != SelectedCategory.ToLower())
        //        {
        //            (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Collapsed;
        //            collapsedItems.Add(CollectionView.Items.GetItemAt(i) as TreeViewItem);
        //        }
        //        else
        //            (CollectionView.Items.GetItemAt(i) as TreeViewItem).Visibility = Visibility.Visible;
        //    }

        //    {
        //        //CollectionView.Items.Clear();

        //        //if (CategoriesComboBox.SelectedIndex == 0)
        //        //{
        //        //    DisplayCurrentListInTreeView(); // ???????????????
        //        //    return;
        //        //}


        //        //PictureCollection sortedCollection = _enumerator.SortByCategory(SelectedCategory, _collection);

        //        //CollectionView.Items.MouseLeftButtonUp += Item_MouseLeftButtonUp;
        //        //List<TreeViewItem> collection = XmlDataEnumerator.GetItemsBy(comboBoxChosenItem, "Category", _collection.GetMemesCache());            
        //    }
        //    {
        //    //if (string.IsNullOrEmpty(SearchBox.Text))
        //    //{
        //    //    Download_Click(sender, e);
        //    //    return;
        //    //}

        //    //var categoriesList = new List<string>();
        //    //XmlDocument doc = new XmlDocument();
        //    //doc.Load("../../App_Data/memes.xml");
        //    //XmlNodeList elementList = doc.GetElementsByTagName("Category");
        //    //for (int i = 0; i < elementList.Count; i++)
        //    //{
        //    //    categoriesList.Add(elementList[i].InnerText.ToLower());
        //    //}

        //    //var match = new List<string>();

        //    //
        //    //foreach (var category in collection)
        //    //{
        //    //    if (category.Contains(comboBoxText.ToLower()))
        //    //    {
        //    //        var subItem = new TreeViewItem()
        //    //        {
        //    //            // Set header as file name
        //    //            Header = _MemesList[counter].Name,
        //    //            // And tag as full path
        //    //            Tag = _MemesList[counter].Uri
        //    //        };

        //    //        FolderView.Items.Add(subItem);

        //    //        subItem.MouseLeftButtonUp += Item_MouseLeftButtonUp;
        //    //    }
        //    //    counter++;
        //    //}

        //    //counter = 0;
        //    }
        //}

        #endregion
    }


}