using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VideoController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index()
        {
            try
            {
                var videoVm = await GetModelData();
                return View(videoVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ContainerVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var video = new Container
                        {
                            CategoryId = data.CategoryId,
                            IsVideo = false
                        };

                        var virtualPath = string.Empty;
                        if (data.UploadedImage != null)
                        {
                            virtualPath = StaticValues.ContainerImagePath;
                            var physicalPath = Server.MapPath(virtualPath);
                            video.ImageName = Utilities.SaveFile(data.UploadedImage, virtualPath, physicalPath, string.Empty);
                        }
                        else if(data.UploadedVideo != null)
                        {
                            virtualPath = StaticValues.ContainerVideoPath;
                            video.IsVideo = true;
                            var physicalPath = Server.MapPath(virtualPath);
                            video.VideoName = Utilities.SaveFile(data.UploadedVideo, virtualPath, physicalPath, string.Empty);
                        }
                        else
                        {
                            TempData["Error"] = "Upload either video or image";
                            return RedirectToAction("Index", "Video", new { area = "ControlPanel" });
                        }
                        _db.Entry(video).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("Index", "Video", new { area = "ControlPanel" });
        }

        private async Task<ContainerPageVm> GetModelData()
        {
            var response = new ContainerPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    response.videoDetails = await _db.Containers.Select(m => new ContainerVm
                    {
                        VideoId = m.VideoId,
                        IsVideo = m.IsVideo,
                        ImageName = m.ImageName,
                        VideoName = m.VideoName,
                        CategoryId = m.CategoryId
                    }).OrderBy(m => m.IsVideo).ToListAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            try
            {
                if (id != 0)
                {
                    using (_db = new DBEntities())
                    {
                        var videoDetails = await _db.Containers.FindAsync(id);

                        if (videoDetails == null)
                        {
                            return Json(new { status = false, message = ErrorMessage.DataNotFound },
                                JsonRequestBehavior.AllowGet);
                        }

                        var virtualPath = (videoDetails.IsVideo) ? StaticValues.ContainerVideoPath : StaticValues.ContainerImagePath;
                        var fileName = (videoDetails.IsVideo) ? videoDetails.VideoName : videoDetails.ImageName;
                        var physicalPath = Server.MapPath(virtualPath);
                        var existingImagePath = Path.Combine(physicalPath, fileName);
                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }
                        _db.Entry(videoDetails).State = EntityState.Deleted;
                        await _db.SaveChangesAsync();
                        return Json(new { status = true, message = SuccessMessage.Deleted }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { status = false, message = ErrorMessage.DataNotFound }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}