using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdvertisementController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var advertisementVm = await GetModelData(id);
                return View(advertisementVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AdvertisementVm data, HttpPostedFileBase advertisementImage, HttpPostedFileBase vAdvertisementImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var imageName = string.Empty;
                        if (advertisementImage != null)
                        {
                            var virtualPath = StaticValues.AdvertisementImagePath;
                            var physicalPath = Server.MapPath(virtualPath);
                            imageName = Utilities.SaveFile(advertisementImage, virtualPath, physicalPath, string.Empty);
                        }

                        var VimageName = string.Empty;
                        if (vAdvertisementImage != null)
                        {
                            var virtualPath = StaticValues.AdvertisementImagePath;
                            var physicalPath = Server.MapPath(virtualPath);
                            VimageName = Utilities.SaveFile(vAdvertisementImage, virtualPath, physicalPath, string.Empty);
                        }

                        var advertisement = new Advertisement
                        {
                            LinkUrl = data.LinkUrl,
                            HImageUrl  = imageName,
                            VImageUrl = VimageName
                        };

                        _db.Entry(advertisement).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Advertisement", new { area = "ControlPanel" });
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(0));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdvertisementVm data, HttpPostedFileBase advertisementImage, HttpPostedFileBase vAdvertisementImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var advertisement = await _db.Advertisements.FindAsync(data.AdvertisementId);
                        if (advertisement == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            if (advertisementImage != null)
                            {
                                var virtualPath = StaticValues.AdvertisementImagePath;
                                var physicalPath = Server.MapPath(virtualPath);
                                advertisement.HImageUrl = Utilities.SaveFile(advertisementImage, virtualPath, physicalPath, !string.IsNullOrWhiteSpace(advertisement.HImageUrl) ? string.Concat(physicalPath, advertisement.HImageUrl) : string.Empty);
                            }

                            if (vAdvertisementImage != null)
                            {
                                var virtualPath = StaticValues.AdvertisementImagePath;
                                var physicalPath = Server.MapPath(virtualPath);
                                advertisement.VImageUrl = Utilities.SaveFile(vAdvertisementImage, virtualPath, physicalPath, !string.IsNullOrWhiteSpace(advertisement.HImageUrl) ? string.Concat(physicalPath, advertisement.VImageUrl) : string.Empty);
                            }
                            advertisement.LinkUrl = data.LinkUrl;
                            _db.Entry(advertisement).State = EntityState.Modified;
                            await _db.SaveChangesAsync();
                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Advertisement", new { area = "ControlPanel", id = data.AdvertisementId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.AdvertisementId));
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { status = false, message = ErrorMessage.DataNotFound },
                        JsonRequestBehavior.AllowGet);
                }

                using (_db = new DBEntities())
                {
                    var advertisement = await _db.Advertisements.FindAsync(id);

                    if (advertisement == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _db.Entry(advertisement).State = EntityState.Deleted;
                        await _db.SaveChangesAsync();
                    }

                    return Json(new { status = true, message = SuccessMessage.Deleted },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<AdvertisementPageVm> GetModelData(long? id)
        {
            var response = new AdvertisementPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    var advertisement = await _db.Advertisements.Select(m => new AdvertisementVm
                    {
                        LinkUrl = m.LinkUrl,
                        HImageUrl = m.HImageUrl,
                        VImageUrl = m.VImageUrl,
                        AdvertisementId = m.AdvertisementId
                    }).OrderByDescending(m => m.AdvertisementId).ToListAsync();

                    if (advertisement.Any())
                    {
                        response.Collections = advertisement;

                        if (id == null || !(id > 0) || advertisement.Any())
                        {
                            //Update operation data
                            var updateData = advertisement.FirstOrDefault(s => s.AdvertisementId == id);

                            if (updateData != null)
                            {
                                response.LinkUrl = updateData.LinkUrl;
                                response.HImageUrl = updateData.HImageUrl;
                                response.VImageUrl = updateData.VImageUrl;
                                response.AdvertisementId = updateData.AdvertisementId;
                            }
                        }
                    }
                    else
                    {
                        response.Collections = new List<AdvertisementVm>();
                    }
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