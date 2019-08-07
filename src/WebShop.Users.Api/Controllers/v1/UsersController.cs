using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Users.Common.Dtos.Users;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Queries;
using Microsoft.AspNetCore.Authorization;
using WebShop.Common.Validation;
using WebShop.Messaging;
using Microsoft.AspNetCore.Cors;
using WebShop.Users.Common.Dtos;
using NSwag.Annotations;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Users.Api.Controllers.v1
{
    /// <summary>
    /// User management endpoints
    /// </summary>
    /// <response code="500">Unrecoverable server error</response>
    /// <response code="401">Not athenticated to perform request</response>
    /// <response code="403">Not authorized to perform request</response>
    [Authorize(Policy = "SameUserOrAdmin")]
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

        /// <summary>
        /// Dependency injected CommandHandler instace
        /// </summary>
        protected readonly ICommandDispatcher _commandDispather;

        /// <summary>
        /// Dependency injected QueryHandler instace
        /// </summary>
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
        /// <param name="registerUserCommand"></param>
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
        public virtual async Task<IActionResult> RegisterUser(
            [FromBody][ModelBinder(BinderType = typeof(UserCommandModelBinder))]RegisterUserCommand registerUserCommand)
        {
            await this._commandDispather.HandleAsync<RegisterUserCommand>(registerUserCommand);
            return CreatedAtRoute(routeName: "User", routeValues: new { id = registerUserCommand.Id }, value: registerUserCommand.Id);
        }

        /// <summary>
        /// Get user profile for the ID
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>User profile data</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{userId}", Name = "User")]
        [ProducesResponseType(typeof(UserInfoDetailsViewDto), 200)]
        public virtual async Task<IActionResult> FindUserById([FromRoute, NotEmptyGuid]Guid userId)
        {
            return Ok(await this._queryDispather.HandleAsync<UserGetQuery, UserInfoDetailsViewDto>(new UserGetQuery() { Id = userId }));
        }

        /// <summary>
        /// Query for user profile
        /// </summary>
        /// <param name="userFilterQuery">Query filter values</param>
        /// <returns>Collection of user profiles</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">No user account for filter options</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserInfoDetailsViewDto>), 200)]
        public virtual async Task<IActionResult> FindUsers([FromQuery]UserFilterQuery userFilterQuery)
        {
            return Ok(await this._queryDispather.HandleAsync<UserFilterQuery, IEnumerable<UserInfoDetailsViewDto>>(userFilterQuery));
        }

        /// <summary>
        /// Updates user profile details
        /// </summary>
        /// <param name="profileUpdateCommand">User profile details</param>
        /// <returns>Empty OK response</returns>
        /// <response code="204">User account profile updated</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid update value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [AllowAnonymous]

        public virtual async Task<IActionResult> UpdateUserInfo(
            [FromBody][ModelBinder(BinderType = typeof(UserCommandModelBinder))]UpdateUserInfoCommand profileUpdateCommand)
        {
            await this._commandDispather.HandleAsync<UpdateUserInfoCommand>(profileUpdateCommand);
            return NoContent();
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdateCommand">User password update details</param>
        /// <returns>Empty OK reponse</returns>
        /// <response code="204">User account password updated</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid update value</response>
        /// <response code="404">User account for ID not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut("{userId}/password")]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> UpdateUserPassword(
            [FromBody][ModelBinder(BinderType = typeof(UserCommandModelBinder))]UpdateUserPasswordCommand passwordUpdateCommand)
        {
            await this._commandDispather.HandleAsync<UpdateUserPasswordCommand>(passwordUpdateCommand);
            return NoContent();
        }

        /// <summary>
        /// Sets user profile image
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="file">Binary content of an image sent with key \"photo\" with headers Content-Type: multipart/form-data.\nMaximum file size is 500KB</param>
        /// <returns>No content 204 status code</returns>
        /// <response code="201">Image successfuly set</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPost("{userId}/image")]
        [RequestSizeLimit(524288)]
        [ProducesResponseType(201)]
        [SwaggerIgnore]
        public virtual async Task<IActionResult> SetUserImage(
            [FromRoute, NotEmptyGuid] Guid userId,
            [FromForm(Name = "photo"), AllowedFileTypes(fileTypes: new String[] { ".jpg", ".jpeg" })]IFormFile file)
        {
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
            return File(await _queryDispather.HandleAsync<UserImageGetQuery, byte[]>(new UserImageGetQuery(userId)), "image/jpg");
        }

        /// <summary>
        /// Removes user profile image
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>No content 204 status code</returns>
        /// <response code="204">Image removed</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid ID value</response>
        /// <response code="404">User image not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [ProducesResponseType(204)]
        [HttpDelete("{userId}/image")]
        public virtual async Task<IActionResult> DeleteUserImage([FromRoute, NotEmptyGuid] Guid userId)
        {
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
            var data = await _queryDispather.HandleAsync<UserImageGetQuery, byte[]>(new UserImageGetQuery(userId));
            return Ok(Convert.ToBase64String(data));
        }

        /// <summary>
        /// Retiever all roles user belongs to
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>List of role names user belongs to</returns>
        [HttpGet("{userId}/roles", Name = "Roles")]
        [ProducesResponseType(typeof(String), 200)]
        public virtual async Task<IActionResult> GetUserRoles([FromRoute, NotEmptyGuid]Guid userId)
        {
            var data = await _queryDispather.HandleAsync<UserRolesGetQuery, IEnumerable<String>>(new UserRolesGetQuery() { UserId = userId });
            return Ok(data);
        }

        /// <summary>
        /// Removes user from the role
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="roleName">Role name</param>
        /// <returns>No content 204 status code</returns>
        [HttpDelete("{userId}/roles/{roleName}")]
        [ProducesResponseType(typeof(String), 200)]
        public virtual async Task<IActionResult> RemoveUserRole(
            [FromRoute, NotEmptyGuid]Guid userId,
            [FromRoute, Required] String roleName)
        {
            await _commandDispather.HandleAsync<RemoveUserRoleCommand>(new RemoveUserRoleCommand(userId, roleName));
            return NoContent();
        }

        /// <summary>
        /// Assignes user to the role
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="roleName">Role name</param>
        /// <returns>No content 204 status code</returns>
        [HttpPut("{userId}/roles/{roleName}")]
        [ProducesResponseType(typeof(String), 200)]
        public virtual async Task<IActionResult> AddUserRole(
            [FromRoute, NotEmptyGuid]Guid userId, 
            [FromRoute, Required] String roleName)
        {
            await _commandDispather.HandleAsync<AddUserRoleCommand>(new AddUserRoleCommand(userId, roleName));
            return NoContent();
        }

    }
}
