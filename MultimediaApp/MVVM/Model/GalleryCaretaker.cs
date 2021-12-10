using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp.MVVM.Model
{
    internal class GalleryCaretaker
    {
        private readonly List<IMemento> _mementos = new List<IMemento>();
        private readonly GalleryService _galleryService; // Dependency

        public GalleryCaretaker(GalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        public void Backup()
        {
            _mementos.Add(_galleryService.Save());
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
                return;
            var memento = _mementos.Last();
            _mementos.Remove(memento);
            try
            {
                _galleryService.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }
    }
}
