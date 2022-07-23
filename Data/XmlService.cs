using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using MultimediaApp.Model;

namespace MultimediaApp.Data;

public class XmlService : IXmlService
{
    private ObservableCollection<Picture> _picturesList;
    private readonly XmlSerializer _formatter = new(typeof(ObservableCollection<Picture>));

    public ObservableCollection<Picture> Deserialize()
    {
        using (FileStream fs = new("../../../AppData/memes.xml", FileMode.OpenOrCreate))
        {
            _picturesList = (ObservableCollection<Picture>)_formatter.Deserialize(fs);
        }

        return _picturesList;
    }

    public void Serialize(ObservableCollection<Picture> pictures)
    {
        using FileStream fileStream = new("../../../AppData/memes.xml", FileMode.Create);
        {
            _formatter.Serialize(fileStream, pictures);
        }
    }

    //public ObservableCollection<Picture> GetCollection()
    //{
    //    return _picturesList;
    //}

    //public void RecieveList(ObservableCollection<Picture> List)
    //{
    //    PicturesList = List;
    //}
}
