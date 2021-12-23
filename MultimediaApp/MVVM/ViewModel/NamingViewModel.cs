using MultimediaApp.MVVM.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;

namespace MultimediaApp
{
    internal class NamingViewModel : INotifyPropertyChanged
    {
        private readonly GalleryService _galleryService;
        private PictureModel _picture;

        public NamingViewModel()
        {
            _galleryService = GalleryService.GetInstance();
        }

        #region propchanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        private RelayCommand getCommand;
        public RelayCommand GetCommand
        {
            get
            {
                return getCommand ?? (getCommand = new RelayCommand(obj =>
                {
                    //_picture = new PictureModel(_picName, _picTag, PicPath);
                    _galleryService.Add(new PictureModel(_picName, _picTag, PicPath));
                }));
            }
        }

        private RelayCommand _openFileDialogCommand;
        public RelayCommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(obj =>
                {
                    // Open FileDialog to take a PicFile
                    OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files| *.jpg; *.jpeg; *.png;" };
                    if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                        return;

                    PicPath = openFileDialog.FileName; // Getting Pic's File Path

                    // Open Naming Window to give a name to the Picture
                    List<string> _imageExtensions = new List<string>() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                    if (!_imageExtensions.Contains(Path.GetExtension(PicPath).ToUpperInvariant()))
                        System.Windows.MessageBox.Show("I don\'t get it..");
                }));
            }
        }

        private string _picName;
        public string PicName
        {
            get
            {
                return _picName;
            }
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
            get
            {
                return _picTag;
            }
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
            get { return _picPath; }
            set
            {
                if (_picPath != value)
                {
                    _picPath = value;
                    OnPropertyChanged("PicPath");
                }
            }
        }        

        //private PictureModel GetPic(PictureModel picture)
        //{            
        //    return _picture;
        //}
    }
}
