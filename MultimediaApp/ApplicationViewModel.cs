using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MultimediaApp
{
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        private XmlFormatter _xmlFormatter = new XmlFormatter();
        private ObservableCollection<Picture> _pictures;
        private Caretaker _collectionCaretaker;
        private Picture _selectedPicture;

        public ObservableCollection<Picture> Collection { get; set; }

        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
                OnPropertyChanged("BitmapImage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //your code
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                //OnPropertyChanged();
            }
        }
        private List<string> _categories;
        public List<string> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        _collectionCaretaker.Backup();

                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Image Files| *.jpg; *.jpeg; *.png;";
                        if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                            return;
                        string filePath = openFileDialog.FileName;

                        List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                        if (!_imageExtensions.Contains(Path.GetExtension(filePath).ToUpperInvariant()))
                        {
                            System.Windows.MessageBox.Show("I don\'t get it..");
                            return;
                        }

                        NamingWindow namingWindow = new NamingWindow(filePath);
                        namingWindow.ShowDialog();
                        _pictures.Add(namingWindow.GetPic());
                        SelectedPicture = namingWindow.GetPic();
                    }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ?? (removeCommand = new RelayCommand(obj =>
                {
                    _collectionCaretaker.Backup();

                    Picture pic = obj as Picture;
                    if (pic != null)
                    {
                        Collection.Remove(pic);
                    }
                },
                (obj) => Collection.Count > 0));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(obj =>
                {
                    _xmlFormatter.Serialize(Collection);
                }));
            }
        }

        private RelayCommand undoCommand;
        public RelayCommand UndoCommand
        {
            get
            {
                return undoCommand ?? (undoCommand = new RelayCommand(obj =>
                {
                    _collectionCaretaker.Undo();
                }));
            }
        }

        public BitmapImage BitmapImage
        {
            get
            {
                try
                {
                    if (_selectedPicture != null)
                    {
                        return new BitmapImage(new Uri(_selectedPicture.Path));
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri($"pack://application:,,,/Images/notFound.png"));
                }
            }
        }

        public ApplicationViewModel()
        {
            // Deserialize existing XML with list
            _xmlFormatter.Deserialize();
            // Init a pictures collection
            _pictures = _xmlFormatter.GetCollection();
            // Throw it to property
            Collection = _pictures;
            // Getting categories list
            _categories = new List<string> { "All" };
            _categories.AddRange(Collection.Select(o => o.Category).Distinct().ToList());
            // Init Caretaker (for Undo func)
            _collectionCaretaker = new Caretaker(this);
        }

        public IMemento Save()
        {
            return new CollectionMemento(this.Collection);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is CollectionMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            this.Collection = memento.GetState();
        }

    }

    public interface IMemento
    {
        ObservableCollection<Picture> GetState();
    }

    class CollectionMemento : IMemento
    {
        private ObservableCollection<Picture> _collection = new ObservableCollection<Picture>();

        public CollectionMemento(ObservableCollection<Picture> MainList)
        {
            this._collection = MainList;
        }

        // Создатель использует этот метод, когда восстанавливает своё
        // состояние.
        public ObservableCollection<Picture> GetState()
        {
            return this._collection;
        }
    }

    class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private ApplicationViewModel _viewModel;

        public Caretaker(ApplicationViewModel ViewModel)
        {
            this._viewModel = ViewModel;
        }

        public void Backup()
        {
            this._mementos.Add(this._viewModel.Save());
        }

        public void Undo()
        {
            if (this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            try
            {
                this._viewModel.Restore(memento);
            }
            catch (Exception)
            {
                this.Undo();
            }
        }

    }
}
