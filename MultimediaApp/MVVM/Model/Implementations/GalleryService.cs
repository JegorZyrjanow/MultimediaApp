using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace MultimediaApp.MVVM.Model
{
    internal sealed class GalleryService : IGalleryService
    {
        private readonly ObservableCollection<PictureModel> _ViewPics;
        private readonly ObservableCollection<PictureModel> _pictures;
        public ObservableCollection<PictureModel> Pictures { get { return _pictures; } }

        //private readonly GalleryModel _gallery;
        //private readonly List<string> _tags;
        //private GalleryCaretaker _collectionCaretaker = new GalleryCaretaker(this);

        private readonly XmlService _xmlFormatter = new XmlService();

        private GalleryService()
        {
            _pictures = ExtractPictures();

            _ViewPics = new ObservableCollection<PictureModel>(_pictures);

            // ==========================

            //_gallery = new GalleryModel();
            //_pictures = _gallery.GetPics();
            //_pictures.CollectionChanged += CollectionChangedMethod;
        }
        private static GalleryService _instance;
        public static GalleryService GetInstance()
        {
            return _instance ?? (_instance = new GalleryService());
        }

        private ObservableCollection<PictureModel> ExtractPictures()
        {
            return new ObservableCollection<PictureModel>(_xmlFormatter.Deserialize());
        }

        public void Add(PictureModel pic)
        {
            //_collectionCaretaker.Backup();

            // DELEGATE

            _pictures.Add(pic);
        }

        public void Remove(int? picId)
        {
            //_collectionCaretaker.Backup();

            if (_pictures == null)
                MessageBox.Show("Collection is empty.");
            else if (picId == null)
                return;
            else
                _pictures.Remove((from pic in _pictures where pic.Id == picId.GetValueOrDefault() select pic).Single());
            //_pictures.RemoveAt(picId.GetValueOrDefault());
        }

        public void Undo() // NOT USED YET
        {
            //_collectionCaretaker.Undo();

            if (_ViewPics == null)
            {
                MessageBox.Show("Collection is empty.");
            }
        }

        public void SaveToXml() // NOT USED YET
        {
            //_gallery.Serialize(_pictures);
        }

        public ObservableCollection<PictureModel> GetPicturesByName(string name)
        {
            if (_pictures == null)
            {
                MessageBox.Show("Collection is empty.");
                return GetAll();
            }

            if (string.IsNullOrEmpty(name))
            {
                return GetAll();
            }

            ObservableCollection<PictureModel> result = new ObservableCollection<PictureModel>(from pic in _pictures where pic.Name.Contains(name) select pic);

            return result;

            //ObservableCollection<PictureModel> coll = new ObservableCollection<PictureModel>(/*from pic in _pictures where pic.Name == name select pic*/);

            //foreach (var pic in _pictures)
            //{
            //    if (pic.Name == name)
            //    {
            //        coll.Add(pic);
            //    }
            //}
        }

        public ObservableCollection<PictureModel> GetPicturesByTag(string tag) // --
        {
            if (_pictures == null)
            {
                MessageBox.Show("Collection is empty.");
                return GetAll();
            }

            else if (tag == "Show all")
                return GetAll();

            ObservableCollection<PictureModel> result = new ObservableCollection<PictureModel>(from pic in _pictures where pic.Tag == tag select pic);
            return result;
        }

        public ObservableCollection<PictureModel> GetAll() // --
        {
            if (_pictures == null)
                MessageBox.Show("Collection is empty.");

            return _pictures;

            //_pictures.CollectionChanged -= CollectionChangedMethod;               // ???
            //_ViewPics = new ObservableCollection<PictureModel>(_pictures);
            //_pictures.CollectionChanged += CollectionChangedMethod;            
            //return _ViewPics;                                                     // ???
        }

        public List<string> GetTags()
        {
            List<string> cats = new List<string>((from pic in _pictures select pic.Tag).Distinct().ToList());
            cats = cats.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
            return cats; // (from pic in _pictures select pic.Tag).Distinct().ToList()
        }

        //public void SetExistingCollectionFromXml()
        //{
        //    _pictures = _xmlFormatter.Deserialize();
        //    //_pictures.CollectionChanged += CollectionChangedMethod;
        //}

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e) // METHOD IS TO ACCEPT CHANGES IN VIEWPICS TO MAIN COLLECTION
        {
            // Different kind of changes that may have occurred in collection            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //_pictures.Add(_editFieldPics.Last());
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                // ___
            }
            if (e.Action == NotifyCollectionChangedAction.Remove) // --?
            {
                //var filteredIds = _ViewPics.Select(p => p.Id);
                //var MainIds = _pictures.Select(p => p.Id);
                //var difference = filteredIds.Except(MainIds);
                //foreach (var id in difference)
                //{
                //    _pictures.RemoveAll(item => item.Id == id);
                //    _pictures.Remove(_pictures.Where(i => i.Id == id).Single());
                //}
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

    #endregion


}
