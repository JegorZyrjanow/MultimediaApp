using System.Collections.ObjectModel;
using MultimediaApp.Model;

namespace MultimediaApp.Data;

internal interface IXmlService
{
    ObservableCollection<Picture> Deserialize();
    void Serialize(ObservableCollection<Picture> pictures);
}
