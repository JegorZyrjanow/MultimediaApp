using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp.Library
{
    public interface IMemento
    {
        string GetName();

        ObservableCollection<Picture> GetState();

        DateTime GetDate();
    }
}
