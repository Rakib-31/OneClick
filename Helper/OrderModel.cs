using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneClick.Helper
{
    public class OrderModel
    {
        public long Id { get; set; }
        public string OrderId { get; set; }
        public bool? PaymentStatus { get; set; }

        public double TotalPayment { get; set; }

        public int OrderStatus { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string UserId { get; set; }
    }

    public class OrderDetails : OrderModel
    {
        public string Category { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
    }
    
    public class CommonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string UserId { get; set; }
    }
    public class ProductDisplayModel: CommonModel
    {
        public int InfoId { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DiscountPrice { get; set; }
        public List<string> Color { get; set; }
    }
    public class PreOrderModel: CommonModel
    {
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public string Color { get; set; }
    }
    public class AddressModel: PreOrderModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string House { get; set; }
        public string Road { get; set; }
        public List<SelectListItem> IsCurrentAddress { get; set; }
        public string SelectedAddress { get; set; }
    }
    public class ConfirmOrderModel
    {
        public PreOrderModel PreorderModel { get; set; }
        public AddressModel Address { get; set; }
    }

}