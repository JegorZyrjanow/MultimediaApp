using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        private string _name;
        public string PicName
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
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

        public Picture GetCommand()
        {
            return _picture;
        }
    }
}
