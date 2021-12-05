using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MultimediaApp
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private XmlFormatter _xmlFormatter = new XmlFormatter();
        private ObservableCollection<Picture> _pictures;
        private Caretaker _collectionCaretaker;
        private Picture _selectedPicture;
        private ICollectionView _view;

        public ObservableCollection<Picture> Collection { get; set; }

        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
                OnPropertyChanged("BitmapImage");
                OnPropertyChanged("SearchText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

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
                OnPropertyChanged("SelectedCategory");
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

                        NamingWindow namingWindow = new NamingWindow();
                        namingWindow.ShowDialog();
                        //_pictures.Add(namingWindow.GetPic());
                        //SelectedPicture = namingWindow.GetPic();
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
                return undoCommand ?? (undoCommand = new RelayCommand(obj => _collectionCaretaker.Undo()));
            }
        }

        private void collectionChanged()
        {

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

        public MainViewModel()
        {
            // Deserialize existing XML with list
            _xmlFormatter.Deserialize();

            // Init a pictures collection
            _pictures = _xmlFormatter.GetCollection();

            // Throw it to property
            Collection = _pictures;
            _view = CollectionViewSource.GetDefaultView(Collection);
            _view.Filter = new Predicate<object>(item => Filter(item as Picture));

            // Getting categories list
            _categories = new List<string> { "All" };
            _categories.AddRange(Collection.Select(o => o.Category).Distinct().ToList());
            _categories = _categories.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            // Init Caretaker (for Undo func)
            _collectionCaretaker = new Caretaker(this);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("SearchText");

                    //==
                    _view.Filter = new Predicate<object>(item => Filter(item as Picture));
                    _view.Refresh();

                    OnPropertyChanged("Collection");
                    // ==

                    FilteredView.Refresh();
                }
            }
        }

        public ICollectionView FilteredView { get; private set; }

        private bool Filter(Picture pic)
        {
            return _searchText == null || pic.Name.Contains(_searchText);
        }

        private void SearchTextChanged(string text)
        {
            // Here is the lambda with your conditions to filter
            //_view.Filter = (o) => { return o; };

            _view.Filter = delegate (object item)
            {
                return ((Picture)item).Name.Contains(_searchText);
            };

            Collection.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedMethod);
            _view.Refresh();
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //_view.Filter = delegate (object item)
                //{
                //    return ((Picture)item).Name.Contains(_searchText);
                //};
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                //your code
            }
        }

        private RelayCommand _eventSearch;
        public RelayCommand EventSearch
        {
            get
            {
                return _eventSearch ?? (_eventSearch = new RelayCommand(obj =>
                {
                    //var eventList = await App.MobileService.GetTable<Event>().ToListAsync();

                }));
            }
        }

        public void TextSearchFilter(ICollectionView filteredView, string searchText)
        {
            //Collection.

            //filteredView.Filter = delegate (object obj)
            //{
            //    if (string.IsNullOrEmpty(searchText))
            //        return true;

            //    string str = obj as string;
            //    if (string.IsNullOrEmpty(str))
            //        return false;

            //    int index = str.IndexOf(searchText, 0, StringComparison.InvariantCultureIgnoreCase);

            //    return index > -1;
            //};
            //filteredView.Refresh();
        }

        //private int _searchText;
        //public int SearchText
        //{
        //    get { return _searchText; }
        //    set
        //    {
        //        IsReadOnly = true;
        //        _searchText = value;
        //        OnPropertyChanged("Price");
        //        //Do your Calculation here
        //        IsReadOnly = false;
        //    }
        //}

        //private bool _isReadOnly;
        //public bool IsReadOnly
        //{
        //    get { return _isReadOnly; }
        //    set
        //    {
        //        _isReadOnly = value;
        //        OnPropertyChanged("IsReadOnly");
        //    }
        //}

        //public class TextSearchFilter
        //{
        //    public TextSearchFilter(ICollectionView filteredView, string searchText)
        //    {
        //        filteredView.Filter = delegate (object obj)
        //        {
        //            if (string.IsNullOrEmpty(searchText))
        //                return true;

        //            string str = obj as string;
        //            if (string.IsNullOrEmpty(str))
        //                return false;

        //            int index = str.IndexOf(searchText, 0, StringComparison.InvariantCultureIgnoreCase);

        //            return index > -1;
        //        };
        //            filteredView.Refresh();
        //    }
        //}

        public IMemento Save()
        {
            return new CollectionMemento(Collection);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is CollectionMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            Collection.Clear();
            foreach (var item in memento.GetState())
            {
                Collection.Add(item);
            }
        }

    }

    public interface IMemento
    {
        ObservableCollection<Picture> GetState();
    }

    class CollectionMemento : IMemento
    {
        private ObservableCollection<Picture> _collection;

        public CollectionMemento(ObservableCollection<Picture> MainList)
        {
            _collection = new ObservableCollection<Picture>(MainList);
        }

        // Создатель использует этот метод, когда восстанавливает своё
        // состояние.
        public ObservableCollection<Picture> GetState()
        {
            return _collection;
        }
    }

    class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private MainViewModel _viewModel;

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
