using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Common.Validation;
using WebShop.Users.AppServices;
using WebShop.Users.AppServices.Commands;
using WebShop.Users.AppServices.Queries;
using WebShop.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using WebShop.Messaging;
using Microsoft.AspNetCore.Cors;

namespace WebShop.Users.Api.Controllers.v1
{
    /// <summary>
    /// User profile image management endpoints
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
    public class ImagesController : Controller
    {
        protected readonly ICommandDispatcher _commandDispather;
        protected readonly IQueryDispatcher _queryDispather;

        /// <summary>
        /// Controller constructor
        /// </summary>
        public ImagesController(
            ICommandDispatcher commandDispatcher = null,
            IQueryDispatcher queryDispatcher = null
            )
        {
            this._commandDispather = commandDispatcher;
            this._queryDispather = queryDispatcher;

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
        [HttpGet("{userId}", Name = "Image")]
        [SwaggerResponse(200, typeof(FileContentResult))]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public virtual async Task<IActionResult> GetImage([FromRoute, NotEmptyGuid]Guid userId)
        {
            return File(await _queryDispather.HandleAsync<ProfileImageGetQuery, byte[]>(new ProfileImageGetQuery(userId)),"image/jpg");
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
        [HttpGet("{userId}/base64", Name = "Base64")]
        [ProducesResponseType(typeof(String), 200)]
        public virtual async Task<IActionResult> GetBase64([FromRoute, NotEmptyGuid]Guid userId)
        {
            var data = await _queryDispather.HandleAsync<ProfileImageGetQuery, byte[]>(new ProfileImageGetQuery(userId));
            return Ok(Convert.ToBase64String(data));
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
        [HttpDelete]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> Delete()
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            await _commandDispather.HandleAsync<ProfileImageRemoveCommand>(new ProfileImageRemoveCommand(userId));
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
        [HttpPost]
        [RequestSizeLimit(524288)]
        [ProducesResponseType(201)]
        [SwaggerIgnore]
        public virtual async Task<IActionResult> PostImage([FromForm(Name = "photo"), AllowedFileTypes(fileTypes: new String[] { ".jpg", ".jpeg" })]IFormFile file)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type.Equals("userid")).Value);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                await _commandDispather.HandleAsync<ProfileImageSetCommand>(new ProfileImageSetCommand(userId, memoryStream.ToArray()));
            }

            return CreatedAtRoute(routeName: "Image", routeValues: new { userId = userId.ToString() }, value: null); ;
        }

    }
}