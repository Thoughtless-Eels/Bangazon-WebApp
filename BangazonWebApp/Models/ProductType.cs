using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebApp.Models
{
    public class ProductType
    {

        [Key]
        public int ProductTypeId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Category")]
        public string ProductTypeName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
