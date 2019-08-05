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
        /// <param name="roleFilterQuery">Role name</param>
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
        public virtual async Task<IActionResult> FindRoleByName(
            [FromRoute, Required][ModelBinder(BinderType = typeof(RoleCommandModelBinder))]RoleGetQuery roleFilterQuery)
        {
            return Ok(await this._queryDispather.HandleAsync<RoleGetQuery, RoleViewDto>(roleFilterQuery));
        }

        /// <summary>
        /// Get role claims
        /// </summary>
        /// <param name="getRoleClaimsQuery">Role name</param>
        /// <returns>Role details</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpGet("{roleName}/claims")]
        [ProducesResponseType(typeof(RoleViewDto), 200)]
        public virtual async Task<IActionResult> GetRoleClaims(
            [FromRoute, Required][ModelBinder(BinderType = typeof(RoleCommandModelBinder))] RoleGetClaimsQuery getRoleClaimsQuery)
        {
            return Ok(await this._queryDispather.HandleAsync<RoleGetClaimsQuery, IEnumerable<RoleClaimViewDto>>(getRoleClaimsQuery));
        }

        /// <summary>
        /// Adds claim to a role
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <param name="roleClaimCommand">Claim to be assigned to name</param>
        /// <returns>Role details</returns>
        /// <response code="200">User account details</response>
        /// <response code="401">Not authenticated to perform request</response>
        /// <response code="403">Not authorized to perform request</response>
        /// <response code="400">Invalid role name value</response>
        /// <response code="404">Role not found</response>
        /// <response code="500">Unrecoverable server error</response>
        [HttpPut("{roleName}/claims")]
        [ProducesResponseType(typeof(RoleViewDto), 200)]
        public virtual async Task<IActionResult> AddRoleClaim([FromRoute, Required]String roleName,
            [ModelBinder(BinderType = typeof(RoleCommandModelBinder))]AddRoleClaimCommand roleClaimCommand)
        {
            await this._commandDispather.HandleAsync<AddRoleClaimCommand>(roleClaimCommand);
            return Ok();
        }


    }
}