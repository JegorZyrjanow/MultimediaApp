using System;

namespace MultimediaApp.Library
{
    [Serializable]
    public class Picture
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
