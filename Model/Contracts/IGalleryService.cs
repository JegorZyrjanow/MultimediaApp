using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MultimediaApp.Model;

internal interface IGalleryService
{
    // Editing
    void Add(Picture pic);
    void Remove(int id);
    void Undo();
    // Getters
    ObservableCollection<Picture> GetPicturesByName(string name);
    ObservableCollection<Picture> GetPicturesByTag(string tag);
    ObservableCollection<Picture> GetAll();
    // Cats
    List<string> GetTags();
    // XML
    void SaveToXml();
    //void SetExistingCollectionFromXml();// used?
}
