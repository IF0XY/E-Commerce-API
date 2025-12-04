using Domain.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class BrandSpecification : BaseSpecification<ProductBrand, int>
    {
        public BrandSpecification(BrandAndTypeQueryParams queryParams, bool forDashBoard = false)
            : base(P => P.Name == queryParams.SearchValue)
        {
            
        }
    }
}
