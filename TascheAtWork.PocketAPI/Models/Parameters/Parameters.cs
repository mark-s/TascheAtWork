using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace TascheAtWork.PocketAPI.Models.Parameters
{
    /// <summary>
    /// Parameter
    /// </summary>
    internal class Parameters
    {

        /// <summary>
        /// Converts an object to a list of HTTP Post parameters.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ConvertToHTTPPostParameters()
        {
            // store HTTP parameters here
            var parameterDict = new Dictionary<string, string>();

            // get object properties
            var properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                      .Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute)));

            // gather attributes of object
            foreach (var propertyInfo in properties)
            {
                var attribute = (DataMemberAttribute)propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault();

                if (attribute == null) continue;

                var name = attribute.Name ?? propertyInfo.Name.ToLower();
                var value = propertyInfo.GetValue(this, null);

                // invalid parameter
                if (value == null)
                    continue;

                // convert array to comma-seperated list
                if (value is IEnumerable && value.GetType().GetElementType() == typeof (string))
                    value = string.Join(",", ((IEnumerable) value).Cast<object>().Select(x => x.ToString()).ToArray());

                // convert booleans
                if (value is bool)
                    value = Convert.ToBoolean(value) ? "1" : "0";

                // convert DateTime to UNIX timestamp
                if (value is DateTime)
                    value = (int) ((DateTime) value - new DateTime(1970, 1, 1)).TotalSeconds;

                parameterDict.Add(name, value.ToString());
            }

            return parameterDict;
        }
    }
}