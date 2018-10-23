using System;

namespace StatisticsViewer.Utils
{
    public static class Time
    {
        private static readonly DateTime Start = new DateTime(1970, 1, 1);

        public static double Get
        {
            get { return DateTime.UtcNow.Subtract(Start).TotalMilliseconds; }
        }
    }
}