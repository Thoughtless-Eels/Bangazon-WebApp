﻿using BangazonWebApp.Data;
using BangazonWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebApp.Models.ProductTypeViewModels
{

            public class ShowProductTypesViewModel
            {
                public int ProductTypeId { get; set; }
                public string ProductTypeName { get; set; }
                public List<Product> CategoryProducts { get; set; }
                public double ProductCount { get; set; }
                public IEnumerable<Product> Products { get; set; }
            }

}




