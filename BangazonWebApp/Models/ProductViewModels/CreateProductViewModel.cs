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
        [StringLength(55, ErrorMessage = "Please shorten the product title to 55 characters")]
        public string Title { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Local Delivery")]
        public bool LocalDelivery { get; set; }

        [Display(Name = "Product Location")]
        public string Location { get; set; }
    
        [Display(Name="Insert Photo URL")]
        public string Photo { get; set; }


        public List<SelectListItem> ProductTypes { get; set; }
        public Product Product { get; set; }

        [Required(ErrorMessage = "Product Type Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a product type before submitting.")]
        [Display(Name = "Product Category")]
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