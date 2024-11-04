using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseObjectController : ControllerBase
    {
        private readonly IResponseObjectInternalRepo _responseObjectInternalRepo;
        public ResponseObjectController(IResponseObjectInternalRepo repo)
        {
            _responseObjectInternalRepo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddResponseObjectAsync([FromBody] string Json)
        {
            try
            {
                ResponseObject responseObject = new ResponseObject();
                responseObject = ResponseObject.FromJson(Json);
                await _responseObjectInternalRepo.AddResponseObjectAsync(responseObject);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{responseObjectID}")]
        public async Task<IActionResult> DeleteResponseObjectAsync(Guid responseObjectID)
        {
            try
            {
                await _responseObjectInternalRepo.DeleteResponseObjectAsync(responseObjectID);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{responseObjectID}")]
        public async Task<IActionResult> GetResponseObjectAsync(Guid responseObjectID)
        {
            try
            {
                var responseObject = await _responseObjectInternalRepo.GetResponseObjectAsync(responseObjectID);
                return Ok(responseObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResponseObjectAsync()
        {
            try
            {
                var response = await _responseObjectInternalRepo.GetAllResponseObjectsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResponseObjectAsync([FromBody] ResponseObject responseObject)
        {
            try
            {
                await _responseObjectInternalRepo.UpdateResponseObjectAsync(responseObject);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
