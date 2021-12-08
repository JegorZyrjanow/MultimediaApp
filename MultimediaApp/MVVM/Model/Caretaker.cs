using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaApp.MVVM.Model
{
    internal class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private MainViewModel _viewModel; // Dependency

        public Caretaker(MainViewModel ViewModel)
        {
            _viewModel = ViewModel;
        }

        public void Backup()
        {
            _mementos.Add(_viewModel.Save());
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                return;
            }

            var memento = _mementos.Last();
            _mementos.Remove(memento);

            try
            {
                _viewModel.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }

    }
}
