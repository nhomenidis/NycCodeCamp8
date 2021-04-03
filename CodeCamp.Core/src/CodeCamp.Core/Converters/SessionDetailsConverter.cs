using System;
using System.Globalization;
using CodeCamp.Core.Data.Entities;
using MvvmCross.Platform.Converters;

namespace CodeCamp.Core.Converters
{
    public class SessionDetailsConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Session))
                return null;

            var session = (Session)value;

            return session.SpeakerId.HasValue 
                ? string.Format("{0}, {1}", session.RoomName, session.SpeakerName)
                : session.RoomName;
        }
    }
}