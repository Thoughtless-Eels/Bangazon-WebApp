using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebApp.Models
{
    public class ProductTypeDisplayModel
    {
        public string ProductCategoryName { get; set; }
        public List<Product> CategoryProducts {get;set;}
        public double ProductCount { get; set; }

    }
}


//