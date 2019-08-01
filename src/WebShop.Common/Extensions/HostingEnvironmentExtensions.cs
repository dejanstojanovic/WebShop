using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebShop.Common.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalhost(this IHostingEnvironment environment)
        {
            return environment.EnvironmentName.Equals("localhost", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsWindows(this IHostingEnvironment environment)
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}
