using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using OneClick.Models;
using OneClick.Helper;

namespace OneClick.Controllers
{
    public class MobileController : Controller
    {
        // GET: Mobile
        public ActionResult Index()
        {
            var model = SingleMobileInfo("all");
            return View(model);
        }
        public ActionResult AddNewMobile()
        {
            var model = new MobileDetailModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateMobile(MobileDetailModel model, int i)
        {
            using (var dbContext = new CommonDataContext())
            {
                bool Success = true;
                string Message = "";
                var exist = dbContext.MobileDetailTbls.FirstOrDefault(x => x.Id == model.Id);
                if (exist != null)
                {
                    exist.BrandName = model.BrandName;
                    exist.MobileName = model.MobileName;
                    exist.Price = model.Price;
                    exist.Discount = model.Discount;
                    exist.BatteryCharging = model.BatteryCharging;
                    exist.BatteryType = model.BatteryType;
                    exist.Bluetooth = model.Bluetooth;
                    exist.Camera = model.Camera;
                    exist.Chipset = model.Chipset;
                    exist.CPU = model.CPU;
                    exist.Dimensions = model.Dimensions;
                    exist.Display = model.Display;
                    exist.DisplayProtection = model.DisplayProtection;
                    exist.DisplayType = model.DisplayType;
                    exist.Features = model.Features;
                    exist.GPS = model.GPS;
                    exist.GPU = model.GPU;
                    exist.Jack = model.Jack;
                    exist.MemoryCardSlot = model.MemoryCardSlot;
                    exist.Model = model.ProductModel;
                    exist.NetworkTechnology = model.NetworkTechnology;
                    exist.OS = model.OS;
                    exist.ProductType = model.ProductType;
                    exist.Quantity = model.Quantity;
                    exist.Radio = model.Radio;
                    exist.Ram = model.Ram;
                    exist.Resolution = model.Resolution;
                    exist.Rom = model.Rom;
                    exist.SelfieCamera = model.SelfieCamera;
                    exist.Sensors = model.Sensors;
                    exist.SIM = model.SIM;
                    exist.Size = model.Size;
                    exist.SKU = model.SKU;
                    exist.USB = model.USB;
                    exist.Video = model.Video;
                    exist.WeightGm = model.WeightGm;
                    exist.WLAN = model.WLAN;
                    Message = "Successfully Updated.";
                }
                else
                {
                    MobileDetailTbl newMobile = new MobileDetailTbl();
                    newMobile.BrandName = model.BrandName;
                    newMobile.MobileName = model.MobileName;
                    newMobile.Price = model.Price;
                    newMobile.Discount = model.Discount;
                    newMobile.BatteryCharging = model.BatteryCharging;
                    newMobile.BatteryType = model.BatteryType;
                    newMobile.Bluetooth = model.Bluetooth;
                    newMobile.Camera = model.Camera;
                    newMobile.Chipset = model.Chipset;
                    newMobile.CPU = model.CPU;
                    newMobile.Dimensions = model.Dimensions;
                    newMobile.Display = model.Display;
                    newMobile.DisplayProtection = model.DisplayProtection;
                    newMobile.DisplayType = model.DisplayType;
                    newMobile.Features = model.Features;
                    newMobile.GPS = model.GPS;
                    newMobile.GPU = model.GPU;
                    newMobile.Jack = model.Jack;
                    newMobile.MemoryCardSlot = model.MemoryCardSlot;
                    newMobile.Model = model.ProductModel;
                    newMobile.NetworkTechnology = model.NetworkTechnology;
                    newMobile.OS = model.OS;
                    newMobile.ProductType = model.ProductType;
                    newMobile.Quantity = model.Quantity;
                    newMobile.Radio = model.Radio;
                    newMobile.Ram = model.Ram;
                    newMobile.Resolution = model.Resolution;
                    newMobile.Rom = model.Rom;
                    newMobile.SelfieCamera = model.SelfieCamera;
                    newMobile.Sensors = model.Sensors;
                    newMobile.SIM = model.SIM;
                    newMobile.Size = model.Size;
                    newMobile.SKU = model.SKU;
                    newMobile.USB = model.USB;
                    newMobile.Video = model.Video;
                    newMobile.WeightGm = model.WeightGm;
                    newMobile.WLAN = model.WLAN;
                    dbContext.MobileDetailTbls.InsertOnSubmit(newMobile);
                    Message = "Successfully Inserted.";
                }
                try
                {
                    dbContext.SubmitChanges();

                }
                catch (Exception ex)
                {
                    Success = false;
                    Message = "Failed to update or insert the data.";
                }
                return Json(new
                {
                    Success,
                    Message
                });
            }
        }
        public JsonResult GetMobileById(int id)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {

                var model = dbContext.MobileTbls.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                return Json(new
                {
                    success = true,
                    data = model,
                    Message = "success performed."
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public List<ProductDisplayModel> SingleMobileInfo(string product)
        {
            using (CommonDataContext dbContext = new CommonDataContext())
            {

                var model = dbContext.MobileDetailTbls.Where(x => (product != "all") ? x.BrandName == product : true).Select(x =>
                    new ProductDisplayModel
                    {
                        Id = x.Id,
                        Name = x.MobileName,
                        Price = x.Price,
                        Discount = x.Discount,
                        DiscountPrice = x.Price - x.Price * x.Discount / 100.00,
                        Category = "Mobile"
                        //MobileInfoId = x.MobileInfoId,
                    }
                ).ToList();
                return model;
            }
        }
        public ActionResult GetMobile(string product)
        {
            var model = SingleMobileInfo(product);
            return View("~/Views/Mobile/index.cshtml", model);
        }
        public ActionResult OrderPreview(int id, string category)
        {
            return RedirectToAction("OrderReview", "Order", new { id, category });
        }
    }
}