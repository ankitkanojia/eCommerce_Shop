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
    public class ColorController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var colorVm = await GetModelData(id);
                return View(colorVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ColorVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var color = new Color
                        {
                            Name = data.Name,
                            Hex = data.Hex,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = User.Identity.GetUserId()
                        };
                        _db.Entry(color).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Color", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(ColorVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var color = await _db.Colors.FindAsync(data.ColorId);
                        if (color == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            color.Name = data.Name;
                            color.Hex = data.Hex;
                            color.UpdatedDate = DateTime.UtcNow;
                            color.UpdatedBy = StaticValues.UserId;//Static UserId
                            _db.Entry(color).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Color", new { area = "ControlPanel", id = data.ColorId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.ColorId));
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
                        var color = await _db.Colors.FindAsync(id);

                        if (color != null)
                        {
                            _db.Entry(color).State = EntityState.Deleted;
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

        private async Task<ColorPageVm> GetModelData(long? id)
        {
            var response = new ColorPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data

                    response.colorDetails = await _db.Colors.Select(m => new ColorVm
                    {
                        ColorId = m.ColorId,
                        Name = m.Name,
                        Hex = m.Hex
                    }).ToListAsync();

                    if (id == null || !(id > 0) || response.colorDetails.Any())
                    {
                        //Update operation data
                        var updateData = response.colorDetails.FirstOrDefault(s => s.ColorId == id);

                        if (updateData != null)
                        {
                            response.ColorId = updateData.ColorId;
                            response.Name = updateData.Name;
                            response.Hex = updateData.Hex;
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