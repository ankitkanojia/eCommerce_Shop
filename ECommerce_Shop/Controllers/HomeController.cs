using ECommerce_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ECommerce_Shop.Models;
using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models.ViewModels;

namespace ECommerce_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBEntities db = new DBEntities();

        public void SendTestMail1620()
        {
            SendEmail.Send("Test BossKINDS", "Be Your Boss", "suchit.devdigital@gmail.com");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            try
            {
                using (db)
                {
                    var faqs = db.FAQs.ToList();

                    return View(faqs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public ActionResult CollectionPoints()
        {
            try
            {
                using (db)
                {
                    var collectionPoints = db.CollectionPoints.OrderBy(m => m.Address).ToList();
                    return View(collectionPoints);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public JsonResult GetProducts()
        {
            try
            {
                using (db)
                {
                    var productDetails = db.Products.Where(m => m.IsActive).Select(k => new ProductVm
                    {
                        ProductId = k.ProductId,
                        ProductName = k.ProductName
                    }).ToList();

                    if (!productDetails.Any())
                    {
                        productDetails = new List<ProductVm>();
                    }
                    return Json(new { productDetails }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
    }
}