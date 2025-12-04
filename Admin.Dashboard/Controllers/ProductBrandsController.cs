using Admin.Dashboard.Models.ProductBrandAndTypeViewModels;
using Admin.Dashboard.Models.Products;
using Domain.Contracts;
using Domain.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Specifications;
using Shared;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class ProductBrandsController(IUnitOfWork _unitOfWork) : Controller
    {
        #region Get All
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return View(brands.Select(B => new BrandAndTypeViewModel()
            {
                Id = B.Id,
                Name = B.Name
            }));
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(CreatedBrandAndTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Validation Error !");
                return RedirectToAction(nameof(Index));
            }
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var QuereyParams = new BrandAndTypeQueryParams()
            {
                SearchValue = model.Name
            };
            var spec = new BrandSpecification(QuereyParams);
            var brandIsExist = await repo.GetAllAsync(spec);
            if (!brandIsExist.Any() || brandIsExist == null)
            {
                await repo.AddAsync(new ProductBrand()
                {
                    Name = model.Name
                });
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Brand Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Brand Name is Exist !";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Something Went Wrong While Creating Brand !";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "No Brand With That Id";
                return RedirectToAction(nameof(Index));
            }
            var brand = await _unitOfWork.GetRepository<ProductBrand, int>().GetByIdAsync(id);
            if (brand == null) return NotFound();

            return View(new BrandAndTypeViewModel()
            {
                Id = brand.Id,
                Name = brand.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BrandAndTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Validation Error !");
                return View(model);
            }
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var QuereyParams = new BrandAndTypeQueryParams()
            {
                SearchValue = model.Name
            };
            var spec = new BrandSpecification(QuereyParams);
            var brandIsExist = await repo.GetAllAsync(spec);

            if (brandIsExist.Select(b => b.Id == model.Id && b.Name == model.Name).FirstOrDefault())
            {
                return RedirectToAction(nameof(Index));
            }

            if (brandIsExist == null || !brandIsExist.Any())
            {
                var product = await repo.GetByIdAsync(model.Id);
                if (product is null) return NotFound();
                product.Name = model.Name;
                repo.Update(product);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Brand Edited Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Brand is Exist !");
                return View(model);
            }
            ModelState.AddModelError("Name", "Validation Error !");
            return View(model);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "No Product With That Id !";
                return RedirectToAction(nameof(Index));
            }
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var brand = await repo.GetByIdAsync(id);
            if (brand == null) return NotFound();

            repo.Remove(brand);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Product Brand Deleted Successfully !";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Somthing Went Wrong While Deleting !";
            return RedirectToAction(nameof(Index));
        } 
        #endregion
    }
}
