using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Attributes;
using Service.Abstraction;
using Shared;
using Shared.DataTransferObjects.ProdcutModule;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] // baseUrl/api/Products
    //public class ProductsController([FromKeyedServices("Lazy")]IServiceManager _serviceManager) : ControllerBase
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get All Products
        [HttpGet]
        // Get: baseUrl/api/Products
        // NameAsc
        // NameDesc
        // PriceAsc
        // PriceDesc

        //[CachAttribute]
        [Cache(60)] // no need to write Attribute 
        public async Task<ActionResult<IEnumerable<PaginatedResult<ProductDto>>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }
        // Get Product by Id
        //[HttpGet("id")] // static segment => baseUrl/api/Products/id?id=10

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorToReturn), StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")] // variable segment => baseUrl/api/Products/10

        // Get: baseUrl/api/Products?id=10 // Query Param
        // Get: baseUrl/api/Products/id // segment
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }
        // Get All Brands
        [HttpGet("brands")] // static segment
        // Get: baseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }
        // Get All Type
        [HttpGet("types")] // static segment
        // Get: baseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
