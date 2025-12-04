using Domain.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        // Get All
        // 1. brandId = null, typeId = null   => true && true
        // 2. brandId = null, typeId = value  => true                 && P.typeId == typeID
        // 3. brandId = value, typeId = null  => P.brandID == brandID && true
        // 3. brandId = value, typeId = value => P.brandID == brandID && P.typeId == typeID
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams, bool forDashBoard = false)
            : base(P => (!queryParams.brandId.HasValue || P.BrandId == queryParams.brandId)
               && (!queryParams.typeId.HasValue || P.TypeId == queryParams.typeId)
               && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.sortingOption)
            {
                case ProductSortingOption.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOption.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    break;
            }
            if (!forDashBoard)
                ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        // Get By Id
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
