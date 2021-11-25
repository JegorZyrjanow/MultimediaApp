using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MultimediaApp.Properties;

namespace MultimediaApp
{
    /// <summary>
    /// Converts a full path to a specific image type of a pic
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path
            string fullPath = (string)value;
            //Uri relPath = new Uri(Directory.GetCurrentDirectory()).MakeRelativeUri(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));

            // If the path is null, ignore
            if (fullPath == null)
                return null;

            // Get the name of the file/folder
            var name = Helper.GetFileName(fullPath);

            // By default, we presume an image
            var image = "icons/file.png";

            // List of image extensions
            List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

            // The the name is black, we presume it's a drive as we cannot have a black file or foldr name
            //if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            //    image = "icons/folder-closed.png";
            //else
            if (imageExtensions.Contains(Path.GetExtension(fullPath).ToUpperInvariant()))
            {
                try
                {
                    return new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
                }
                catch
                {
                    return Resources.ae8ac2fa217d23aadcc913989fcc34a2;
                    //return new BitmapImage(Resources.ae8ac2fa217d23aadcc913989fcc34a2);
                    throw;
                }
                finally { }

                //return new BitmapImage(new Uri(Path.GetFullPath(fullPath), UriKind.Absolute));
            }
            else
                return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
