using Content_Player.Models;
using Data.Entities;
using Shared.Services;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Content_Player.Converters
{
    public class MaterialTypeToImageConverter : IValueConverter
    {
        public object? Convert(object material, Type targetType, object parameter, CultureInfo culture)
        {
            switch (material)
            {
                case Material:
                    return new BitmapImage(UriService.GetAbsoluteUri("/Images/listening.png", "Shared"));
                case FakeSegmentMaterial:
                    return new BitmapImage(UriService.GetAbsoluteUri("/Images/testing.png", "Shared"));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
