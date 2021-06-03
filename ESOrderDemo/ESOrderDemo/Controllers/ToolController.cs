namespace ESOrderDemo.Controllers
{
    using Elasticsearch.Net;
    using Microsoft.AspNetCore.Mvc;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("tool")]
    public class ToolController : BaseController
    {
        private readonly IElasticClient _client;

        public ToolController(IElasticClient client)
        {
            _client = client;
        }

        [HttpGet("drop")]
        public IActionResult Drop()
        {
            var resp = _client.LowLevel.Indices.Delete<StringResponse>("order-2021");

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var resp = _client.LowLevel.Indices.Create<StringResponse>("order-2021", PostData.Serializable(new
            {
                settings = new { number_of_shards = 1 },
                mappings = new
                {
                    properties = new
                    {
                        order_id = new { type = "keyword" },
                        cus_name = new { type = "keyword" },
                        item_id = new { type = "keyword" },
                        item_name = new { type = "text", fields = new { keyword = new { type = "keyword", ignore_above = 256 } } },
                        number = new { type = "integer" },
                        create_time = new { type = "long" },
                        update_time = new { type = "long" },
                    }
                },
            }));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        [HttpGet("show")]
        public IActionResult Show()
        {
            var resp = _client.LowLevel.Indices.GetMapping<StringResponse>("order-2021");
#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif
            return GetResult(resp);
        }

        [HttpGet("aliases")]
        public IActionResult Aliases()
        {
            var resp = _client.LowLevel.Indices.BulkAliasForAll<StringResponse>(PostData.Serializable(new
            {
                actions = new List<object>
                {
                   new  { add  = new  { index  = "order-2021", alias = "order" } }
                }
            }));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif
            return GetResult(resp);
        }

        [HttpGet("bulk-add")]
        public IActionResult BulkInsert()
        {
            var list = BuildData();

            var indexName = "order-2021";

            var bulkRequest = list.SelectMany(m =>
                new[]
                {
                    _client.SourceSerializer.SerializeToString(new { index = new { _index = indexName } }, SerializationFormatting.None),
                    _client.SourceSerializer.SerializeToString(m, SerializationFormatting.None)
                });

            var resp = _client.LowLevel.Bulk<StringResponse>(PostData.String(string.Join("\n", bulkRequest) + "\n"));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif
            return GetResult(resp);
        }

        private IEnumerable<object> BuildData()
        {
            var list = Enumerable.Range(0, 10).Select(x => new
            {
                order_id = Guid.NewGuid().ToString("N"),
                cus_name = $"catcher-{new Random().Next(1, 99999)}",
                item_id = $"123-{new Random().Next(1, 99999)}",
                item_name = $"foo-{new Random().Next(1, 99999)}",
                number = new Random().Next(1,10),
                create_time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                update_time = 0,
            });

            return list;
        }
    }
}
