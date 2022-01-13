using System;
using System.Collections.Generic;
using System.Windows;

namespace MultimediaApp
{
    [Serializable]
    public class PictureModel
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Path { get; set; }
        public int Id { get; set; }

        private PictureModel() { }

        internal PictureModel(string name, List<string> tags, string path)
        {
            Name = name;
            Tags = tags;
            Path = path;
        }
    }
}
