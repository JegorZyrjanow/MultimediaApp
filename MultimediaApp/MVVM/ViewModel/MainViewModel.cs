using MultimediaApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;


namespace MultimediaApp
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        readonly GalleryService _galleryService;
        public MainViewModel()
        {
            _galleryService = GalleryService.GetInstance();
            //_galleryService.SetExistingCollectionFromXml();
            Categories = _galleryService.GetCategories();
            _galleryService.Pictures.CollectionChanged += CollectionChangedMethod;

            foreach (var item in _galleryService.GetAll())
            {
                _pictures.Add(item);
            }

        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Different kind of changes that may have occurred in collection            
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Replace
                || e.Action == NotifyCollectionChangedAction.Remove
                || e.Action == NotifyCollectionChangedAction.Move)
            {
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
            set { _categories = value; }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                _galleryService.GetByTag(_selectedCategory);
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(obj =>
                {
                    // Open FileDialog to take a PicFile
                    OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files| *.jpg; *.jpeg; *.png;" };
                    if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
                    string filePath = openFileDialog.FileName; // Getting Pic's File Path

                    // Open Naming Window to give a name to the Picture
                    List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                    if (!_imageExtensions.Contains(Path.GetExtension(filePath).ToUpperInvariant())) System.Windows.MessageBox.Show("I don\'t get it..");
                    else new NamingWindow().ShowDialog();



                    _galleryService.Add();
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
