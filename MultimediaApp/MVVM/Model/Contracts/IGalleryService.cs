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
        ObservableCollection<PictureModel> GetByName(string name);
        ObservableCollection<PictureModel> GetByTag(string tag);
        ObservableCollection<PictureModel> GetAll();
        // Cats
        List<string> GetCategories();
        // XML
        void SaveToXml();
        //void SetExistingCollectionFromXml();// used?
    }
}
