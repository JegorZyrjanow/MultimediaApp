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
        //private readonly ObservableCollection<PictureModel> _viewPics;
        private readonly ObservableCollection<PictureModel> _pictures;
        public ObservableCollection<PictureModel> Pictures { get { return _pictures; } }
        public PictureModel SelectedPicture { get; set; }

        //private readonly GalleryModel _gallery;
        //private readonly List<string> _tags;
        //private GalleryCaretaker _collectionCaretaker = new GalleryCaretaker(this);

        private readonly XmlService _xmlService = new XmlService();

        private GalleryService()
        {
            _pictures = ExtractPictures();
        }
        private static GalleryService _instance;
        public static GalleryService GetInstance()
        {
            return _instance ?? (_instance = new GalleryService());
        }

        private ObservableCollection<PictureModel> ExtractPictures()
        {
            return new ObservableCollection<PictureModel>(_xmlService.Deserialize());
        }

        public void Add(PictureModel pic)
        {
            //_collectionCaretaker.Backup();

            // DELEGATE ???

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



            //if (_viewPics == null)
            //    MessageBox.Show("Collection is empty.");
        }

        public void EditPicture(string newName, List<string> newTags)
        {
            SelectedPicture.Name = newName;
            SelectedPicture.Tags = newTags;
            _pictures.Add(new PictureModel("", null, "") { Id= -1, Name="", Path="", Tags=new List<string>(newTags)});
            _pictures.Remove(_pictures.First(p => p.Id == -1));
        }

        public void EditPicture(string newName)
        {
            SelectedPicture.Name = newName;
        }

        public void EditPicture(List<string> newTags)
        {
            SelectedPicture.Tags = newTags;
        }

        public void SaveToXml() => _xmlService.Serialize(_pictures);

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
            {
                //ObservableCollection<PictureModel> coll = new ObservableCollection<PictureModel>(/*from pic in _pictures where pic.Name == name select pic*/);

                //foreach (var pic in _pictures)
                //{
                //    if (pic.Name == name)
                //    {
                //        coll.Add(pic);
                //    }
                //}
            }
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

            ObservableCollection<PictureModel> result = new ObservableCollection<PictureModel>(from pic in _pictures where pic.Tags.Contains(tag) select pic); // contains
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
            //List<string> newList = s
            //foreach (var item in _pictures)
            //{
            //    List<string> cats = new List<string>(item.Tags).Distinct().ToList();
            //}
            //List<string> cats = new List<string>((from pic in _pictures select pic.Tags).Distinct().ToList());

            List<List<string>> treeOfCats = new List<List<string>>((from pic in _pictures select pic.Tags).Distinct().ToList());

            List<string> cats = new List<string>();
            cats = new List<string>(treeOfCats.SelectMany(x => x).Distinct().ToList());

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
        public IMemento Save() => new GalleryServiceMemento(_pictures);

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is GalleryServiceMemento))
                throw new Exception("Unknown memento class " + memento.ToString());

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
        public GalleryServiceMemento(ObservableCollection<PictureModel> state) => _state = state;
        public ObservableCollection<PictureModel> GetState() => _state;
    }

    #endregion

}
