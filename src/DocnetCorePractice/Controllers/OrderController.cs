using DocnetCorePractice.Model;
using DocnetCorePractice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocnetCorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IServiceProvider serviceProvider)
        {
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
        }

        [HttpPost]
        [Route("/api/[controller]/create-order-request")]
        public IActionResult OrderRequest([FromBody]CreateOrderRequestModel orderRequestModel)
        {
            try
            {
                var emptyItem = orderRequestModel.Items.Count <= 0;
                if (emptyItem)
                {
                    Response.StatusCode = 400;
                    return BadRequest("No item in the list");
                }
                var result = _orderService.CreateOrder(orderRequestModel);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("/api/[controller]/update-order-request")]
        public IActionResult UpdateOrderRequest([FromBody]UpdateOrderRequestModel requestModel)
        {
            return Ok(requestModel);
        }
    }
}
