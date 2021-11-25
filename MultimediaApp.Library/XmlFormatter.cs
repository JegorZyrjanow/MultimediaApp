using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultimediaApp.Library
{
    public class XmlFormatter
    {
        private List<Picture> PicturesList;
        private XmlSerializer formatter = new XmlSerializer(typeof(List<Picture>));

        public void Deserialize()
        {
            //try
            //{
                using (FileStream fs = new FileStream("../../AppData/memes.xml", FileMode.OpenOrCreate))
                {
                    PicturesList = (List<Picture>)formatter.Deserialize(fs);
                }
            //}
            //catch { }
        }

        public void Serialize(List<Picture> pictures)
        {                        
            using (FileStream fileStream = new FileStream("../../AppData/memes.xml", FileMode.Create))
            {
                formatter.Serialize(fileStream, pictures);
            }
            

        }

        public List<Picture> GetCollection()
        {
            return PicturesList;
        }

        public void RecieveList(List<Picture> List)
        {
            PicturesList = List;
        }
    }
}
