using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp.Library
{
    public interface IMemento
    {
        string GetName();

        Tuple<List<Picture>, List<string>> GetState();

        DateTime GetDate();
    }
}
