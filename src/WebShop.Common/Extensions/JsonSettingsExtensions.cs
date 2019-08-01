using WebShop.Common.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Common.Extensions
{

    public static class JsonSettingsExtensions
    {
        /// <summary>
        /// Custom serialization handler settings to avoid parsing failure of 0001-01-01T00:00:00
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureJsonSettings(this IServiceCollection services)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateParseHandling = DateParseHandling.None
            };
            settings.Converters.Add(new CustomDateTimeConverter(defaultTimeZoneId: "UTC"));
            JsonConvert.DefaultSettings = () => settings;

        }
    }
}
