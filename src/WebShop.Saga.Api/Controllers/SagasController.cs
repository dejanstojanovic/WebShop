using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Messaging.Saga;

namespace WebShop.Saga.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Guid>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<ISagaState> Get(Guid sagaId)
        {
            throw new NotImplementedException();
        }

    }
}
