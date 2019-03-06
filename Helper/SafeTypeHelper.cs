using Com.Utility.Collection.Entity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Utility.Collection
{
    public sealed class SafeTypeHelper
    {
        #region safe type convert

        public static byte SafeByte(object o)
        {
            byte returnValue = 0;
            try
            {
                returnValue = Convert.ToByte(o);
            }
            catch
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the long.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static long SafeLong(object o)
        {
            long returnValue = 0;
            try
            {
                returnValue = Convert.ToInt64(o);
            }
            catch
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the int.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static int SafeInt(object o)
        {
            int returnValue = 0;
            try
            {
                returnValue = Convert.ToInt32(o);
            }
            catch
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the double.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static double SafeDouble(object o)
        {
            double returnValue = 0.0;
            try
            {
                returnValue = Convert.ToDouble(o);
            }
            catch
            {
                returnValue = 0.0;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static string SafeString(object o)
        {
            string returnValue = "";
            try
            {
                returnValue = Convert.ToString(o);
                if (!string.IsNullOrEmpty(returnValue))
                    returnValue = returnValue.StripNonString();
            }
            catch
            {
                returnValue = "";
            }

            return returnValue.Trim();
        }

        /// <summary>
        /// Safes the bool.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static bool SafeBool(object o)
        {
            bool returnValue = false;
            try
            {
                returnValue = Convert.ToBoolean(o);
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        public static bool SafeSQLBool(object o)
        {
            bool returnValue = false;
            try
            {
                if (SafeInt(o) == 1)
                    returnValue = true;
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the decimal.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static decimal SafeDecimal(object o)
        {
            decimal returnValue = 0.0M;
            try
            {
                returnValue = Convert.ToDecimal(o);
            }
            catch
            {
                returnValue = 0.0M;
            }

            return returnValue;
        }

        /// <summary>
        /// Safes the time.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static DateTime SafeTime(object o)
        {
            DateTime returnValue = DateTime.Parse("1900-01-01");
            try
            {
                returnValue = Convert.ToDateTime(o);
                if (returnValue.Year < 1900)
                {
                    returnValue = DateTime.Parse("1900-01-01");
                }
            }
            catch
            {
                returnValue = DateTime.Parse("1900-01-01");
            }

            return returnValue;
        }

        #endregion safe type convert

        #region safe type properties

        public static string DefaultString { get { return string.Empty; } }
        public static int DefaultInt { get { return 0; } }
        public static DateTime DefaltDateTime { get { return SafeTime(DateTime.Parse("1900-01-01")); } }

        public static T GetConfiguration<T>(string configurationJsonPath, string configurationJsonFilename)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(configurationJsonPath)
                    .AddJsonFile(configurationJsonFilename, optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            //update the key values
            string configSection = configuration.GetSection("Configuration").Value;
            return JsonConvert.DeserializeObject<T>(configSection);
        }

        public static string CurrentMethod
        {
            get
            {
                var st = new StackTrace();
                var sf = st.GetFrame(1);

                return sf.GetMethod().Name;
            }
        }

        #endregion safe type properties

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            monthsApart = Math.Abs(monthsApart);
            if (monthsApart > 0)
                return monthsApart;
            else
                return 1;
        }

        public static string GetTimeDifference(DateTime startDate, DateTime endDate)
        {
            string timeDifference = "Seconds";

            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            monthsApart = Math.Abs(monthsApart);

            if (monthsApart > 0)
                timeDifference = (monthsApart == 1 ? "1 Month" : monthsApart + " Months");
            else
            {
                TimeSpan diffTimeSpan = endDate.Subtract(startDate);
                int weeks = SafeInt(diffTimeSpan.TotalDays / 7);
                if (weeks > 0)
                    timeDifference = (weeks == 1 ? "1 Week" : weeks + " Weeks");
                else
                {
                    int days = SafeInt(diffTimeSpan.TotalDays);
                    if (days > 0)
                        timeDifference = (days == 1 ? "1 Day" : days + " Days");
                    else
                    {
                        int hrs = SafeInt(diffTimeSpan.TotalHours);
                        if (hrs > 0)
                            timeDifference = (hrs == 1 ? "1 Hour" : hrs + " Hours");
                        else
                        {
                            int mins = SafeInt(diffTimeSpan.TotalMinutes);
                            timeDifference = (mins == 1 ? "1 Minute" : mins + " Minutes");
                        }
                    }
                }
            }

            return timeDifference;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static string GenerateRandomString(int defaultLength = 7)
        {
            const string valid = "abcdefghijkmnpqrstuvwxyzABCDEFGHIJKMNPQRSTUVWXYZ123456789";
            int length = defaultLength;

            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
                res.Append(valid[rnd.Next(valid.Length)]);

            return res.ToString();
        }

        public static bool IsPastDateTime(string _value,
            string timeZoneName = Constants.TimeZone.Default,
            string customFormat = Constants.DateFormat.ServerDateTime)
        {
            bool isPastDateTime = true;
            try
            {
                DateTime parsedDateTime;
                if (DateTime.TryParseExact(_value, customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
                {
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                    DateTime currentUtcDateTime = DateTime.UtcNow;
                    DateTime currentTimezoneDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcDateTime, timeZoneInfo);

                    if (DateTime.Compare(parsedDateTime, currentTimezoneDateTime) >= 0)
                        isPastDateTime = false;
                }
            }
            catch { }

            return isPastDateTime;
        }

        public static ReadOnlyCollection<TimeZoneInfo> GeTimeZoneInfos()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }
    }
}