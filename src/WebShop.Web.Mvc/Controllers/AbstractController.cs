using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebShop.Web.Mvc.Controllers
{
    public abstract class AbstractController : Controller
    {
        readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public AbstractController(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected IConfiguration Configuration => _configuration;
        protected IServiceProvider ServiceProvider => _serviceProvider;

        protected HttpClient GetHttpClient()
        {
            return _serviceProvider.GetService(typeof(HttpClient)) as HttpClient;
        }
    }
}
