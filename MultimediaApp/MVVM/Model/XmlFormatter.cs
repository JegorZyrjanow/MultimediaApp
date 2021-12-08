using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace MultimediaApp
{
    public class XmlFormatter
    {
        private ObservableCollection<Picture> _picturesList;
        private readonly XmlSerializer _formatter = new XmlSerializer(typeof(ObservableCollection<Picture>));

        public void Deserialize()
        {
            using (FileStream fs = new FileStream("../../AppData/memes.xml", FileMode.OpenOrCreate))
            {
                _picturesList = (ObservableCollection<Picture>)_formatter.Deserialize(fs);
            }
        }

        public void Serialize(ObservableCollection<Picture> pictures)
        {
            using (FileStream fileStream = new FileStream("../../AppData/memes.xml", FileMode.Create))
            {
                _formatter.Serialize(fileStream, pictures);
            }
        }

        public ObservableCollection<Picture> GetCollection()
        {
            return _picturesList;
        }

        //public void RecieveList(ObservableCollection<Picture> List)
        //{
        //    PicturesList = List;
        //}
    }
}
