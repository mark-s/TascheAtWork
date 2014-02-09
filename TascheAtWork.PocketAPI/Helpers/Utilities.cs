using System;
using System.Collections.Generic;

namespace TascheAtWork.PocketAPI.Helpers
{
    /// <summary>
    /// General utilities
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// converts DateTime to an UNIX timestamp
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>UNIX timestamp</returns>
        public static int? GetUnixTimestamp(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            return (int)((DateTime)dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }


        /// <summary>
        /// Convert a dictionary to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static List<T> DictionaryToList<T>(Dictionary<string, T> dictionary) where T : new()
        {
            if (dictionary == null)
                return null;

            var itemEnumerator = dictionary.GetEnumerator();
            var items = new List<T>();

            while (itemEnumerator.MoveNext())
                items.Add(itemEnumerator.Current.Value);

            return items;
        }
    }
}
