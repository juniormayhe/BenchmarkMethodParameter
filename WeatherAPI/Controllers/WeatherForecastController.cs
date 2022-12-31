using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetWithoutAnyArgument")]
        public IEnumerable<WeatherForecast> GetWithoutAnyArgument()
        {
            // for the sake of comparing results, the example of call to API without any arguments
            const int MAX_RECORDS = 50;
            return Enumerable.Range(1, MAX_RECORDS).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetWithPrimitiveArgument")]
        public IEnumerable<WeatherForecast> GetWithPrimitiveArgument(int maxRecords)
        {
            // for the sake of comparing results, the example of call to API without using a complex type
            return Enumerable.Range(1, maxRecords).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Receives a complex type like a http context parameter.
        /// </summary>
        /// <param name="httpContext">Non primivite object just for example purpose. This can be any class type you have set for dealing with search request.</param>
        /// <remarks>
        /// - In real world apps you would instead avoid this and get the current 
        ///   Http context with DefaultHttpContext which is the default implementation 
        ///   of the HttpContext abstract class.
        /// </remarks>
        /// <returns>IEnumerable of WeatherForecast</returns>
        [HttpGet("GetWithComplexArgument")]
        public IEnumerable<WeatherForecast> GetWithComplexArgument(HttpContext httpContext)
        {
            // here we give a purpose for using HttpContext, like picking from client how many records must be fetched.
            // anyway it is better to totally avoid this approach and use instead a specific int parameter or custom class type to configure the client request.
            httpContext.Request.Headers.TryGetValue("MaxRecords", out StringValues values);
            
            int maxRecords = Convert.ToInt32(values.First());
            
            return Enumerable.Range(1, maxRecords).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}