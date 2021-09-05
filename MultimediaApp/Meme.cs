using System;

namespace MultimediaApp
{
    [Serializable]
    public class Meme
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string Uri { get; set; }
    }
}
