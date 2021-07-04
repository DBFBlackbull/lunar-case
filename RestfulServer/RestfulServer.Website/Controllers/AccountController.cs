using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestfulServer.Core.DataAccess;

namespace RestfulServer.Website.Controllers
{
    [ApiController]
    [Route("[controller]/{customerId}/[action]")]
    public class AccountController : Controller
    {
        private readonly ICountableResource _countableResource;

        public AccountController(ICountableResource countableResource)
        {
            _countableResource = countableResource;
        }

        [HttpGet]
        public IActionResult GetResource(string customerId)
        {
            try
            {
                return Ok(_countableResource.GetValue(customerId));
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
        [Route("{increment:int?}")]
        public IActionResult IncrementResource([FromRoute] string customerId, [FromRoute] int? increment = null)
        {
            // We want to increment by 1 by default
            var incrementSize = increment ?? 1;
            if (incrementSize < 0)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Increment must be a positive integer",
                    Increment = increment
                });
            }

            try
            {
                var success = _countableResource.ChangeValue(customerId, incrementSize);
                return success 
                    ? Ok("Resource incremented") 
                    : InternalError();
            }
            catch (ArgumentNullException)
            {
                return InvalidCustomerId(customerId);
            }
            catch (Exception)
            {
                return InternalError();
            }
        }

        [HttpPut]
        [Route("{decrement:int?}")]
        public IActionResult DecrementResource([FromRoute] string customerId, [FromRoute] int? decrement)
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
                var success = _countableResource.ChangeValue(customerId, decrementSize);
                return success 
                    ? Ok("Resource decremented") 
                    : InternalError();
            }
            catch (ArgumentNullException)
            {
                return InvalidCustomerId(customerId);
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