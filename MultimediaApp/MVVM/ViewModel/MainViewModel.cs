using MultimediaApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;


namespace MultimediaApp
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        readonly GalleryService _galleryService;
        public MainViewModel()
        {
            _galleryService = GalleryService.GetInstance();
            _galleryService.SetExistingCollectionFromXml();
            _galleryService.SetTags();
            _galleryService.Pictures.CollectionChanged += CollectionChangedMethod;

            foreach (var item in _galleryService.GetAll())
            {
                _pictures.Add(item);
            }
            
            // Getting categories list
            //_categories = new List<string> { "All" };
            //_categories.AddRange(Pictures.Select(o => o.Category).Distinct().ToList());
            //_categories = _categories.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            //_view = CollectionViewSource.GetDefaultView(Pictures);
            //_view.Filter = new Predicate<object>(item => Filter(item as Picture));
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Different kind of changes that may have occurred in collection            
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Replace
                || e.Action == NotifyCollectionChangedAction.Remove
                || e.Action == NotifyCollectionChangedAction.Move)
                OnPropertyChanged("Pictures");
        }

        private ObservableCollection<Picture> _pictures = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> Pictures
        {
            get { return _pictures; }
            set
            {
                _pictures = value;
            }   
        }

        private Picture _selectedPicture;
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
                return addCommand ?? (addCommand = new RelayCommand(obj => _galleryService.Add()));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ?? (removeCommand = new RelayCommand(obj => _galleryService.Remove(_selectedPicture.Id),
                (obj) => Pictures.Count > 0));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(obj => _galleryService.SaveToXml()));
            }
        }

        private RelayCommand undoCommand;
        public RelayCommand UndoCommand
        {
            get
            {
                return undoCommand ?? (undoCommand = new RelayCommand(obj => _galleryService.Undo()));
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //private string _searchText;
        //public string SearchText
        //{
        //    get { return _searchText; }
        //    set
        //    {
        //        if (_searchText != value)
        //        {
        //            _searchText = value;
        //            OnPropertyChanged("SearchText");

        //            //==
        //            _view.Filter = new Predicate<object>(item => Filter(item as Picture));
        //            _view.Refresh();

        //            OnPropertyChanged("Collection");
        //            // ==

        //            FilteredView.Refresh();
        //        }
        //    }
        //}

        //private bool Filter(Picture pic)
        //{
        //    return _searchText == null || pic.Name.Contains(_searchText);
        //}

        //private void SearchTextChanged(string text)
        //{
        //    // Here is the lambda with your conditions to filter
        //    //_view.Filter = (o) => { return o; };

        //    _view.Filter = delegate (object item)
        //    {
        //        return ((Picture)item).Name.Contains(_searchText);
        //    };

        //    Pictures.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedMethod);
        //    _view.Refresh();
        //}

        //private RelayCommand _eventSearch;
        //public RelayCommand EventSearch
        //{
        //    get
        //    {
        //        return _eventSearch ?? (_eventSearch = new RelayCommand(obj =>
        //        {
        //            //var eventList = await App.MobileService.GetTable<Event>().ToListAsync();

        //        }));
        //    }
        //}

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
    }
}
