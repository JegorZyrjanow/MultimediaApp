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
        #region propchanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

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
                }
            }
        }

        private RelayCommand getCommand;
        public RelayCommand GetCommand
        {
            get
            {
                return getCommand ?? (getCommand = new RelayCommand(obj =>
                {
                    _picture.Name = _picName;
                    _picture.Tag = _picTag;
                    _galleryService.Add(_picture);

                    Window w = System.Windows.Application.Current.Windows[0];
                    w.Close();
                }));
            }
        }

        //private PictureModel GetPic(PictureModel picture)
        //{            
        //    return _picture;
        //}
    }
}
