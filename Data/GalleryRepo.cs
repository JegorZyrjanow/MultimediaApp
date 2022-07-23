using MultimediaApp.Model;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MultimediaApp.Data
{
    internal class GalleryRepo : IGalleryRepo
    {
        private readonly ObservableCollection<Picture> _context;
        private readonly IXmlService _xmlService;

        // Replace ObsCollection with Dbcontext or custom one
        public GalleryRepo(ObservableCollection<Picture> context, IXmlService xmlService)
        {
            _context = context;
            _xmlService = xmlService;
        }

        public Picture GetAll(string name)
        {
            throw new NotImplementedException();
        }

        public Picture GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Move this logic to separated Object
        //public Picture GetByName(string name)
        //{
        //    if (_context == null)
        //    {
        //        throw new Exception("Collection is empty.");
        //    }

        //    // Move this logic to View
        //    // if (string.IsNullOrEmpty(name))
        //    //    return GetAll();

        //    var result = new ObservableCollection<Picture>(
        //        from item in _context where item.Name.Contains(name) select item);

        //    return result;
        //}

        public List<string> GetTags()
        {
            return new List<string>((from item in _context select item.Tag)
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct()
                .ToList());
        }

        public void Add(Picture pic)
        {
            _context.Add(pic);
        }

        public void Remove(int id)
        {
            if (_context == null)
            {
                throw new Exception("Collection is empty");
            }

            _context.Remove((from item in _context where item.Id == id select item).Single());
        }
    }
}
