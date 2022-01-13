using MultimediaApp.MVVM.Model;
//using MultimediaApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;


namespace MultimediaApp
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        readonly GalleryService _galleryService;

        private string lastName = "";
        private string lastTag = "Show all";

        public MainViewModel()
        {
            if (!IsInDesignMode)
            {
                _galleryService = GalleryService.GetInstance();
                // Getting existing pics collection
                Pictures = new ObservableCollection<PictureModel>(_galleryService.GetAll()); // Copy the gallery is NOT REFERENCE
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
                Pictures = new ObservableCollection<PictureModel>(_galleryService.GetAll());

                if (lastName != "")
                    SearchText = lastName;
                if (lastTag != "Show all")
                    SelectedCategory = lastTag;
            }
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Remove)
                Categories = _galleryService.GetTags();

            OnPropertyChanged("Pictures");
            OnPropertyChanged("Categories");
        }

        private ObservableCollection<PictureModel> _pictures = new ObservableCollection<PictureModel>();
        public ObservableCollection<PictureModel> Pictures
        {
            get { return _pictures; }
            set
            {
                ObservableCollection<PictureModel> newList = new ObservableCollection<PictureModel>(value.OrderBy(pic => pic.Name));
                _pictures = newList;
            }
        }

        private PictureModel _selectedPicture;
        public PictureModel SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                _galleryService.SelectedPicture = _selectedPicture;
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
                if (lastName != "")
                    Pictures = new ObservableCollection<PictureModel>(from pic in _pictures where pic.Name.Contains(lastName) select pic);
                lastTag = value; // Save last category query
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

        private RelayCommand editPicCommand;
        public RelayCommand EditPicCommand
        {
            get
            {
                return editPicCommand ?? (editPicCommand = new RelayCommand(obj => new EditWindow().ShowDialog()));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                _pictures = new ObservableCollection<PictureModel>(_galleryService.GetPicturesByName(value));
                if (lastTag != "Show all")
                {
                    //var cats = treeOfCats.SelectMany(x => x).Distinct().ToList();
                    //cats = cats.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                    //return cats; // (from pic in _pictures select pic.Tag).Distinct().ToList()

                    //List<List<string>> treeOfCats = new List<List<string>>((from pic in _pictures select pic.Tags).Distinct().ToList());
                    //List<List<PictureModel>> treeOfPics = new List<List<PictureModel>>((from pic in _pictures select pic).ToList();

                    Pictures = new ObservableCollection<PictureModel>(from pic in _pictures where pic.Tags.Contains(lastTag) select pic);
                }
                lastName = value; //Save last search text
                OnPropertyChanged("Pictures");
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
                    var memory = new MemoryStream();
                    Properties.Resources.MissingImage.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;

                    //IntPtr hBitmap = Properties.Resources.MissingImage.GetHbitmap();
                    //BitmapImage converted = new BitmapImage(new Uri(Imaging.CreateBitmapSourceFromHBitmap(
                    //    hBitmap,
                    //    IntPtr.Zero,
                    //    Int32Rect.Empty,
                    //    BitmapSizeOptions.FromEmptyOptions()).ToString()));
                    ////return new BitmapImage(Properties.Resources.MissingImage); // If file not found show that it is
                    //return converted;
                }
            }
        }



    }
}
