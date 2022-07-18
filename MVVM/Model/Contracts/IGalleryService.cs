using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MultimediaApp.MVVM.Model
{
    internal interface IGalleryService
    {
        // Editing
        void Add(PictureModel pic);
        void Remove(int? id);
        void Undo();
        // Getters
        ObservableCollection<PictureModel> GetPicturesByName(string name);
        ObservableCollection<PictureModel> GetPicturesByTag(string tag);
        ObservableCollection<PictureModel> GetAll();
        // Cats
        List<string> GetTags();
        // XML
        void SaveToXml();
        //void SetExistingCollectionFromXml();// used?
    }
}
