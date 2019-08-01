using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebShop.Common.Extensions;
using WebShop.Common.Converters;

namespace WebShop.Common.Serialization
{
    public class CustomDateTimeConverter: IsoDateTimeConverter
    {
        private readonly string defaultTimeZoneId;
        public CustomDateTimeConverter(string defaultTimeZoneId)
        {
            this.defaultTimeZoneId = defaultTimeZoneId;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(DateTimeOffset) && objectType != typeof(DateTimeOffset?))
                return base.ReadJson(reader, objectType, existingValue, serializer);

            var dateText = reader.Value.ToString();
            if (objectType == typeof(DateTimeOffset?) && string.IsNullOrEmpty(dateText))
                return null;

            if (dateText.IndexOfAny(new[] { 'Z', 'z', '+' }) == -1 && dateText.Count(c => c == '-') == 2)
            {
                var dateTime = DateTime.Parse(dateText);
                var timeZone = TimeZoneConverter.ToTimeZoneInfo(this.defaultTimeZoneId);
                var offset = timeZone.GetUtcOffset(dateTime);
                return new DateTimeOffset(dateTime, offset);
            }
            return DateTimeOffset.Parse(dateText);
        }
    }
}
