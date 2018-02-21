using System.Collections.Generic;
using System.Linq;
using BangazonWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BangazonWebApp.Models.ProductViewModels
{
    public class CreateProductViewModel
    {
        
        public string Title { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool LocalDelivery { get; set; }

        public string Location { get; set; }
    
        public string Photo { get; set; }

        public List<SelectListItem> ProductTypes { get; set; }
        public Product Product { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }


        public CreateProductViewModel(ApplicationDbContext ctx)
        {

            this.ProductTypes = ctx.ProductType
                                    .OrderBy(l => l.ProductTypeName)
                                    .AsEnumerable()
                                    .Select(li => new SelectListItem
                                    {
                                        Text = li.ProductTypeName,
                                        Value = li.ProductTypeId.ToString()
                                    }).ToList();

            this.ProductTypes.Insert(0, new SelectListItem
            {
                Text = "Choose category...",
                Value = "0"
            });
        }
    }
}