using Infrastructure.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Endpoint = Infrastructure.Entities.Endpoint;
using Service;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EndpointController : ControllerBase
    {
        private readonly IEndpointInternalService _repository;
        public EndpointController(IEndpointInternalService repo) 
        {
            _repository = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddEndpointAsync(EndpointDRO endpoint)
        {
            string validation = EndpointValidator(endpoint);

            if (validation != "")
            {
                return BadRequest(validation);
            }
            Endpoint endp = new Endpoint()
            {
                Name = endpoint.Name,
                Method = endpoint.Method,
                RandomizeResponse = endpoint.RandomizeResponse,
                ResponseObject = endpoint.ResponseObject,
                Delay = endpoint.Delay,
                Path = endpoint.Path,
                shouldFail = endpoint.shouldFail,
            };

            try
            {
                await _repository.AddEndpointAsync(endp);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEndpointAsync(Guid endpointID)
        {
            try
            {
                await _repository.DeleteEndpointAsync(endpointID);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetEndpointsAsync()
        {
            var endpoints = await _repository.GetEndpointsAsync();
            return Ok(endpoints);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEndpointAsync(Endpoint endpoint)
        {
            try
            {
                await _repository.UpdateEndpointAsync(endpoint);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        private string EndpointValidator(EndpointDRO endpoint)
        {
            if (string.IsNullOrEmpty(endpoint.Name))
            {
                return "Name is required & cannot be empty";
            }
            else if (string.IsNullOrEmpty(endpoint.Path) || endpoint.Path == "/")
            {
                return "Path is required & cannot be empty or only contains '/'";
            }
            else if (!string.IsNullOrEmpty(endpoint.ResponseObject.ToString())) // if response object id is empty let it pass
            {
                if (!Guid.TryParse(endpoint.ResponseObject.ToString(), out Guid _))
                {
                    return "Response Object ID must be a valid GUID";
                }
            }
            return "";
        }
    }
}
