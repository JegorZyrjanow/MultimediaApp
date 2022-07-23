using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using MultimediaApp.Data;
using ImageProcessingLib.Filters;

namespace MultimediaApp.Model;

internal sealed class GalleryService : IGalleryService
{
    public ObservableCollection<Picture> Pictures => _pictures;

    //private readonly ObservableCollection<PictureModel> _viewPics;
    //private readonly List<string> _tags;
    //private GalleryCaretaker _collectionCaretaker = new GalleryCaretaker(this);
    private readonly ObservableCollection<Picture> _pictures; // IN REPO NOW [context]
    private readonly ColorFilter _colorFilter;

    private readonly IGalleryRepo _repo;

    // - [ ] REG WITH DI AS SINGLETON
    private GalleryService(IGalleryRepo repo)
    {
        _repo = repo; // - [ ] ADD WITH DI
        _colorFilter = new();
        _pictures = ExtractPictures();
    }

    public void Add(Picture pic) => _repo.Add(pic); // Do I need It?

    public void Remove(int picId)
    {
        //_collectionCaretaker.Backup();

        if (_pictures == null)
        {
            MessageBox.Show("Collection is empty.");
            return;
        }
        else
            _pictures.Remove((from pic in _pictures where pic.Id == picId select pic).Single());
        //_pictures.RemoveAt(picId.GetValueOrDefault());
    }

    public void Undo() // NOT USED YET
    {
        //_collectionCaretaker.Undo();



        //if (_viewPics == null)
        //    MessageBox.Show("Collection is empty.");
    }

    public void SaveToXml() => _xmlService.Serialize(_pictures);

    public ObservableCollection<Picture> GetPicturesByName(string name)
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

        var result = new ObservableCollection<Picture>(from pic in _pictures where pic.Name.Contains(name) select pic);

        return result;
    }

    public ObservableCollection<Picture> GetPicturesByTag(string tag) // --
    {
        if (_pictures == null)
        {
            MessageBox.Show("Collection is empty.");
            return GetAll();
        }

        else if (tag == "Show all")
            return GetAll();

        var result = new ObservableCollection<Picture>(from pic in _pictures where pic.Tag == tag select pic);
        return result;
    }

    public ObservableCollection<Picture> GetAll() // --
    {
        if (_pictures == null)
            MessageBox.Show("Collection is empty.");

        return _pictures;

        //_pictures.CollectionChanged -= CollectionChangedMethod;               // ???
        //_ViewPics = new ObservableCollection<PictureModel>(_pictures);
        //_pictures.CollectionChanged += CollectionChangedMethod;            
        //return _ViewPics;                                                     // ???
    }

    //public List<string> GetTags()
    //{
    //}

    #region Filters

    public void ToBlackAndWhite()
    {
        _colorFilter.ToBlackAndWhite(null);
    }

    #endregion

    //public void SetExistingCollectionFromXml()
    //{
    //    _pictures = _xmlFormatter.Deserialize();
    //    //_pictures.CollectionChanged += CollectionChangedMethod;
    //}

    private ObservableCollection<Picture> ExtractPictures() => new(_xmlService.Deserialize());

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
    /// <summary>
    /// NOT USED
    /// </summary>
    /// <returns></returns>
    //public IMemento Save() => new GalleryServiceMemento(_pictures);

    //// Восстанавливает состояние Создателя из объекта снимка.
    //public void Restore(IMemento memento)
    //{
    //    if (memento is not GalleryServiceMemento)
    //        throw new Exception("Unknown memento class " + memento.ToString());

    //    _pictures.Clear();
    //    foreach (var item in memento.GetState())
    //    {
    //        _pictures.Add(item);
    //    }
    //}


}

//public interface IMemento
//{
//    ObservableCollection<PictureModel> GetState();
//}
//class GalleryServiceMemento : IMemento
//{
//    private readonly ObservableCollection<PictureModel> _state;
//    public GalleryServiceMemento(ObservableCollection<PictureModel> state) => _state = state;
//    public ObservableCollection<PictureModel> GetState() => _state;        
//}

#endregion

