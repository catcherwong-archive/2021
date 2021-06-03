namespace ESOrderDemo.Controllers
{
    using Elasticsearch.Net;
    using Microsoft.AspNetCore.Mvc;
    using Nest;
    using System;

    [ApiController]
    [Route("sem")]
    public class SemController : BaseController
    {
        private readonly IElasticClient _client;

        public SemController(IElasticClient client)
        {
            _client = client;
        }

        [HttpPost("{index}/search")]
        public IActionResult Search(string index, [FromBody] string postData)
        {
            var resp = _client.LowLevel.Search<StringResponse>(index, PostData.String(postData));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpPost("{index}/_doc")]
        public IActionResult Doc(string index, [FromBody] string postData)
        {
            var resp = _client.LowLevel.Index<StringResponse>(index, PostData.String(postData));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpPost("_bulk")]
        public IActionResult Bulk([FromBody] string postData)
        {
            var resp = _client.LowLevel.Bulk<StringResponse>(PostData.String(postData));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpPost("{index}/_update_by_query")]
        public IActionResult UpdateByQuery(string index, [FromBody] string postData)
        {
            var resp = _client.LowLevel.UpdateByQuery<StringResponse>(index, PostData.String(postData));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpPost("{index}/_delete_by_query")]
        public IActionResult DeleteByQuery(string index, [FromBody] string postData)
        {
            var resp = _client.LowLevel.DeleteByQuery<StringResponse>(index, PostData.String(postData));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }
    }
}
