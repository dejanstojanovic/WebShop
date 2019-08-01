using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Users.Contracts.ApplicationUser;
using WebShop.Users.Contracts;
using WebShop.Users.AppServices;
using WebShop.Users.AppServices.Commands;
using WebShop.Users.AppServices.Queries;
using AutoMapper;
using WebShop.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using WebShop.Common.Validation;
using WebShop.Messaging;
using Microsoft.AspNetCore.Cors;

namespace WebShop.Users.Api.Controllers.v2
{
    /// <summary>
    /// User management endpoints
    /// </summary>
    /// <response code="500">Unrecoverable server error</response>
    /// <response code="401">Not athenticated to perform request</response>
    /// <response code="403">Not authorized to perform request</response>
    [Authorize("UserIdPolicy")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [ProducesResponseType(typeof(ErrorMessage), 500)]
    [ProducesResponseType(typeof(ErrorMessage), 404)]
    [ProducesResponseType(typeof(ErrorMessage), 409)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public class UsersController : v1.UsersController
    {

    }
}
