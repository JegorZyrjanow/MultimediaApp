using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MultimediaApp.MVVM.Model
{
    internal interface IGalleryService
    {
        // Editing
        void Add(PictureModel pic);
        void Remove(int? id);
        void Undo(); // NOT USED
        void EditPicture(string newName);
        void EditPicture(string newName, List<string> newTags);
        void EditPicture(List<string> newTags);
        // Getters
        ObservableCollection<PictureModel> GetPicturesByName(string name);
        ObservableCollection<PictureModel> GetPicturesByTag(string tag); // --
        ObservableCollection<PictureModel> GetAll();
        // Cats
        List<string> GetTags();
        // XML
        void SaveToXml();
        //void SetExistingCollectionFromXml();// used?
    }
}
