using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Users.Common.Dtos.ApplicationUser;
using WebShop.Users.Common;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Queries;
using AutoMapper;
using WebShop.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using WebShop.Common.Validation;
using WebShop.Messaging;
using Microsoft.AspNetCore.Cors;
using WebShop.Users.Common.Dtos;
using NSwag.Annotations;
using System.IO;
using Microsoft.AspNetCore.Http;

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
    [ProducesResponseType(typeof(ErrorMessageDto), 500)]
    [ProducesResponseType(typeof(ErrorMessageDto), 404)]
    [ProducesResponseType(typeof(ErrorMessageDto), 409)]
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
        public virtual async Task<IActionResult> RegisterUser([FromBody]UserRegisterDto userRegister)
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
        [ProducesResponseType(typeof(UserInfoDetailsViewDto), 200)]
        public virtual async Task<IActionResult> FindUserById([FromRoute, NotEmptyGuid]Guid id)
        {
            return Ok(await this._queryDispather.HandleAsync<ProfileGetQuery, UserInfoDetailsViewDto>(new ProfileGetQuery() { Id = id }));
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
        [ProducesResponseType(typeof(IEnumerable<UserInfoDetailsViewDto>), 200)]
        public virtual async Task<IActionResult> FindUsers([FromQuery]UserInfoViewDto profileBrowse)
        {
            return Ok(await this._queryDispather.HandleAsync<ProfileBrowseQuery, IEnumerable<UserInfoDetailsViewDto>>(new ProfileBrowseQuery(profileBrowse)));
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
        public virtual async Task<IActionResult> UpdateUserInfo([FromBody]UserInfoUpdateDto profileUpdate)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await this._commandDispather.HandleAsync<UpdateUserInfoCommand>(new UpdateUserInfoCommand(userId, profileUpdate));
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
        public virtual async Task<IActionResult> UpdateUserPassword([FromBody]UserPasswordUpdateDto passwordUpdate)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await this._commandDispather.HandleAsync<UpdateUserPasswordCommand>(new UpdateUserPasswordCommand(userId, passwordUpdate));
            return NoContent();
        }

        /// <summary>
        /// Sets user profile image
        /// </summary>
        /// <param name="file">Binary content of an image sent with key \"photo\" with headers Content-Type: multipart/form-data.\nMaximum file size is 500KB</param>
        /// <returns>No content 204 status code</returns>
        /// <response code="201">Image successfuly set</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPost("image")]
        [RequestSizeLimit(524288)]
        [ProducesResponseType(201)]
        [SwaggerIgnore]
        public virtual async Task<IActionResult> SetUserImage([FromForm(Name = "photo"), AllowedFileTypes(fileTypes: new String[] { ".jpg", ".jpeg" })]IFormFile file)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                await _commandDispather.HandleAsync<SetUserImageCommand>(new SetUserImageCommand(userId, memoryStream.ToArray()));
            }
            return CreatedAtRoute(routeName: "Image", routeValues: new { userId = userId.ToString() }, value: null); ;
        }


        /// <summary>
        /// Get user profile image content
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>User profile image in base64 format</returns>
        /// <response code="200">User account image file</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{userId}/image", Name = "Image")]
        [SwaggerResponse(200, typeof(FileContentResult))]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public virtual async Task<IActionResult> GetUserImage([FromRoute, NotEmptyGuid]Guid userId)
        {
            return File(await _queryDispather.HandleAsync<ProfileImageGetQuery, byte[]>(new ProfileImageGetQuery(userId)), "image/jpg");
        }

        /// <summary>
        /// Removes user profile image
        /// </summary>
        /// <returns>No content 204 status code</returns>
        /// <response code="204">Image removed</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [ProducesResponseType(204)]
        [HttpDelete("/image")]
        public virtual async Task<IActionResult> DeleteUserImage()
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await _commandDispather.HandleAsync<RemoveUserImageCommand>(new RemoveUserImageCommand(userId));
            return NoContent();
        }

        /// <summary>
        /// Get user profile image in base64 string format
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>User profile image in base64 format</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{userId}/image/base64", Name = "Base64")]
        [ProducesResponseType(typeof(String), 200)]
        public virtual async Task<IActionResult> GetUserImageBase64([FromRoute, NotEmptyGuid]Guid userId)
        {
            var data = await _queryDispather.HandleAsync<ProfileImageGetQuery, byte[]>(new ProfileImageGetQuery(userId));
            return Ok(Convert.ToBase64String(data));
        }

    }
}
