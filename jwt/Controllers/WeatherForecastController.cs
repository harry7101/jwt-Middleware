using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServcieTestA _serviceTestA;
        private readonly IServcieTestA _serviceTestAU;
        private readonly IServcieTestB _serviceTestb;

        private  IServcieTestC _serviceTestc;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IServcieTestA serviceTestA, 
            IServcieTestB serviceTestB,
             IServcieTestC serviceTestC,
             ServiceTestAU serviceTestAU
            )
        {
            _logger = logger;
            _serviceTestA = serviceTestA;
            _serviceTestb = serviceTestB;
            _serviceTestc = serviceTestC;
            _serviceTestAU = serviceTestAU;
        }

        [HttpGet("Getmsg")]
        public string Getmsg()
        {
            _logger.LogInformation($"_serviceTestb.show():{_serviceTestb.show("")},_serviceTestb.showServiceA(){_serviceTestb.showServiceA("showServiceA")}");
            _logger.LogInformation($"_serviceTestc.serviceA.show{_serviceTestc.servcieTestA.show("aaa")}");
            _logger.LogInformation($"_serviceTestc.serviceA.show,{_serviceTestAU.show("_serviceTestAU")}");
            return _serviceTestA.show("IServcieTestA.show");
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var s = UserId;
            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
