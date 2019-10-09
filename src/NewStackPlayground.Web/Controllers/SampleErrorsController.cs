using System;
using Microsoft.AspNetCore.Mvc;
using NewStackPlayground.Gateway.Commands;
using NewStackPlayground.Gateway.Queries;

namespace NewStackPlayground.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleErrorsController : GatewayController
    {
        [HttpGet("unknown")]
        public ActionResult<object> GetUnknown()
        {
            return HandleQuery<ThrowSampleUnknownErrorQuery, object>(new ThrowSampleUnknownErrorQuery());
        }

        [HttpPost("unknown")]
        public ActionResult<object> PostUnknown([FromBody] ThrowSampleUnknownErrorCommand command)
        {
            return HandleCommand<ThrowSampleUnknownErrorCommand, object>(command);
        }

        [HttpGet("known")]
        public ActionResult<object> GetKnown()
        {
            return HandleQuery<ThrowSampleKnownErrorQuery, object>(new ThrowSampleKnownErrorQuery());
        }

        [HttpPost("known")]
        public ActionResult<object> PostKnown([FromBody] ThrowSampleKnownErrorCommand command)
        {
            return HandleCommand<ThrowSampleKnownErrorCommand, object>(command);
        }

        [HttpGet("nongateway")]
        public ActionResult<object> GetNonGateway()
        {
            throw new Exception("Sample non-gateway error.");
        }
    }
}
