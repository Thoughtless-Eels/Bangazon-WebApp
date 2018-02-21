using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebApp.Models
{
    public class Product   {

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the product title to 55 characters")]
        public string Title { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

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

        public string Photo { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        public ICollection<LineItem> LineItems { get; set; }


    }
}


