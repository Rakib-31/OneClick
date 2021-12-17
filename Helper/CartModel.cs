using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneClick.Helper
{
    public class CartModel
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string UserId { get; set; }
        public string Color { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}