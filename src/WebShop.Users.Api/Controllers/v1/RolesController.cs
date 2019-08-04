using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.Messaging;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Dtos;
using WebShop.Users.Common.Dtos.Roles;

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
        /// <param name="roleCreate"></param>
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
        public virtual async Task<IActionResult> RegisterUser([FromBody]RoleCreateDto roleCreate)
        {
            roleCreate.Id = roleCreate.Id != Guid.Empty ? roleCreate.Id : Guid.NewGuid();
            await this._commandDispather.HandleAsync<AddRoleCommand>(new AddRoleCommand(roleCreate.Id, roleCreate.Name));
            return CreatedAtRoute(routeName: "Role", routeValues: new { id = roleCreate.Id }, value: roleCreate.Id);
        }

    }
}