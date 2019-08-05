using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.Common.Validation;
using WebShop.Messaging;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Dtos;
using WebShop.Users.Common.Dtos.Roles;
using WebShop.Users.Common.Queries;

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
    public class RolesController : ControllerBase
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
        public RolesController(
            ICommandDispatcher commandDispatcher = null,
            IQueryDispatcher queryDispatcher = null
            )
        {
            this._commandDispather = commandDispatcher;
            this._queryDispather = queryDispatcher;
        }

        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="createRoleCommand"></param>
        /// <returns>Role fetch URL in headers</returns>
        /// <response code="201">User account created</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid data</response>
        /// <response code="409">Role alredy exists</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Guid), 201)]
        public virtual async Task<IActionResult> CreateRole([FromBody][ModelBinder(BinderType = typeof(RoleCommandModelBinder))]CreateRoleCommand createRoleCommand)
        {
            await this._commandDispather.HandleAsync<CreateRoleCommand>(createRoleCommand);
            return CreatedAtRoute(routeName: "Role", routeValues: new { id = createRoleCommand.Id }, value: createRoleCommand.Id);
        }

        /// <summary>
        /// Get role by name
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role details</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{roleName}", Name = "Role")]
        [ProducesResponseType(typeof(RoleViewDto), 200)]
        [AllowAnonymous]
        public virtual async Task<IActionResult> FindRoleByName([FromRoute] String roleName)
        {
            return Ok(await this._queryDispather.HandleAsync<RoleGetQuery, RoleViewDto>(new RoleGetQuery() { RoleName=roleName}));
        }

        /// <summary>
        /// Removes role
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Empty response</returns>
        /// <response code="204">Empty response</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpDelete("{roleName}")]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> DeleteRole(
            [FromRoute, Required]String roleName)
        {
            await this._commandDispather.HandleAsync<RemoveRoleCommand>(new RemoveRoleCommand(roleName));
            return NoContent();
        }

        /// <summary>
        /// Get role claims
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of role claims</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{roleName}/claims")]
        [ProducesResponseType(typeof(IEnumerable<RoleClaimViewDto>), 200)]
        public virtual async Task<IActionResult> GetRoleClaims(
            [FromRoute, Required]String roleName)
        {
            return Ok(await this._queryDispather.HandleAsync<RoleGetClaimsQuery, IEnumerable<RoleClaimViewDto>>(new RoleGetClaimsQuery() { RoleName=roleName}));
        }

        /// <summary>
        /// Adds claim to a role
        /// </summary>
        /// <param name="addRoleClaimCommand">Role claim object</param>
        /// <returns>Empty response</returns>
        /// <response code="204">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut("{roleName}/claims")]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> AddRoleClaim(
            [FromBody, ModelBinder(BinderType = typeof(RoleCommandModelBinder))]AddRoleClaimCommand addRoleClaimCommand)
        {
            await this._commandDispather.HandleAsync<AddRoleClaimCommand>(addRoleClaimCommand);
            return NoContent();
        }

        /// <summary>
        /// Removes claim from a role with specific name
        /// </summary>
        /// <param name="removeRoleClaimCommand">Claim to be removed from the role</param>
        /// <returns>Empty response</returns>
        /// <response code="204">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpDelete("{roleName}/claims")]
        [ProducesResponseType(204)]
        public virtual async Task<IActionResult> AddRoleClaim(
            [FromBody,ModelBinder(BinderType = typeof(RoleCommandModelBinder))]RemoveRoleClaimCommand removeRoleClaimCommand)
        {
            await this._commandDispather.HandleAsync<RemoveRoleClaimCommand>(removeRoleClaimCommand);
            return NoContent();
        }


    }
}