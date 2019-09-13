using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var sizeVm = await GetModelData(id);
                return View(sizeVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(SizeVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var size = new Size
                        {
                            CategoryId = data.CategoryId,
                            Name = data.Name,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = StaticValues.UserId//Static UserId
                        };

                        _db.Entry(size).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Size", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(SizeVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var size = await _db.Sizes.FindAsync(data.SizeId);
                        if (size == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            size.CategoryId = data.CategoryId;
                            size.Name = data.Name;
                            size.UpdatedDate = DateTime.UtcNow;
                            size.UpdatedBy = User.Identity.GetUserId(); 
                            _db.Entry(size).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Size", new { area = "ControlPanel", id = data.SizeId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.SizeId));
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
                        var size = await _db.Sizes.FindAsync(id);
                        if (size != null)
                        {
                            _db.Entry(size).State = EntityState.Deleted;
                            await _db.SaveChangesAsync();
                            return Json(new { status = true, message = SuccessMessage.Deleted }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { status = false, message = ErrorMessage.DataNotFound }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<SizePageVm> GetModelData(long? id)
        {
            var response = new SizePageVm();
            try
            {
                using (_db = new DBEntities())
                {
                    response.sizeDetails = await _db.Sizes.Select(m => new SizeVm
                    {
                        SizeId = m.SizeId,
                        Name = m.Name,
                        CategoryId = m.CategoryId,
                        CategoryName = m.Category.Name
                    }).ToListAsync();

                    if (id == null || !(id > 0) || response.sizeDetails.Any())
                    {
                        var updateData = response.sizeDetails.FirstOrDefault(s => s.SizeId == id);

                        if (updateData != null)
                        {
                            response.SizeId = updateData.SizeId;
                            response.Name = updateData.Name;
                            response.CategoryId = updateData.CategoryId;
                        }
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