using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MultimediaApp.MVVM.Model
{
    internal sealed class GalleryService : IGalleryService
    {
        private readonly GalleryModel _gallery;
        private readonly ObservableCollection<PictureModel> _pictures;
        private readonly List<string> _tags;
        
        private ObservableCollection<PictureModel> _editFieldPics;
        public ObservableCollection<PictureModel> Pictures { get { return _editFieldPics; } }

        //private GalleryCaretaker _collectionCaretaker = new GalleryCaretaker(this);

        #region Constructor

        private GalleryService()
        {
            _gallery = new GalleryModel();
            _pictures = _gallery.GetPics();

            _pictures.CollectionChanged += CollectionChangedMethod;
            _editFieldPics = new ObservableCollection<PictureModel>(_pictures);
        }
        private static GalleryService _instance;
        public static GalleryService GetInstance()
        {
            return _instance ?? (_instance = new GalleryService());
        }

        #endregion

        public void Add(PictureModel pic)
        {
            // DELEGATE
            
            _gallery.AddPicture(pic);

            //_collectionCaretaker.Backup();            
            // After adding pic obj throwed into IDK WHERE TO
            // Here will be EVENT that coll was updated
        }

        public void Remove(int? picId)
        {
            //_collectionCaretaker.Backup();
            
            if (_editFieldPics == null)
                MessageBox.Show("Collection is empty.");
            else if (picId == null)
                return;
            else
                _editFieldPics.RemoveAt(picId.GetValueOrDefault());
        }

        public void Undo() // not used
        {
            if (_editFieldPics == null)
            {
                MessageBox.Show("Collection is empty.");
            }
            //
            //_collectionCaretaker.Undo();
        }

        public void SaveToXml()
        {
            //_gallery.Serialize(_pictures);
        }

        public ObservableCollection<PictureModel> GetByName(string name)
        {
            if (_editFieldPics == null)
                MessageBox.Show("Collection is empty.");

            _editFieldPics = (ObservableCollection<PictureModel>)(from pic in _pictures where pic.Name == name select pic);

            return _editFieldPics;
        }

        public ObservableCollection<PictureModel> GetByTag(string tag) // --
        {
            if (_editFieldPics == null)
                MessageBox.Show("Collection is empty.");
            IEnumerable<PictureModel> filteredPics = from pic in _pictures where pic.Tag == tag select pic;
            _editFieldPics = (ObservableCollection<PictureModel>)filteredPics;

            return _editFieldPics;
        }

        public ObservableCollection<PictureModel> GetAll() // --
        {
            if (_editFieldPics == null)
                MessageBox.Show("Collection is empty.");

            _editFieldPics.CollectionChanged -= CollectionChangedMethod;

            _editFieldPics = new ObservableCollection<PictureModel>(_pictures);

            _editFieldPics.CollectionChanged += CollectionChangedMethod;
            
            return _editFieldPics;
        }

        public List<string> GetCategories()
        {
            return _gallery.GetTags();
        } // ???

        //public void SetExistingCollectionFromXml()
        //{
        //    _pictures = _xmlFormatter.Deserialize();
        //    //_pictures.CollectionChanged += CollectionChangedMethod;
        //}

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Different kind of changes that may have occurred in collection            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _editFieldPics.Add(_editFieldPics.Last());
            }    
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                // ___
            }
            if (e.Action == NotifyCollectionChangedAction.Remove) // --?
            {
                var filteredIds = _editFieldPics.Select(p => p.Id);
                var MainIds = _pictures.Select(p => p.Id);
                var difference = filteredIds.Except(MainIds);
                foreach (var id in difference)
                {
                    _pictures.RemoveAt(id);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                
            }
        }

        #region Memento
        public IMemento Save()
        {
            return new GalleryServiceMemento(_pictures);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is GalleryServiceMemento))
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
        ObservableCollection<PictureModel> GetState();
    }

    class GalleryServiceMemento : IMemento
    {
        private readonly ObservableCollection<PictureModel> _state;

        public GalleryServiceMemento(ObservableCollection<PictureModel> state)
        {
            _state = state;
        }

        public ObservableCollection<PictureModel> GetState()
        {
            return _state;
        }
    }
}
