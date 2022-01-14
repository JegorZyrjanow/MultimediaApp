using MultimediaApp.MVVM.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MultimediaApp
{
    class EditViewModel : INotifyPropertyChanged
    {
        private readonly GalleryService _galleryService;

        public EditViewModel()
        {
            if (!IsInDesignMode)
            {
                _galleryService = GalleryService.GetInstance();
                if (_galleryService.SelectedPicture != null)
                {
                    NewPicName = _galleryService.SelectedPicture.Name;
                    OnPropertyChanged("NewPicName");
                    NewPicTags = string.Join(" ", _galleryService.SelectedPicture.Tags);
                    OnPropertyChanged("NewPicTags");
                }
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

        private RelayCommand getCommand;
        public RelayCommand GetCommand
        {
            get
            {
                return getCommand ?? (getCommand = new RelayCommand(obj =>
                {
                    if (_galleryService.SelectedPicture == null)
                    {
                        System.Windows.MessageBox.Show("Please, choose a picture");
                        return;
                    }

                    List<string> newListOfTags = new List<string>(NewPicTags.Split(' ').ToList());
                    // here we throw new strings

                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                    if (NewPicTags == "Show all")
                    {
                        System.Windows.MessageBox.Show("Please, choose other category name");
                        return;
                    }
                    _galleryService.EditPicture(NewPicName, newListOfTags);
                }));
            }
        }
                
        private string _picName;
        public string NewPicName
        {
            get => _picName;

            set
            {
                if (_picName != value)
                {
                    _picName = value;
                    OnPropertyChanged("NewPicName");
                }
            }
        }

        private List<string> _picTags;
        public string NewPicTags
        {
            get
            {
                if (_picTags == null)
                {
                    return null;
                }
                return string.Join(" ", _picTags);
            }

            set
            {
                if (_picTags != new List<string>(value.Split(' ')))
                {
                    _picTags = new List<string>(value.Split(' '));
                    OnPropertyChanged("NewPicTags");
                }
            }
        }
    }
}
