using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp.MVVM.Model
{
    internal interface IXmlService
    {
        ObservableCollection<PictureModel> Deserialize();
        void Serialize(ObservableCollection<PictureModel> pictures);
    }
}
