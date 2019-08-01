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

namespace WebShop.Users.Api.Controllers.v1
{
    /// <summary>
    /// User management endpoints
    /// </summary>
    /// <response code="500">Unrecoverable server error</response>
    /// <response code="401">Not athenticated to perform request</response>
    /// <response code="403">Not authorized to perform request</response>
    [Authorize("UserIdPolicy")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [ProducesResponseType(typeof(ErrorMessage), 500)]
    [ProducesResponseType(typeof(ErrorMessage), 404)]
    [ProducesResponseType(typeof(ErrorMessage), 409)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public class UsersController : ControllerBase
    {

        protected readonly ICommandDispatcher _commandDispather;
        protected readonly IQueryDispatcher _queryDispather;

        /// <summary>
        /// Controller constructor
        /// </summary>
        public UsersController(
            ICommandDispatcher commandDispatcher = null,
            IQueryDispatcher queryDispatcher = null
            )
        {
            this._commandDispather = commandDispatcher;
            this._queryDispather = queryDispatcher;

        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns>User fetch URL in headers</returns>
        /// <response code="201">User account created</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid data</response>
        /// <response code="409">User with same ID or email alredy exists</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Guid), 201)]
        public virtual async Task<IActionResult> Register([FromBody]Register userRegister)
        {
            userRegister.Id = userRegister.Id != Guid.Empty ? userRegister.Id : Guid.NewGuid();
            await this._commandDispather.HandleAsync<RegisterUserCommand>(new RegisterUserCommand(userRegister));
            return CreatedAtRoute(routeName: "User", routeValues:new { id = userRegister.Id }, value: userRegister.Id);
        }

        /// <summary>
        /// Get user profile for the ID
        /// </summary>
        /// <param name="id">Unique user identifier</param>
        /// <returns>User profile data</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{id}", Name = "User")]
        [ProducesResponseType(typeof(ProfileView), 200)]
        public virtual async Task<IActionResult> GetById([FromRoute, NotEmptyGuid]Guid id)
        {
            return Ok(await this._queryDispather.HandleAsync<ProfileGetQuery, ProfileView>(new ProfileGetQuery() { Id = id }));
        }


        /// <summary>
        /// Query for user profile
        /// </summary>
        /// <param name="profileBrowse">Query filter values</param>
        /// <returns>Collection of user profiles</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">No user account for filter options</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProfileView>), 200)]
        public virtual async Task<IActionResult> GetUsers([FromQuery]ProfileBrowse profileBrowse)
        {
            return Ok(await this._queryDispather.HandleAsync<ProfileBrowseQuery, IEnumerable<ProfileView>>(new ProfileBrowseQuery(profileBrowse)));
        }

        /// <summary>
        /// Updates user profile details
        /// </summary>
        /// <param name="profileUpdate">User profile details</param>
        /// <returns>Empty OK response</returns>
        /// <response code="204">User account profile updated</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid update value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> UpdateProfile([FromBody]ProfileUpdate profileUpdate)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await this._commandDispather.HandleAsync<UpdateProfileCommand>(new UpdateProfileCommand(userId, profileUpdate));
            return NoContent();
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdate">User password update details</param>
        /// <returns>Empty OK reponse</returns>
        /// <response code="204">User account password updated</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid update value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut("password")]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> UpdatePassword([FromBody]PasswordUpdate passwordUpdate)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await this._commandDispather.HandleAsync<UpdatePasswordCommand>(new UpdatePasswordCommand(userId, passwordUpdate));
            return NoContent();
        }


    }
}
