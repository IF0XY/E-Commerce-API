using Domain.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class TypeSpecification : BaseSpecification<ProductType, int>
    {
        public TypeSpecification(BrandAndTypeQueryParams queryParams, bool forDashBoard = false)
            : base(P => P.Name == queryParams.SearchValue)
        {
            
        }
    }
}
