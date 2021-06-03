namespace ESOrderDemo.Controllers
{
    using Elasticsearch.Net;
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : ControllerBase
    {
        protected IActionResult GetResult(StringResponse resp)
        {
            if (resp.Success)
            {
                return Ok(resp.Body);
            }
            else
            {
                return StatusCode(resp.HttpStatusCode ?? 400, resp.Body);
            }
        }
    }
}
