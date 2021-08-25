using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp
{
    [Serializable]
    public class Meme
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Uri { get; set; }
    }
}
