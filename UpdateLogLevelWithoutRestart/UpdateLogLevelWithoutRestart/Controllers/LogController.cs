using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nacos.V2;
using System.Threading.Tasks;

namespace UpdateLogLevelWithoutRestart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly INacosConfigService _configSvc;

        public LogController(ILogger<LogController> logger, INacosConfigService configSvc)
        {
            _logger = logger;
            _configSvc = configSvc;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogTrace("trace");
            _logger.LogDebug("debug");
            _logger.LogInformation("info");
            _logger.LogWarning("warning");
            _logger.LogError("error");
            _logger.LogCritical("critical");

            return Ok("OK");
        }

        [HttpGet("ul")]
        public async Task<IActionResult> UpdateLogLevel([FromQuery]string level)
        {
            var l = level switch
            {
                "d" => "Debug",
                "t" => "Trace",
                "i" => "Information",
                "w" => "Warning",
                "e" => "Error",
                _ => "Debug",
            };

            /*
             
{
	"Logging": {
		"LogLevel": {
			"Default": "Warning",
			"Microsoft": "Warning",
			"Microsoft.Hosting.Lifetime": "Information"
		}
	}
}
             */
            var config = "{ \n" +
                "\"Logging\": { \n" +
                    "\"LogLevel\": { \n" +
                        "\"Default\": \""+ l +"\", \n" +
                        "\"Microsoft\": \"Warning\", \n" +
                        "\"Microsoft.Hosting.Lifetime\": \"Information\" \n" +
                        "} \n" +
                    "} \n" +
                "} ";

            await _configSvc.PublishConfig("common", "DEFAULT_GROUP", config, "json").ConfigureAwait(false);

            return Ok("OK");
        }
    }
}
