using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MultimediaApp.MVVM.Model
{
    internal class GalleryModel
    {
        private readonly XmlService _xmlFormatter = new XmlService();

        private readonly ObservableCollection<PictureModel> _pictures;
        private readonly List<string> _tags;

        public GalleryModel()
        {
            // Deserialize pictures
            _pictures = _xmlFormatter.Deserialize();
            // Set tags
            _tags = new List<string>();
            // Filter for unique categories
            _tags.AddRange(_pictures.Select(o => o.Tag).Distinct().ToList());
            // Filter for non-empty categories
            _tags = _tags.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        public ObservableCollection<PictureModel> GetPics()
        {
            return _pictures;
        }

        public List<string> GetTags()
        {
            return _tags;
        }

        public void SetChanges()
        {
            // ???
        }

        public void AddPicture(PictureModel pic)
        {
            _pictures.Add(pic);
        }

        //public void CollectionChanged()
        //{

        //}

        public ObservableCollection<PictureModel> GetPictures()
        {
            return _pictures;
        }
    }
}
