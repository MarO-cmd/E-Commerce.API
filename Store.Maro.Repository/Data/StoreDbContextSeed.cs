using Store.Maro.Core.Entities;
using Store.Maro.Core.Entities.Orders;
using Store.Maro.Repository.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Maro.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext _context)
        {
            // brands 
            if(_context.Brands.Count() == 0 ) // check if we are in the begining or not seed before
            {
                //1. read file from json
                var brandsData = File.ReadAllText(@"..\Store.Maro.Repository\Data\DataSeed\brands.json");

                //2. "Deserialize" Convert Json to List<T> (object to be stored in DB)

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands is not null && brands.Count() > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.Types.Count() == 0) // check if we are in the begining or not seed before
            {
                //1. read file from json
                var typesData = File.ReadAllText(@"..\Store.Maro.Repository\Data\DataSeed\types.json");

                //2. "Deserialize" Convert Json to List<T> (object to be stored in DB)

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types is not null && types.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.Products.Count() == 0) // check if we are in the begining or not seed before
            {
                //1. read file from json
                var productData = File.ReadAllText(@"..\Store.Maro.Repository\Data\DataSeed\products.json");

                //2. "Deserialize" Convert Json to List<T> (object to be stored in DB)

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products is not null && products.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }

            }

            if (_context.DeliveryMethods.Count() == 0) // check if we are in the begining or not seed before
            {
                //1. read file from json
                var DeliveryData = File.ReadAllText(@"..\Store.Maro.Repository\Data\DataSeed\delivery.json");

                //2. "Deserialize" Convert Json to List<T> (object to be stored in DB)

                var Delivaries = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                if (Delivaries is not null && Delivaries.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(Delivaries);
                    await _context.SaveChangesAsync();
                }

            }




        }
    }
}
