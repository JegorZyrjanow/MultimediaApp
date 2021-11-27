using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace MultimediaApp
{
    public class XmlFormatter
    {
        private ObservableCollection<Picture> PicturesList;
        private XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Picture>));

        public void Deserialize()
        {
            using (FileStream fs = new FileStream("../../AppData/memes.xml", FileMode.OpenOrCreate))
            {
                PicturesList = (ObservableCollection<Picture>)formatter.Deserialize(fs);
            }
        }

        public void Serialize(ObservableCollection<Picture> pictures)
        {
            using (FileStream fileStream = new FileStream("../../AppData/memes.xml", FileMode.Create))
            {
                formatter.Serialize(fileStream, pictures);
            }
        }

        public ObservableCollection<Picture> GetCollection()
        {
            return PicturesList;
        }

        //public void RecieveList(ObservableCollection<Picture> List)
        //{
        //    PicturesList = List;
        //}
    }
}
