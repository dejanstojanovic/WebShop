using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Users.Api.Clients.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebShop.Web.Mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Web.Mvc.Controllers
{
    [Authorize]
    public class AccountsController : AbstractController
    {
        public AccountsController(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {
        }

        public async Task<IActionResult> Index([FromQuery]UserFilterViewModel filter)
        {
            var apiClient = new UsersClient(Configuration.GetValue<String>("UsersApi"), GetHttpClient());
            var users = await apiClient.FindUsersAsync(
                firstName: "Dejan", 
                lastName:"", 
                email:"d.stojanovic@hotmail.com",
                pageIndex:0,
                pageSize:10,
                occupation:"", 
                education:"",
                dateOfBirth:null
                );

            var user = users?.FirstOrDefault();
            if (user != null)
            {
                await apiClient.UpdateUserInfoAsync(
                    new UpdateUserInfoCommand()
                    {
                        DateOfBirth = user.DateOfBirth,
                        Education=user.Education,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Occupation = "IT master - level 1"
                    }
                    , user.Id.ToString());
            }

            return View("Index");
        }


    }
}