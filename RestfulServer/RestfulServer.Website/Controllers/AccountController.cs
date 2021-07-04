using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestfulServer.Core.DataAccess;

namespace RestfulServer.Website.Controllers
{
    [ApiController]
    [Route("[controller]/{customerId}")]
    public class AccountController : Controller
    {
        private readonly ICountableResource _countableResource;

        public AccountController(ICountableResource countableResource)
        {
            _countableResource = countableResource;
        }

        [HttpGet]
        public async Task<IActionResult> GetResource(string customerId)
        {
            try
            {
                var result = await _countableResource.GetValue(customerId);
                return Ok(result.Value);
            }
            catch (ArgumentNullException e)
            {
                return InvalidCustomerId(customerId);
            }
            catch (Exception e)
            {
                return InternalError();
            }
        }

        [HttpPut]
        [Route("[action]/{increment:int?}")]
        public async Task<IActionResult> IncrementResource([FromRoute] string customerId, [FromRoute] int? increment = null)
        {
            // We want to increment by 1 by default
            var incrementSize = increment ?? 1;
            if (incrementSize < 1)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Increment must be a positive integer",
                    Increment = increment
                });
            }

            try
            {
                var dto = await _countableResource.ChangeValueBy(customerId, incrementSize);
                if (dto.Success)
                    return Ok("Resource incremented");
                        
                return dto.ExceptionThrown
                    ? InternalError()
                    : BadRequest(dto.ErrorMessage);
            }
            catch (Exception)
            {
                return InternalError();
            }
        }

        [HttpPut]
        [Route("[action]/{decrement:int?}")]
        public async Task<IActionResult> DecrementResource([FromRoute] string customerId, [FromRoute] int? decrement)
        {
            // We want to increment by 1 by default
            var decrementSize = decrement ?? -1;
            if (decrement > -1)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Decrement must be a negative integer",
                    Increment = decrement
                });
            }

            try
            {
                var dto = await _countableResource.ChangeValueBy(customerId, decrementSize);
                if (dto.Success)
                    return Ok("Resource decremented");
                
                return dto.ExceptionThrown
                    ? InternalError()
                    : BadRequest(dto.ErrorMessage);
            }
            catch (Exception)
            {
                return InternalError();
            }
        }
        
        private BadRequestObjectResult InvalidCustomerId(string customerId)
        {
            return BadRequest(new
            {
                ErrorMessage = "The customerId is not valid",
                CustomerId = customerId
            });
        }
        
        private ObjectResult InternalError()
        {
            return StatusCode((int) HttpStatusCode.InternalServerError,
                "Something went wrong on the server. Contact support");
        }
    }
}