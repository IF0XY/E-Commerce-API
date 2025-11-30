using Domain.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderWithPaymentIntentSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId)
            :base(O => O.PaymentIntentId == paymentIntentId)
        {
                
        }
    }
}
