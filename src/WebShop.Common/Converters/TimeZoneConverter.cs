using WebShop.Common.Exceptions;
using WebShop.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WebShop.Common.Converters
{
    class TimeZoneMapping
    {
        public String Linux { get; set; }
        public String Windows { get; set; }
    }

    public static class TimeZoneConverter
    {

        static IEnumerable<TimeZoneMapping> zoneMappings =
            JsonConvert.DeserializeObject<IEnumerable<TimeZoneMapping>>(
            new StreamReader(
            typeof(TimeZoneConverter).Assembly.GetManifestResourceStream($"{typeof(TimeZoneConverter).Namespace}.{typeof(TimeZoneConverter).Name}.TimeZones.json"))
           .ReadToEnd());

        /// <summary>
        /// Operating system agnostic method for parsing time zone name http://bit.ly/2Kurqhv
        /// </summary>
        /// <param name="timeZoneId">Time zone name to be parsed</param>
        /// <returns>TimeZoneInfo instance</returns>
        public static TimeZoneInfo ToTimeZoneInfo(String timeZoneId)
        {
            TimeZoneMapping mapping = zoneMappings.FirstOrDefault(z =>
            z.Windows.Equals(timeZoneId, StringComparison.CurrentCultureIgnoreCase) ||
            z.Linux.Equals(timeZoneId, StringComparison.CurrentCultureIgnoreCase));

            if (mapping == null)
            {
                throw new NotFoundException($"Time zone with ID \"{timeZoneId}\" does not exist");
            }
            return TimeZoneInfo.FindSystemTimeZoneById(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? mapping.Windows : mapping.Linux);
        }
    }
}
