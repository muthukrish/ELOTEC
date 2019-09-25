namespace ELOTEC.Controllers
{
    using ELOTEC.Infrastructure.Constants;
    using ELOTEC.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Collections.Generic;

    [Route(Routes.Test)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [SwaggerOperation(Summary = SwaggerSummary.List, Tags = new[] { SwaggerTag.Test })]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = SwaggerSummary.Get, Tags = new[] { SwaggerTag.Test })]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [SwaggerOperation(Summary = SwaggerSummary.Post, Tags = new[] { SwaggerTag.Test })]
        public ActionResult<string> Post([FromBody] string value)
        {
            return value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = SwaggerSummary.Put, Tags = new[] { SwaggerTag.Test })]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = SwaggerSummary.Delete, Tags = new[] { SwaggerTag.Test })]
        public void Delete(int id)
        {
        }
    }
}
