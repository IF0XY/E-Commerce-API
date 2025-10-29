using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.IdentityModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Service.Abstraction;
using Service.Specifications;
using Shared.DataTransferObjects.OrderModule;
using Shared.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map AddressDto To OrderAddress
            var address = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);
            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(orderDto.BasketId);

            // Create Order Item List
            List<OrderItem> orderItems = new List<OrderItem>();
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);
                var OrderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        PictureUrl = item.PictureUrl,
                        ProductName = item.ProductName
                    },
                    Price = product.Price,
                    Quantity = item.Quantity

                };
                orderItems.Add(OrderItem);
            }

            // Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            // Calculate SubTotal
            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);

            var order = new Order(email, address, deliveryMethod, orderItems, subTotal);

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            return _mapper.Map<Order, OrderToReturnDto>(order!);
        }
    }
}
