using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MultimediaApp.MVVM.Model
{
    internal sealed class GalleryService : IGalleryService
    {
        private ObservableCollection<Picture> _pictures;
        private List<string> _tags;

        private ObservableCollection<Picture> _filteredPictures = new ObservableCollection<Picture>();

        private readonly XmlFormatter _xmlFormatter = new XmlFormatter();
        //private GalleryCaretaker _collectionCaretaker = new GalleryCaretaker(this);

        // mb Constructor
        public void Add()
        {
            //_collectionCaretaker.Backup();

            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files| *.jpg; *.jpeg; *.png;" };
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string filePath = openFileDialog.FileName;

            List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            if (!_imageExtensions.Contains(Path.GetExtension(filePath).ToUpperInvariant()))
                System.Windows.MessageBox.Show("I don\'t get it..");
            else new NamingWindow().ShowDialog();


            // After adding pic obj throwed into IDK WHERE TO
            // Here will be EVENT that coll was updated
        }

        public void Remove(int id)
        {
            if (_pictures == null)
                MessageBox.Show("Collection is empty.");
            // 
            //_collectionCaretaker.Backup();
            // 
            _pictures.RemoveAt(id);
        }

        public void Undo() // not used
        {
            if (_pictures == null)
            {
                MessageBox.Show("Collection is empty.");
            }
            //
            //_collectionCaretaker.Undo();
        }

        public void SaveToXml()
        {
            _xmlFormatter.Serialize(_pictures);
        }

        public ObservableCollection<Picture> GetByName(string name)
        {
            if (_pictures == null)
                MessageBox.Show("Collection is empty.");

            _filteredPictures = (ObservableCollection<Picture>)(from pic in _pictures where pic.Name == name select pic);

            return _filteredPictures;
        }

        public ObservableCollection<Picture> GetByTag(string tag)
        {
            if (_pictures == null)
                MessageBox.Show("Collection is empty.");

            _filteredPictures = (ObservableCollection<Picture>)(from pic in _pictures where pic.Tag == tag select pic);

            return _filteredPictures;
        }

        public ObservableCollection<Picture> GetAll()
        {
            if (_pictures == null)
                MessageBox.Show("Collection is empty.");

            foreach (var item in _pictures)
            {
                _filteredPictures.Add(item);
            }
            return _filteredPictures;
        }

        public List<string> GetCategories()
        {
            return _tags;
        }

        public void SetExistingCollectionFromXml()
        {
            _pictures = _xmlFormatter.Deserialize();
        }

        public void SetTags()
        {
            _tags = new List<string>();
            // Filter for unique categories
            _tags.AddRange(_pictures.Select(o => o.Tag).Distinct().ToList());
            // Filter for non-empty categories
            _tags = _tags.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        private GalleryService()
        {

        }
        private static GalleryService _instance;
        public static GalleryService GetInstance()
        {
            return _instance ?? (_instance = new GalleryService());
        }

        #region Memento
        public IMemento Save()
        {
            return new GalleryServiceMemento(_pictures);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is GalleryService))
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

    public interface IMemento
    {
        ObservableCollection<Picture> GetState();
    }

    class GalleryServiceMemento : IMemento
    {
        private readonly ObservableCollection<Picture> _state;

        public GalleryServiceMemento(ObservableCollection<Picture> state)
        {
            _state = state;
        }

        public ObservableCollection<Picture> GetState()
        {
            return _state;
        }
    }
}
