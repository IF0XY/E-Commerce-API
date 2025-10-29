using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress address, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            UserEmail = userEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddress Address { get; set; } = default!; // Own RealationShip
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; } // FK
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderItem> Items { get; set; } = []; // intialize with Empty Collection
        public decimal SubTotal { get; set; }
        public decimal GetTotal() // Get(Name) he Will Get the Total by convention (Get + (Name of Prop))
        {
            return (SubTotal + DeliveryMethod.Price);
        }
    }
}
