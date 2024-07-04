using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.PreOrderService;
using SWP391.DAL.Entities;

namespace SWP391.APIs.Controllers.PreOrderController
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreOrderController : ControllerBase
    {
        private readonly PreOrderService _preOrderService;

        public PreOrderController(PreOrderService preOrderService)
        {
            _preOrderService = preOrderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePreOrder([FromBody] CreatePreOrderModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var preOrder = await _preOrderService.CreatePreOrderAsync(model.UserId, model.ProductId);
            return Ok(preOrder);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPreOrdersByUserId(int userId)
        {
            var preOrders = await _preOrderService.GetPreOrdersByUserIdAsync(userId);
            return Ok(preOrders);
        }
        [HttpGet("GetAllPreOrders")]
        public async Task<ActionResult<List<PreOrder>>> GetAllPreOrders()
        {
            var preOrders = await _preOrderService.GetAllPreOrdersAsync();
            return Ok(preOrders);
        }
    }

    public class CreatePreOrderModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

}
