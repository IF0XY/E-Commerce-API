using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DataTransferObjects.OrderModule;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Create Order
        //[Authorize]
        [HttpPost] // POST : baseUrl/api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return Ok(order);
        }
        [AllowAnonymous]
        // Get Delivery Methods
        [HttpGet("DeliveryMethods")] // Get: baseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

        // Get All Orders By Email
        //[Authorize]
        [HttpGet] // Get :  baseUrl/api/Orders
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync(GetEmailFromToken());
            return Ok(orders);
        }
        // Get Order By Id
        //[Authorize]
        [HttpGet("{id:guid}")] // Get: baseUrl/api/Controller/Orders/{id}
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
    }
}