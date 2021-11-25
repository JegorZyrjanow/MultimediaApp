using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MultimediaApp.Library;

namespace MultimediaApp
{
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Picture> Collection;

        private XmlFormatter _xmlFormatter = new XmlFormatter();
        private PictureCollection _collection;
        private Caretaker _collectionCaretaker;
        private List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public ApplicationViewModel()
        {
            _xmlFormatter.Deserialize();
            _collection = new PictureCollection(_xmlFormatter.GetCollection());

            Collection = _collection.GetCollection();
        }

        public void Init()
        {
            _collection.GetUniqueCategories().ForEach(category => CategoriesComboBox.Items.Add(category));


            _collectionCaretaker = new Caretaker(_collection);
            // Add Common Category at 0th index
            CategoriesComboBox.Items.Insert(0, "All");
            // Set Default ComboBox Item
            CategoriesComboBox.SelectedItem = 0;
            CategoriesComboBox.SelectedIndex = 0;

            _collectionCaretaker.Backup();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
