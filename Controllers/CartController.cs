using OneClick.Models;
using OneClick.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCart(string id)
        {
            bool success = true;
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                List<CartsTbl> carts = new List<CartsTbl>();
                try
                {
                    carts = dbContext.CartsTbls.Where(x => x.UserId == id).ToList();
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
                    success,
                    data = carts
                });

            }
        }

        [HttpPost]
        public JsonResult SaveCart(CartModel model)
        {
            bool success = true;
            CartsTbl carts = new CartsTbl();

            using (CommonDataContext dbContext = new CommonDataContext())

            {
                if (model.Id > 0)
                {
                    //update
                    CartsTbl data = dbContext.CartsTbls.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (model.Category != null)
                        data.Category = model.Category;
                    if (model.Color != null)
                        data.Color = model.Color;
                    if (model.Quantity != 0)
                        data.Quantity = model.Quantity;
                }
                else
                {
                    carts = new CartsTbl
                    {
                        UserId = model.UserId,
                        ProductId = model.ProductId,
                        Category = model.Category,
                        Quantity = model.Quantity,
                        Color = model.Color
                    };
                    dbContext.CartsTbls.InsertOnSubmit(carts);
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
                    success,
                    data = carts
                });

            }
        }
        public JsonResult DeleteCart(string userId, int id)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                bool success = true;
                var model = dbContext.CartsTbls.Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();
                dbContext.CartsTbls.DeleteOnSubmit(model);

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
                    success
                });
            }
        }
        public JsonResult DeleteAllCart(string userId)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {
                bool success = true;
                var model = dbContext.CartsTbls.Where(x => x.UserId == userId).ToList();
                dbContext.CartsTbls.DeleteAllOnSubmit(model);

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
                    success
                });
            }
        }
    }
}