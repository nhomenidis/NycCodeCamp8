using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace CodeCamp.Core.Converters
{
    public class MultiLineTextValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString().Replace("{nl}", Environment.NewLine);
        }
    }
}