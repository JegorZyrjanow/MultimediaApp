using MultimediaApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace MultimediaApp
{
    public class XmlService : IXmlService
    {
        private ObservableCollection<PictureModel> _picturesList;
        private readonly XmlSerializer _formatter = new XmlSerializer(typeof(ObservableCollection<PictureModel>));

        public ObservableCollection<PictureModel> Deserialize()
        {
            using (FileStream fs = new FileStream("../../AppData/memes.xml", FileMode.OpenOrCreate))
            {
                _picturesList = (ObservableCollection<PictureModel>)_formatter.Deserialize(fs);
            }

            return _picturesList;
        }

        public void Serialize(ObservableCollection<PictureModel> pictures)
        {
            using (FileStream fileStream = new FileStream("../../AppData/memes.xml", FileMode.Create))
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
}
