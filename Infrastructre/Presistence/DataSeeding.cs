using Domain.Contracts;
using Domain.Models.IdentityModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence
{
    public class DataSeeding(StoreDbContext _dbContext, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
                if (!_dbContext.ProductBrands.Any())
                {
                    //var ProductBrandData = await File.ReadAllTextAsync(@"..\Infrastructre\Presistence\Data\DataSeeding\brands.json");
                    var ProductBrandData = File.OpenRead(@"..\Infrastructre\Presistence\Data\DataSeeding\brands.json"); // openRead : Stream With File
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    if (productBrands is not null && productBrands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                    }
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypeData = File.OpenRead(@"..\Infrastructre\Presistence\Data\DataSeeding\types.json");
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypeData);
                    if (productTypes is not null && productTypes.Any())
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var ProductData = File.OpenRead(@"..\Infrastructre\Presistence\Data\DataSeeding\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                    }
                }
                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliveryMethodData = File.OpenRead(@"..\Infrastructre\Presistence\Data\DataSeeding\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                    {
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "Mohamed@Gmail.com",
                        DisplayName = "Mohamed Aly",
                        UserName = "MohamedAly",
                        PhoneNumber = "01234567890",
                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Salma@Gmail.com",
                        DisplayName = "Salma Mohamed",
                        UserName = "SalmaMohamed",
                        PhoneNumber = "01236844231",
                    };
                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                // To Do
            }

        }
    }
}
