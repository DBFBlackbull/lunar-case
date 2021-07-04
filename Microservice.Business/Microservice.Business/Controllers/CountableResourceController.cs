using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Business.Controllers
{
    [ApiController]
    [Route("[controller]/{id}")]
    public class CountableResourceController : Controller
    {
        private static readonly IDictionary<string, int> Dictionary;

        static CountableResourceController()
        {
            Dictionary = new Dictionary<string, int>();
        }

        [HttpGet]
        public IActionResult GetValue(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id is not valid");

            Dictionary.TryGetValue(id, out var value);
            return Ok(value.ToString());
        }

        [HttpPut]
        [Route("ChangeValueBy/{value:int}")]
        public IActionResult ChangeValue(string id, int value)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id is not valid");

            if (value == 0)
                return BadRequest("Value is 0");
            
            if (!Dictionary.ContainsKey(id))
                Dictionary[id] = 0;

            Dictionary[id] += value;
            return Ok("Value updated");
        }
    }
}