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
            await _repository.AddEndpointAsync(endp);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEndpointAsync(Guid endpointID)
        {
            await _repository.DeleteEndpointAsync(endpointID);
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
            await _repository.UpdateEndpointAsync(endpoint);
            return Ok();
        }
    }
}
