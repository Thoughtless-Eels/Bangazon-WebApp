using BangazonWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebApp.Models.ProductTypeViewModels
{
    public class ShowProductTypesViewModel
    {
        public IEnumerable<ProductTypeDisplayModel> GroupedProducts { get; set; }

        public ShowProductTypesViewModel(ApplicationDbContext ctx)
        {
            //create a constructor that is going to populate the IEenumerbale:
            this.GroupedProducts = ctx.ProductType
            .AsEnumerable()
            .Select(pt => new ProductTypeDisplayModel
            {
                ProductCategoryName = pt.ProductTypeName,
                //populate the products list by matching the product types id on the product type were iterating through:

                CategoryProducts = ctx.Product.Where(p => p.ProductTypeId == pt.ProductTypeId).ToList()
            });
                    
         }




        }




    }

    

