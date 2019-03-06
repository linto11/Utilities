using Com.Utility.Collection.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Com.Utility.Collection
{
    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string StripNonString(this string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                inputValue =
                    Regex.Replace(inputValue, @"[\p{C}]|[\p{So}]|[\u20E3]", string.Empty);
                inputValue = inputValue.Trim();
            }

            return inputValue;
        }

        public static string ExtractDate(this string _value, string customFormat = Constants.DateFormat.ServerDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(_value, customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date.ToString(Constants.DateFormat.ServerDate);
            else
                return null;
        }

        public static string ExtractTime24H(this string _value, string customFormat = Constants.DateFormat.ServerDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(_value, customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date.ToString(Constants.DateFormat.ServerTime);
            else
                return null;
        }

        public static string FromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetDescription(this Enum en)
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

        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static bool IsNullOrEmpty(this string _value)
        {
            if (string.IsNullOrEmpty(SafeTypeHelper.SafeString(_value)))
                return true;
            else
                return false;
        }

        public static bool IsNumber(this string _value)
        {
            foreach (char c in _value)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static bool IsValidDateFormat(this string _value, string customFormat = Constants.DateFormat.ServerDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(_value, customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return true;
            else
                return false;
        }

        public static bool IsValidDateTimeFormat(this string _value, string customFormat = Constants.DateFormat.ServerDateTime)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(_value, customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                return true;
            else
                return false;
        }

        public static bool IsValidEmailAddress(this string _value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(_value);
                return addr.Address == _value;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidJson(this string _value)
        {
            _value = _value.ToSafeString();
            if ((_value.StartsWith("{") && _value.EndsWith("}")) || //For object
                (_value.StartsWith("[") && _value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(_value);
                    return true;
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidMobileNumberFormat(this string _value)
        {
            bool isValid = false;

            //check if mobile number is available and format
            if (!_value.IsNullOrEmpty())
            {
                string part1 = _value.Substring(0, 2);
                string part2 = _value.Substring(1);
                if (part1.Equals("00") && part2.IsNumber())
                    isValid = true;
            }

            return isValid;
        }

        public static bool IsValidTimeFormat(this string _value, string customFormat = Constants.DateFormat.TimeSpan)
        {
            string modifiedDateTime = string.Format("{0} {1}", DateTime.UtcNow.ToString(Constants.DateFormat.ServerDate), _value);

            DateTime dateTime;
            if (DateTime.TryParseExact(modifiedDateTime, Constants.DateFormat.ServerDateTime,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Merges two object instances together.  The primary instance will retain all non-Null values, and the second will merge all properties that map to null properties the primary
        /// </summary>
        /// <typeparam name="T">Type Parameter of the merging objects. Both objects must be of the same type.</typeparam>
        /// <param name="primary">The object that is receiving merge data (modified)</param>
        /// <param name="secondary">The object supplying the merging properties.  (unmodified)</param>
        /// <returns>The primary object (modified)</returns>
        public static T MergeWith<T>(this T primary, T secondary)
        {
            if (primary != null && secondary != null)
                foreach (var pi in typeof(T).GetProperties())
                {
                    var priValue = pi.GetGetMethod().Invoke(primary, null);
                    var secValue = pi.GetGetMethod().Invoke(secondary, null);

                    if (priValue == null || priValue.GetType().IsValueType)
                        pi.GetSetMethod().Invoke(primary, new[] { secValue });
                }

            return primary;
        }

        public static List<T> MergeWith<T>(this List<T> primaries, List<T> secondaries, string propertyNameToValidate)
        {
            int primaryIndex = 0;
            foreach (var primary in primaries.ToList())
            {
                if (primary != null)
                    foreach (var secondary in secondaries.ToList())
                    {
                        if (secondary != null)
                        {
                            var primaryProperties = primary.GetType().GetProperties();
                            var secondaryProperties = primary.GetType().GetProperties();

                            var priProperty = primaryProperties.Where(p => p.Name.Equals(propertyNameToValidate)).FirstOrDefault();
                            var secProperty = secondaryProperties.Where(p => p.Name.Equals(propertyNameToValidate)).FirstOrDefault();

                            var priValue = priProperty.GetGetMethod().Invoke(primary, null);
                            var secValue = secProperty.GetGetMethod().Invoke(secondary, null);

                            if (priValue != null && secValue != null && priValue.Equals(secValue))
                            {
                                primaries[primaryIndex] = primary.MergeWith(secondary);
                                break;
                            }
                        }
                    }
                primaryIndex++;
            }

            return primaries;
        }

        public static int? Nullify(this int? _value)
        {
            if (_value > 0)
                return _value;
            else
                return null;
        }

        public static string Nullify(this string _value)
        {
            if (string.IsNullOrEmpty(SafeTypeHelper.SafeString(_value)))
                return null;
            else
                return _value;
        }

        public static string RemoveLastChar(this string _value)
        {
            string newVal = SafeTypeHelper.SafeString(_value);
            if (!string.IsNullOrEmpty(newVal))
                newVal = newVal.Remove(newVal.Length - 1);

            return newVal;
        }

        public static object SqlNullify(this int _value)
        {
            if (_value > 0)
                return _value;
            else
                return DBNull.Value;
        }

        public static object SqlNullify(this string _value)
        {
            if (string.IsNullOrEmpty(SafeTypeHelper.SafeString(_value)))
                return DBNull.Value;
            else
                return _value;
        }

        public static object SqlNullify(this DateTime _value)
        {
            if (_value.Year < 1900)
                return DBNull.Value;
            else
                return _value;
        }

        public static string ToAlphaNumericString(this string inString)
        {
            if (string.IsNullOrEmpty(inString))
                return string.Empty;

            return new string(inString.Where(c => char.IsLetterOrDigit(c)).ToArray());
        }

        public static string ToBase64(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string ToPascalCase(this string rawString)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(rawString, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }

        public static string ToCamelCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        public static string ToCommaSeperatedCurrency(this double _currency)
        {
            return string.Format("{0:0,0.00}", _currency);
        }

        public static string ToFormattedCurrency(this double _currency, string seperator)
        {
            return string.Format("{0:0" + seperator + "0.00}", _currency);
        }

        public static string ToCustomDateFormat(this string _value, string toCustomFormat,
            string fromCustomFormat = Constants.DateFormat.ServerDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(_value, fromCustomFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date.ToString(toCustomFormat);
            else
                return null;
        }

        public static string ToCustomDateFormat(this DateTime _value, string toCustomFormat = Constants.DateFormat.ServerDateTime)
        {
            return _value.ToString(toCustomFormat);
        }

        public static DateTime ToCustomTimeZone(this DateTime _value, string timeZoneName = Constants.TimeZone.Default)
        {
            try
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                return TimeZoneInfo.ConvertTime(_value, timeZoneInfo);
            }
            catch
            {
                return _value;
            }
        }

        public static DateTime? ToDateTime(this string _value,
            string fromCustomFormat = Constants.DateFormat.ServerDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(_value, fromCustomFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date;
            else
                return null;
        }

        public static string ToSafeString(this string _value)
        {
            return SafeTypeHelper.SafeString(_value);
        }

        public static string ToTitleCase(this string _value)
        {
            CultureInfo _Cultureinfo;
            TextInfo _Textinfo;
            try
            {
                _Cultureinfo = Thread.CurrentThread.CurrentCulture;
                _Textinfo = _Cultureinfo.TextInfo;

                return _Textinfo.ToTitleCase(_value);
            }
            catch (Exception)
            {
                return _value;
            }
            finally
            {
                _Cultureinfo = null;
                _Textinfo = null;
            }
        }

        public static long ToUnixTimeSeconds(this string _value,
            string fromCustomFormat = Constants.DateFormat.ServerDate)
        {
            var dateTime = SafeTypeHelper.SafeTime(_value.ToDateTime(fromCustomFormat));

            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(dateTime.Ticks) - epochTicks;

            return (long)unixTicks.TotalSeconds;
        }
    }
}