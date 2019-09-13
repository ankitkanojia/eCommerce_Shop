using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class FileManagerController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long id)
        {
            try
            {
                using (_db = new DBEntities())
                {
                    var fileManagers = await GetModelData(id);
                    return View(fileManagers);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<FileManagerPageVm> GetModelData(long id)
        {
            var response = new FileManagerPageVm();
            try
            {
                using (_db = new DBEntities())
                {

                    var fileManagers = CommonFunctions.GetFileManager(id);

                    if (id == (int)EnumList.ImageType.SLIDER)
                    {
                        response.type = "Slider";
                        response.imagePath = StaticValues.SliderImagePath;
                    }
                    else if (id == (int)EnumList.ImageType.OFFERS)
                    {
                        response.type = "Offer";
                        response.imagePath = StaticValues.OfferImagePath;
                    }
                    else if (id == (int)EnumList.ImageType.PARTNERS)
                    {
                        response.type = "Partner";
                        response.imagePath = StaticValues.PartnerImagePath;
                    }
                    response.typeId = id;
                    response.containerDetails = fileManagers;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FileManagerVm data, HttpPostedFileBase imageContainerFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var fileManager = new FileManager();
                        if (imageContainerFile != null)
                        {
                            var virtualPath = string.Empty;
                            if (data.typeId == (int)EnumList.ImageType.SLIDER)
                            {
                                virtualPath = StaticValues.SliderImagePath;
                            }
                            else if (data.typeId == (int)EnumList.ImageType.OFFERS)
                            {
                                virtualPath = StaticValues.OfferImagePath;
                            }
                            else if (data.typeId == (int)EnumList.ImageType.PARTNERS)
                            {
                                virtualPath = StaticValues.PartnerImagePath;
                            }
                            var physicalPath = Server.MapPath(virtualPath);
                            fileManager.ImageName = Utilities.SaveFile(imageContainerFile, virtualPath, physicalPath, string.Empty);
                        }
                        else
                        {
                            TempData["Error"] = "Data not found.";
                        }

                        fileManager.typeId = data.typeId;
                        _db.Entry(fileManager).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.ImageAdded;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return RedirectToAction("Index", "FileManager", new { area = "ControlPanel", id = data.typeId });
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
                    var fileManager = await _db.FileManagers.FindAsync(id);

                    if (fileManager == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var virtualPath = string.Empty;
                        if (fileManager.typeId == (int)EnumList.ImageType.SLIDER)
                        {
                            virtualPath = StaticValues.SliderImagePath;
                        }
                        else if (fileManager.typeId == (int)EnumList.ImageType.OFFERS)
                        {
                            virtualPath = StaticValues.OfferImagePath;
                        }
                        else if (fileManager.typeId == (int)EnumList.ImageType.PARTNERS)
                        {
                            virtualPath = StaticValues.PartnerImagePath;
                        }
                        var physicalPath = Server.MapPath(virtualPath);
                        var existingImagePath = Path.Combine(physicalPath, fileManager.ImageName);
                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }
                    }
                    _db.Entry(fileManager).State = EntityState.Deleted;
                    await _db.SaveChangesAsync();

                    return Json(new { status = true, message = SuccessMessage.Deleted },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}