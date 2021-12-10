using System;
using System.Windows;

namespace MultimediaApp
{
    [Serializable]
    public class Picture
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Path { get; set; }
        public int Id { get; set; }

        private Picture() { }

        internal Picture(string name, string category, string path)
        {
            Name = name;
            Tag = category;
            Path = path;
        }
    }
}
