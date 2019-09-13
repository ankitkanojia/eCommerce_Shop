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
    //[Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var brandVm = await GetModelData(id);
                return View(brandVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(BrandVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var brand = new Brand
                        {
                            Name = data.Name,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = User.Identity.GetUserId()
                        };

                        _db.Entry(brand).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Brand", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(BrandVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var brand = await _db.Brands.FindAsync(data.BrandId);
                        if (brand == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            brand.Name = data.Name;
                            brand.UpdatedDate = DateTime.UtcNow;
                            brand.UpdatedBy = StaticValues.UserId;//Static UserId
                            _db.Entry(brand).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Brand", new { area = "ControlPanel", id = data.BrandId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.BrandId));
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
                        var brand = await _db.Brands.FindAsync(id);

                        if (brand != null)
                        {
                            _db.Entry(brand).State = EntityState.Deleted;
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

        private async Task<BrandPageVm> GetModelData(long? id)
        {
            var response = new BrandPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data

                    response.brandDetails = await _db.Brands.Select(m => new BrandVm
                    {
                        BrandId = m.BrandId,
                        Name = m.Name
                    }).ToListAsync();

                    if (id == null || !(id > 0) || response.brandDetails.Any())
                    {
                        //Update operation data
                        var updateData = response.brandDetails.FirstOrDefault(s => s.BrandId == id);

                        if (updateData != null)
                        {
                            response.BrandId = updateData.BrandId;
                            response.Name = updateData.Name;
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