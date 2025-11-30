using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DataTransferObjects.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpPost("{basketId}")] // POST : baseUrl/api/Payments/{basketId}
        public async Task<ActionResult<BasketDto>> CreateOrUpdate(string basketId)
        {
            var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(basket);
        }
        [HttpPost("WebHook")] // POST : baseUrl/api/Payments/WebHook
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(json, Request.Headers["Stripe-Signature"]!);
            return new EmptyResult();
        }
    }
}
