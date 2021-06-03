namespace ESOrderDemo.Controllers
{
    using Elasticsearch.Net;
    using Microsoft.AspNetCore.Mvc;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("order")]
    public class OrderController : BaseController
    {
        private readonly IElasticClient _client;

        public OrderController(IElasticClient client)
        {
            _client = client;
        }

        [HttpGet("sample")]
        public IActionResult Sample()
        {
            return Ok("order with es");
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="dto">订单信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var order = new
            {
                order_id = Guid.NewGuid().ToString("N"),
                cus_name = dto.CusName,
                item_id = dto.ItemId,
                item_name = dto.ItemName,
                number = dto.Number,
                create_time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                update_time = 0
            };

            var resp = await _client.LowLevel.IndexAsync<StringResponse>("order-2021", PostData.Serializable(order));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        /// <summary>
        /// 根据 order_id 获取订单详情
        /// </summary>
        /// <param name="order_id">订单Id</param>
        /// <returns></returns>
        [HttpGet("{order_id}")]
        public async Task<IActionResult> GetById(string order_id)
        {
            var q = new
            {
                query = new 
                {
                    term = new { order_id = order_id }
                }
            };

            var resp = await _client.LowLevel.SearchAsync<StringResponse>("order", PostData.Serializable(q));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        /// <summary>
        /// 根据 order_id 删除订单
        /// </summary>
        /// <param name="order_id">订单Id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string order_id)
        {
            var q = new
            {
                query = new
                {
                    term = new { order_id = order_id }
                }
            };

            var resp = await _client.LowLevel.DeleteByQueryAsync<StringResponse>("order", PostData.Serializable(q));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        /// <summary>
        /// 根据用户姓名，分页查询订单(浅分页，from + size)
        /// </summary>
        /// <param name="cus_name">用户姓名</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IActionResult> GetForCus([FromQuery] string cus_name, [FromQuery] int pageIndex = 1, [FromQuery]int pageSize = 10)
        {
            var q = new
            {
                query = new
                {
                    term = new { cus_name = cus_name }
                },
                size = pageSize,
                from = (pageIndex - 1) * pageSize,
                sort = new List<object> { new { create_time = new { order = "desc" } } }
            };

            var resp = await _client.LowLevel.SearchAsync<StringResponse>("order", PostData.Serializable(q));

#if DEBUG
            Console.WriteLine(resp.DebugInformation);
#endif

            return GetResult(resp);
        }

        public class CreateOrderDto
        { 
            /// <summary>
            /// 用户
            /// </summary>
            public string CusName { get; set; }

            /// <summary>
            /// 商品Id
            /// </summary>
            public string ItemId { get; set; }

            /// <summary>
            /// 商品名
            /// </summary>
            public string ItemName { get; set; }

            /// <summary>
            /// 购买数量
            /// </summary>
            public int Number { get; set; }
        }
    }
}
