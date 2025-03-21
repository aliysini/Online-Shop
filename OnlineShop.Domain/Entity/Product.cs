﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entity
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        //public string CategoryName {get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
    }
}
