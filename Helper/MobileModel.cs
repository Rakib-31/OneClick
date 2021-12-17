using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneClick.Helper
{
    public class MobileModel : ProductDisplayModel
    {
        public string BrandName { get; set; }
        public int Ram { get; set; }
        public int Rom { get; set; }
        public double Size { get; set; }
        public string SKU { get; set; }
    }

}