using OneClick.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using OneClick.Models;
using Microsoft.AspNet.Identity;

namespace PShop.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MyOrder(string search)
        {
            bool success = false;
            var userId = User.Identity.GetUserId();
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                var model = dbContext.OrdersTbls.Where(x => x.UserId == userId).ToList();
                if (model != null)
                {
                    success = true;
                }

                return Json(new
                {
                    success,
                    data = model
                });
            }
        }

        public IQueryable<OrderDetails> OrderDetails(int id)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                var model = from o in dbContext.OrdersTbls.Where(x => x.Id == id)
                            from op in dbContext.OrderProductTbls
                            where (o.OrderId == op.OrderId)
                            select new OrderDetails
                            {
                                OrderId = o.OrderId.ToString(),
                                DeliveryDate = o.DeliveryDate,
                                OrderDate = o.OrderDate,
                                PaymentStatus = o.PaymentStatus,
                                TotalPayment = o.TotalPayment,
                                Category = op.Category,
                                ProductId = op.ProductId,
                                ShippedDate = o.ShippedDate,
                                OrderStatus = o.OrderStatus
                            };
                return model;
            }
        }
        public OrderDetails GetOrderedMobileDetails(OrderDetails temp)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                OrderDetails product = dbContext.MobileTbls.Where(x => x.Id == temp.ProductId).Select(x =>
                    new OrderDetails
                    {
                        OrderId = temp.OrderId.ToString(),
                        DeliveryDate = temp.DeliveryDate,
                        OrderDate = temp.OrderDate,
                        PaymentStatus = temp.PaymentStatus,
                        TotalPayment = temp.TotalPayment,
                        Category = temp.Category,
                        ProductId = temp.ProductId,
                        ShippedDate = temp.ShippedDate,
                        OrderStatus = temp.OrderStatus,
                        ProductName = x.MobileName,
                        BrandName = x.BrandName
                    }
                ).FirstOrDefault();
                return product;
            }

        }
        public JsonResult GetSingularOrder(int id)
        {
            List<OrderDetails> Products = new List<OrderDetails>();
            bool success = false;
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                var model = OrderDetails(id);
                if (model != null)
                {
                    success = true;
                    List<OrderDetails> orders = model.ToList();

                    OrderDetails product = new OrderDetails();
                    foreach (OrderDetails temp in orders)
                    {
                        if (temp.Category == "Mobile")
                        {
                            product = GetOrderedMobileDetails(temp);
                        }
                        if (product != null)
                        {
                            Products.Add(product);
                        }

                    }
                }

                return Json(new
                {
                    success,
                    data = Products
                });
            }
        }
        //public string MakeOrderId()
        //{
        //    int length = 7;

        //    string str_build = "";
        //    Random random = new Random();

        //    char letter;

        //    for (int i = 0; i < length; i++)
        //    {
        //        double flt = random.NextDouble();
        //        int shift = Convert.ToInt32(Math.Floor(25 * flt));
        //        letter = Convert.ToChar(shift + 65);
        //        str_build += letter;
        //    }
        //    byte[] tmpHash;
        //    byte [] tmpSource = ASCIIEncoding.ASCII.GetBytes(str_build);

        //    tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
        //    return tmpHash.ToString();
        //}
        public OrderProductTbl MakeListData(int id, string Category, int Quantity, string Color, double SubTotal)
        {
            return new OrderProductTbl
            {
                ProductId = id,
                Category = Category,
                Quantity = Quantity,
                Color = Color,
                ProductPrice = SubTotal
            };
        }
        public string GetDeliveryAddress(AddressModel orderModel)
        {
            string address = "";
            var userId = User.Identity.GetUserId();
            if (orderModel != null)
            {
                if(orderModel.SelectedAddress == "0")
                {
                    using(var dbContext = new CommonDataContext())
                    {
                        var user = dbContext.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                        if(user != null)
                        {
                            orderModel.Road = user.Road;
                            orderModel.House = user.House;
                            orderModel.City = user.City;
                            orderModel.Country = user.Country;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(orderModel.Road))
                {
                    address = string.IsNullOrEmpty(address) ? orderModel.Road : address + ", " + orderModel.Road;
                }
                if (!string.IsNullOrEmpty(orderModel.House))
                {
                    address = string.IsNullOrEmpty(address) ? orderModel.House : address + ", " + orderModel.House;
                }
                if (!string.IsNullOrEmpty(orderModel.City))
                {
                    address = string.IsNullOrEmpty(address) ? orderModel.City : address + ", " + orderModel.City;
                }
                if (!string.IsNullOrEmpty(orderModel.Country))
                {
                    address = string.IsNullOrEmpty(address) ? orderModel.Country : address + ", " + orderModel.Country;
                }
            }
            
            return address;
        }
        public OrdersTbl GetOrderModel(string userId, Guid orderId, double totalPayment, string address )
        {
            return new OrdersTbl
            {
                UserId = userId,
                OrderId = orderId,
                TotalPayment = totalPayment,
                PaymentStatus = false,
                OrderStatus = 1,
                OrderDate = DateTime.UtcNow,
                DeliveryAddress = address
            };
        }
        [HttpPost]
        public JsonResult ConfirmOrder(AddressModel orderModel)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                var userId = User.Identity.GetUserId();
                bool success = false;
                string msg = "Order not completed.";
                try
                {
                    List<OrderProductTbl> data = new List<OrderProductTbl>();
                    OrderProductTbl OrderData = new OrderProductTbl();
                    double TotalPayment = 0.0;
                    var OrderId = Guid.NewGuid();

                    if (orderModel.Id != 0)
                    {
                        OrderData = MakeListData(orderModel.Id, orderModel.Category, orderModel.Quantity, orderModel.Color, orderModel.SubTotal);
                        OrderData.OrderId = OrderId;
                        TotalPayment = orderModel.SubTotal;
                        data.Add(OrderData);
                    }
                    else
                    {
                        var model = dbContext.CartsTbls.Where(x => x.UserId == userId).ToList();
                        foreach (var x in model)
                        {
                            OrderData = MakeListData(x.ProductId, x.Category, x.Quantity, x.Color, x.SubTotal);
                            OrderData.OrderId = OrderId;
                            TotalPayment += x.SubTotal;
                            data.Add(OrderData);
                        }
                    }

                    dbContext.OrderProductTbls.InsertAllOnSubmit(data);

                    
                    string deliveryAddress = GetDeliveryAddress(orderModel);
                    var OrderDetails = GetOrderModel(userId, OrderId, TotalPayment, deliveryAddress);

                    
                    dbContext.OrdersTbls.InsertOnSubmit(OrderDetails);
                    dbContext.SubmitChanges();
                    success = true;
                    msg = "Order successfully placed.";
                    //clear cart
                }
                catch (Exception ex)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    //AIManager.Instance.LogError(ex, tenant: "", controllerName, actionName);
                }

                return Json(new
                {
                    msg,
                    success
                });
            }
        }
        [HttpPost]
        public JsonResult CancelOrder(int id)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                bool success = true;
                var model = dbContext.OrdersTbls.Where(x => x.Id == id).FirstOrDefault();
                if (model != null)
                {
                    model.OrderStatus = 1;
                }
                try
                {
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    success = false;
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    //AIManager.Instance.LogError(ex, tenant: "", controllerName, actionName);
                }

                return Json(new
                {
                    msg = "Order success",
                    success
                });
            }
        }
        public JsonResult DeleteOrder(int id)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                bool success = true;
                var model = dbContext.OrdersTbls.Where(x => x.Id == id).FirstOrDefault();

                if (model != null)
                {
                    var orderProductmodel = dbContext.OrderProductTbls.Where(x => x.OrderId == model.OrderId).ToList();
                    dbContext.OrderProductTbls.DeleteAllOnSubmit(orderProductmodel);
                    dbContext.OrdersTbls.DeleteOnSubmit(model);
                }
                try
                {
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    success = false;
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    //AIManager.Instance.LogError(ex, tenant: "", controllerName, actionName);
                }

                return Json(new
                {
                    msg = "Order success",
                    success
                });
            }
        }
        public ActionResult OrderReview(int id, string category)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                if (category == "Mobile")
                {
                    var product = dbContext.MobileDetailTbls.Where(x => x.Id == id).Select(x =>
                        new ProductDisplayModel
                        {
                            Id = x.Id,
                            Name = x.MobileName,
                            Category = category,
                            Price = x.Price,
                            Discount = x.Discount,
                            DiscountPrice = x.Price - x.Price * x.Discount / 100.0,
                            Color = x.Color != null && x.Color != "" ? x.Color.Split(',').ToList() : new List<string>(),
                            //Quantity = x.q
                        }
                    ).FirstOrDefault();
                    return View("OrderReviewPage", product);
                }
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult PreOrder(PreOrderModel orderModel)
        {
            var orderConfirm = new AddressModel();
            orderConfirm.Id = orderModel.Id;
            orderConfirm.Name = orderModel.Name;
            orderConfirm.Category = orderModel.Category;
            orderConfirm.Quantity = orderModel.Quantity;
            orderConfirm.SubTotal = orderModel.SubTotal;
            orderConfirm.Color = orderModel.Color;
            


            return RedirectToAction("OrderShipmentAddress", orderConfirm);
        }
        public ActionResult OrderShipmentAddress(AddressModel model)
        {
            
            if(User.Identity.IsAuthenticated == false)
            {
                var url = HttpContext.Request.Url.PathAndQuery;
                return RedirectToAction("Login", "Account", new { returnUrl = url });
            }
            
            model.IsCurrentAddress = GetBooleanFromStringSelectedList("0");
            return View("~/Views/Order/OrderShipment.cshtml", model);
        }

        public static List<SelectListItem> GetBooleanFromStringSelectedList(string selectedPainQuestion)
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem {Text = "Current Address", Value = "0", Selected = selectedPainQuestion == "0"},
                new SelectListItem {Text = "New Address", Value = "1", Selected = selectedPainQuestion == "1"}
            };
            return items;
        }
    }
}