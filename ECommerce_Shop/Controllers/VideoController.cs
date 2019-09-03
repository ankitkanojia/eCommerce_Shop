using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce_Shop.Controllers
{
    public class VideoController : Controller
    {
        private DBEntities db = new DBEntities();

        public async Task<ActionResult> Index(long id = 0)
        {
            try
            {
                var videoVm = await GetModelData(id);
                videoVm.CategoryId = id;
                if (id == 0)
                {
                    videoVm.CategoryName = "All Video(s)";
                }
                return View(videoVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<ContainerPageVm> GetModelData(long id)
        {
            var response = new ContainerPageVm();

            try
            {
                using (db = new DBEntities())
                {
                    if (id == 0)
                    {
                        //Model initialization with grid data
                        response.videoDetails = await db.Containers.Select(m => new ContainerVm
                        {
                            VideoId = m.VideoId,
                            VideoName = m.VideoName,
                            ImageName = m.ImageName,
                            IsVideo = m.IsVideo
                        }).OrderBy(m => m.IsVideo).ToListAsync();

                    }
                    else
                    {
                        //Model initialization with grid data
                        response.videoDetails = await db.Containers.Where(m => m.CategoryId == id).Select(m => new ContainerVm
                        {
                            VideoId = m.VideoId,
                            VideoName = m.VideoName,
                            ImageName = m.ImageName,
                            IsVideo = m.IsVideo
                        }).OrderBy(m => m.IsVideo).ToListAsync();

                    }

                    var categoryDetails = await db.Categories.FindAsync(id);
                    if (categoryDetails != null)
                    {
                        response.CategoryName = categoryDetails.Name;
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