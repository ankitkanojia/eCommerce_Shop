using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    public class ConfigurationsController : Controller
    {
        private DBEntities _db = new DBEntities();

        public ActionResult Index()
        {
            try
            {
                var configurationVm = _db.Configurations.OrderBy(m => m.ConfigurationId).Select(m => new ConfigurationVm
                {
                    ConfigurationId = m.ConfigurationId,
                    ConfigurationType = m.ConfigurationType,
                    ConfigurationValue = m.ConfigurationValue
                }).ToList();

                return View(configurationVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ConfigurationVm data)
        {
            try
            {
                using (_db = new DBEntities())
                {
                    var configurations = _db.Configurations.ToList();
                    foreach (var item in configurations)
                    {
                        if (item.ConfigurationId == (long)EnumList.Configurations.LOGO && data.logoContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/logo/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            item.ConfigurationValue = "logo" + Path.GetExtension(data.logoContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.logoContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.WOMENCOVER && data.womenContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/womencover/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            item.ConfigurationValue = "womencover" + Path.GetExtension(data.womenContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.womenContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.MENCOVER && data.menContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/mencover/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            item.ConfigurationValue = "mencover" + Path.GetExtension(data.menContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.menContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.KIDSCOVER && data.kidsContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/kidscover/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                            item.ConfigurationValue = "kidscover" + Path.GetExtension(data.kidsContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.kidsContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.UNIFORMCOVER && data.uniformContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/uniformcover/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            item.ConfigurationValue = "uniformcover" + Path.GetExtension(data.uniformContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.uniformContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.STUDIOCOVER && data.studioContainer != null)
                        {
                            var physicalPath = Server.MapPath("~/Uploads/Configuration/studiocover/");
                            var oldFile = Path.Combine(physicalPath, item.ConfigurationValue);
                            if (!string.IsNullOrWhiteSpace(oldFile) && System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }

                            item.ConfigurationValue = "studiocover" + Path.GetExtension(data.uniformContainer.FileName);
                            var newFilePath = Path.Combine(physicalPath, item.ConfigurationValue);
                            data.studioContainer.SaveAs(newFilePath);
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.MOBILENO)
                        {
                            item.ConfigurationValue = data.mobileno;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.WHATSAPP)
                        {
                            item.ConfigurationValue = data.whatsappno;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.EMAIL)
                        {
                            item.ConfigurationValue = data.email;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.FBLink)
                        {
                            item.ConfigurationValue = data.fblink;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.TWLink)
                        {
                            item.ConfigurationValue = data.twlink;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.INSLink)
                        {
                            item.ConfigurationValue = data.inslink;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.LILink)
                        {
                            item.ConfigurationValue = data.lilink;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.ABOUTUS)
                        {
                            item.ConfigurationValue = data.about;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.TERMSANDCONDITIONS)
                        {
                            item.ConfigurationValue = data.terms;
                        }

                        if (item.ConfigurationId == (long)EnumList.Configurations.PRIVACYPOLICY)
                        {
                            item.ConfigurationValue = data.terms;
                        }
                    }
                    _db.SaveChanges();

                    System.Web.HttpContext.Current.Application.UnLock();
                    System.Web.HttpContext.Current.Application["mobileno"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["whatsappno"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.WHATSAPP).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["email"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.EMAIL).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["fblink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.FBLink).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["twlink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.TWLink).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["inslink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.INSLink).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["lilink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.LILink).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["aboutus"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.ABOUTUS).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["termsandconditions"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.TERMSANDCONDITIONS).ConfigurationValue;
                    System.Web.HttpContext.Current.Application["privacypolicy"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.PRIVACYPOLICY).ConfigurationValue;
                }
                TempData["Success"] = SuccessMessage.Updated;
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("Index", "Configurations", new { area = "ControlPanel" });
        }

        private async Task<List<ConfigurationVm>> GetModelData()
        {
            var response = new List<ConfigurationVm>();
            try
            {
                using (_db = new DBEntities())
                {
                    response = await _db.Configurations.OrderBy(m => m.ConfigurationId).Select(m => new ConfigurationVm
                    {
                        ConfigurationId = m.ConfigurationId,
                        ConfigurationType = m.ConfigurationType,
                        ConfigurationValue = m.ConfigurationValue
                    }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }
    }
}