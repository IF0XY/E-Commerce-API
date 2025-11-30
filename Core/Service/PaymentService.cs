using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.BasketModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Service.Abstraction;
using Service.Specifications;
using Shared.DataTransferObjects.BasketModule;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class PaymentService(IConfiguration _configuration,
                                  IBasketRepository _basketRepository,
                                  IUnitOfWork _unitOfWork,
                                  IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                throw new BasketNotFoundException(basketId);

            var productRepo = _unitOfWork.GetRepository<Domain.Models.ProductModule.Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            if (basket.DeliveryMethodId is null)
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                                              .GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Price;

            var amount = (long)(basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingPrice) * 100; // (* 100) => Because The Amount is in Cents

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]
                };

                var paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateAsync(basket);
            return _mapper.Map<Basket, BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var endPointSecret = _configuration.GetSection("Stripe")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endPointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFaildAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentRecievedAsync(paymentIntent.Id);
                    break;
                default:
                    Console.WriteLine($"Unhandled Stripe Event Type: {stripeEvent.Type}");
                    break;
            }
        }

        private async Task UpdatePaymentRecievedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                     .GetByIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentRecieved;

            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task UpdatePaymentFaildAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                     .GetByIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentFaild;

            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
