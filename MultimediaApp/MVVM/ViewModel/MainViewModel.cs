using MultimediaApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;


namespace MultimediaApp
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        readonly GalleryService _galleryService;

        private string lastTag;
        private string lastName;

        public MainViewModel()
        {
            if (!IsInDesignMode)
            {
                _galleryService = GalleryService.GetInstance();
                // Getting existing pics collection
                _pictures = new ObservableCollection<PictureModel>(_galleryService.GetAll()); // Copy the gallery is NOT REFERENCE
                                                                                              // Getting categories
                Categories = _galleryService.GetTags();
                // Watching any changes in the GalleryService
                _galleryService.Pictures.CollectionChanged += CollectionChangedMethod;
            }
        }

        private bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Different kind of changes that may have occurred in collection            
            if (e.Action == NotifyCollectionChangedAction.Add
            || e.Action == NotifyCollectionChangedAction.Replace
            || e.Action == NotifyCollectionChangedAction.Remove
            || e.Action == NotifyCollectionChangedAction.Move)
            {

                _pictures = new ObservableCollection<PictureModel>(_galleryService.GetPicturesByTag(lastTag));
                _pictures = new ObservableCollection<PictureModel>(_galleryService.GetPicturesByName(lastName));

                //_pictures = new ObservableCollection<PictureModel>(_galleryService.GetAll()); // replace with filters 
                OnPropertyChanged("Pictures");
                OnPropertyChanged("Categories");
            }
        }

        private ObservableCollection<PictureModel> _pictures = new ObservableCollection<PictureModel>();
        public ObservableCollection<PictureModel> Pictures
        {
            get { return _pictures; }
            set { _pictures = value; }
        }

        private PictureModel _selectedPicture;
        public PictureModel SelectedPicture
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

        private List<string> _categories;
        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = new List<string>();
                _categories.Clear();
                _categories.Add("Show all");
                _categories.AddRange(value);
            } 
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                _pictures = new ObservableCollection<PictureModel>(_galleryService.GetPicturesByTag(value));
                OnPropertyChanged("Pictures");
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(obj =>
                {
                    new NamingWindow().ShowDialog(); // Open Naming Window to set Pic parameters
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
                    if (SelectedPicture == null)
                        return;
                    _galleryService.Remove(_selectedPicture.Id);
                },
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
                        return new BitmapImage(new Uri(_selectedPicture.Path));
                    else
                        return null;
                }
                catch (Exception)
                {
                    // If file not found show that it is lol
                    return new BitmapImage(new Uri($"pack://application:,,,/Images/notFound.png"));
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("SearchText");
                    _pictures = new ObservableCollection<PictureModel>(_galleryService.GetPicturesByName(value));
                    lastName = value; //Save last search text
                    OnPropertyChanged("Pictures");
                }
            }
        }

        

    }
}
