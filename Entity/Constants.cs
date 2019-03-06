using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Utility.Collection.Entity
{
    public class Constants
    {
        public struct Country
        {
            public const string Alpha2Code = "AE";
        }

        public struct BooleanInt
        {
            public const int Valid = 1;
            public const int Invalid = 0;

            public const int True = 1;
            public const int False = 0;
        }

        public struct Boolean
        {
            public const bool True = true;
            public const bool False = false;
        }

        public struct DateFormat
        {
            public const string ServerDate = "yyyy-MM-dd";
            public const string ServerTime = "HH:mm";
            public const string ServerDateTime = "yyyy-MM-dd HH:mm";
            public const string TimeSpan = @"hh\:mm";
        }

        public struct TimeZone
        {
            public const string Default = "Arabian Standard Time";
        }
    }
}