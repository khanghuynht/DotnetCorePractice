using DocnetCorePractice.Model;
using DocnetCorePractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DocnetCorePractice.Controllers
{
    [ApiController]
    public class CaffeController : ControllerBase
    {
        public readonly ICaffeService _caffeService;

        public CaffeController(IServiceProvider serviceProvider)
        {
            _caffeService = serviceProvider.GetRequiredService<ICaffeService>();
        }
        [HttpPost]
        [Route("/api/[controller]/add-caffe")]
        public IActionResult AddCaffe([FromBody] CaffeModel caffeModel)
        {
            try
            {
                var caffeList = _caffeService.AddCaffe(caffeModel);
                return Ok(caffeList);
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/get-all-caffe")]
        public IActionResult GetAllCaffe()
        {
            var result = _caffeService.GetAllCaffe();
            if (result.IsNullOrEmpty())
            {
                Response.StatusCode = 204;
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/get-detail-caffe")]
        public IActionResult GetDetailCaffe(string Id)
        {
            var result = _caffeService.GetDetailCaffe(Id);
            if (result == null)
            {
                Response.StatusCode = 204;
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("/api/[controller]/update-caffe")]
        public IActionResult UpdateCaffe(string Id, double price, int discount)
        {
            try
            {
                var result = _caffeService.UpdateCaffe(Id, price, discount);
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                Response.StatusCode = 400;
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/api/[controller]/delete-caffe")]
        public IActionResult DeleteCaffe(string id)
        {
            try
            {
                var result = _caffeService.DeleteCaffe(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return BadRequest(ex.Message);
            }
        }

    }
}
