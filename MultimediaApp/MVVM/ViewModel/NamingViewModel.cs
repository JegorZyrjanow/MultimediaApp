using MultimediaApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultimediaApp
{
    internal class NamingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private Picture _picture;
        private string _filePath;

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

        private string _category;
        public string PicCategory
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    _category = value;
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
                    
                }));
            }
        }

        private Picture GetPic(Picture picture)
        {
            return _picture;
        }
    }
}
