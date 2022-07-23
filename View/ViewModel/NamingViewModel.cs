using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MultimediaApp.Model;

namespace MultimediaApp;

internal class NamingViewModel : INotifyPropertyChanged
{
    private readonly GalleryService _galleryService;

    public NamingViewModel()
    {
        if (!IsInDesignMode)
            _galleryService = GalleryService.GetInstance();       
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

    private RelayCommand getCommand;
    public RelayCommand GetCommand
    {
        get
        {
            return getCommand ??= new RelayCommand(obj =>
            {
                if (PicPath == null)
                {
                    System.Windows.MessageBox.Show("Please, choose a picture");
                    return;
                }
                if (PicName == null)
                    PicName = Path.GetFileName(PicPath);
                if (PicTag == "Show all")
                    System.Windows.MessageBox.Show("Please, choose other category name");
                var pic = new Picture { Name = PicName, Tag = PicTag, Path = PicPath, Id = GetLastItemId() + 1};
                _galleryService.Add(pic);

                // CLOSE THIS WINDOW
                
                //_picture = new PictureModel(_picName, _picTag, PicPath);
            });
        }
    }

    public int GetLastItemId()
    {
        List<int> ids = (from pic in _galleryService.GetAll() select pic.Id).ToList();
        int lastId;
        lastId = ids.Count == 0 ? 0 : ids.Max();
        return lastId;

        // =========================

        //var lastItem = _galleryService.GetAll()[_galleryService.GetAll().Count - 1];
        //return lastItem.Id + 1;
    }

    private RelayCommand _openFileDialogCommand;
    public RelayCommand OpenFileDialogCommand
    {
        get
        {
            return _openFileDialogCommand ??= new RelayCommand(obj =>
            {
                // Open FileDialog to take a PicFile
                var dialog = new OpenFileDialog { Filter = "Image Files| *.jpg; *.jpeg; *.png;" };
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    PicPath = dialog.FileName; // Getting Pic's File Path

                    // Open Naming Window to give a name to the Picture
                    var _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                    if (!_imageExtensions.Contains(Path.GetExtension(PicPath).ToUpperInvariant()))
                        System.Windows.MessageBox.Show("I don\'t get it..");
                }
            });
        }
    }

    private string _picName;
    public string PicName
    {
        get => _picName;
        
        set
        {
            if (_picName != value)
            {
                _picName = value;
                OnPropertyChanged("PicName");
            }
        }
    }

    private string _picTag;
    public string PicTag
    {
        get => _picTag;

        set
        {
            if (_picTag != value)
            {
                _picTag = value;
                OnPropertyChanged("PicTag");
            }
        }
    }

    private string _picPath;
    public string PicPath
    {
        get => _picPath;
        set
        {
            if (_picPath != value)
            {
                _picPath = value;
                OnPropertyChanged("PicPath");
            }
        }
    }
}
