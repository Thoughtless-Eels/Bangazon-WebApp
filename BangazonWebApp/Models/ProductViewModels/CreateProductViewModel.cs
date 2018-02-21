using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BangazonWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BangazonWebApp.Models.ProductViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public List<SelectListItem> ProductTypes { get; set; }

        public Product Product { get; set; }

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