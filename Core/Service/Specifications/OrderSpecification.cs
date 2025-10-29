using Domain.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderSpecification : BaseSpecification<Order, Guid>
    {
        // Get All Orders By Email
        public OrderSpecification(string email) : base(O => O.UserEmail == email)
        {
            AddInclude(O => O.DeliveryMethod); 
            AddInclude(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        // Get Order By Id
        public OrderSpecification(Guid id) : base(O => O.Id == id)
        {
            AddInclude(O => O.DeliveryMethod); 
            AddInclude(O => O.Items);
        }
    }
}
