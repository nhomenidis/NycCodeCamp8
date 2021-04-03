using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace CodeCamp.Core.Converters
{
    public class StringFormatValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            return string.Format("{0:" + parameter + "}", value);
        }
    }
}