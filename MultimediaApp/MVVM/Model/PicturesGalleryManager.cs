using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultimediaApp.MVVM.Model
{
    internal sealed class PicturesGalleryManager
    {

        private XmlFormatter _xmlFormatter = new XmlFormatter();
        private Caretaker _collectionCaretaker;

        private ObservableCollection<Picture> _pictures;
        private ObservableCollection<string> _tags;

        private ObservableCollection<Picture> _filteredPictures;

        // mb Constructor

        // Methods
        public ObservableCollection<Picture> GetByName(string name)
        {

            return filteredCollection;
        }
        
        public ObservableCollection<Picture> GetByTag(string tag)
        {

            return filteredCollection;
        }
        
        public ObservableCollection<Picture> GetAll()
        {
            return _filteredPictures;
        }

        private void Add()
        {
            _collectionCaretaker.Backup();

            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files| *.jpg; *.jpeg; *.png;" };
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string filePath = openFileDialog.FileName;

            List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            if (!_imageExtensions.Contains(Path.GetExtension(filePath).ToUpperInvariant()))
            {
                System.Windows.MessageBox.Show("I don\'t get it..");
                return;
            }

            NamingWindow namingWindow = new NamingWindow();
            namingWindow.ShowDialog();

            // After adding pic obj throwed into IDK WHERE
            // Here will be EVENT that coll was updated
        }

        public void Remove(object obj)
        {
            _collectionCaretaker.Backup();

            if (obj is Picture pic)
            {
                _pictures.Remove(pic);
            }
        }

        public void SaveToXml()
        {
            _xmlFormatter.Serialize(_pictures);
        }

        public void Undo()
        {
            _collectionCaretaker.Undo();
        }

        private PicturesGalleryManager() { }
        private static PicturesGalleryManager _instance;
        public static PicturesGalleryManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PicturesGalleryManager();
            }
            return _instance;
        }

        #region Memento
        public IMemento Save()
        {
            return new CollectionMemento(_pictures);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is CollectionMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            _pictures.Clear();
            foreach (var item in memento.GetState())
            {
                _pictures.Add(item);
            }
        }
        #endregion


    }
}
