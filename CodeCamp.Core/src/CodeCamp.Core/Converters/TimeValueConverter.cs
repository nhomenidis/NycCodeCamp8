using System;
using System.Globalization;
using CodeCamp.Core.Extensions;
using MvvmCross.Platform.Converters;

namespace CodeCamp.Core.Converters
{
    public class TimeValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(DateTime))
                return null;

            return ((DateTime)value).FormatTime();
        }
    }
}