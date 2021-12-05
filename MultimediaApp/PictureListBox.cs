using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultimediaApp
{
    internal class PictureListBox : UserControl
    {
        public static readonly DependencyProperty DataSource = DependencyProperty.Register(nameof(PictureCollection), typeof(ObservableCollection<Picture>), typeof(PictureListBox), new PropertyMetadata());
        
        public ObservableCollection<Picture> PictureCollection
        {
            get => (ObservableCollection<Picture>)GetValue(DataSource);
            set => SetValue(DataSource, value);
        }
    }
}
