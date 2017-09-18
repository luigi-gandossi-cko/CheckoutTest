using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class Product
    {
        public Guid Id { get;  set; }
        public String Name { get;  set; }
        public Decimal Price { get;  set; }
        public String ImagePath { get;  set; }
        public string Description { get; set; }
        public int CategoryID { get;  set; }

      

    }
}