using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using BangazonWebApp.Data;
using BangazonWebApp.Models;

namespace BangazonWebApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any products.
                if (context.ProductType.Any())
                {
                    return;   // DB has been seeded
                }

                var productTypes = new ProductType[]
                {
                    new ProductType {
                        ProductTypeName = "Electronics"
                    },
                    new ProductType {
                        ProductTypeName = "Appliances"
                    },
                    new ProductType {
                        ProductTypeName = "Housewares"
                    },
                };

                foreach (ProductType i in productTypes)
                {
                    context.ProductType.Add(i);
                }
                context.SaveChanges();
            }
        }
    }
}
