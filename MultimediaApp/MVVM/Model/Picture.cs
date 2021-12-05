using System;
using System.Windows;

namespace MultimediaApp
{
    [Serializable]
    public class Picture
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Path { get; set; }
        //public int Id { get; set; }

        internal Picture() { }

        internal Picture(string name, string category, string path)
        {
            Name = name;
            Category = category;
            Path = path;
        }
    }
}
