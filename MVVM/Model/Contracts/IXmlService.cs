using System.Collections.ObjectModel;

namespace MultimediaApp.MVVM.Model
{
    internal interface IXmlService
    {
        ObservableCollection<PictureModel> Deserialize();
        void Serialize(ObservableCollection<PictureModel> pictures);
    }
}
