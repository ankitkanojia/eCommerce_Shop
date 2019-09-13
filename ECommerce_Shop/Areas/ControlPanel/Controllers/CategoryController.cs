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
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var categoryVm = await GetModelData(id);
                return View(categoryVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CategoryVm data, HttpPostedFileBase categoryImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var imageName = string.Empty;
                        if (categoryImage != null)
                        {
                            var virtualPath = StaticValues.CategoryImagePath;
                            var physicalPath = Server.MapPath(virtualPath);
                            imageName = Utilities.SaveFile(categoryImage, virtualPath, physicalPath, string.Empty);
                        }

                        var category = new Category
                        {
                            Name = data.Name,
                            ImageName = imageName,
                            RootCategoryId = data.RootCategoryId.HasValue ? data.RootCategoryId.Value : 0,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = User.Identity.GetUserId(),
                            IsActive = true,
                            CustomUrl = data.CustomUrl
                        };

                        _db.Entry(category).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Category", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(CategoryVm data, HttpPostedFileBase categoryImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var category = await _db.Categories.FindAsync(data.CategoryId);
                        if (category == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            if (categoryImage != null)
                            {
                                var virtualPath = StaticValues.CategoryImagePath;
                                var physicalPath = Server.MapPath(virtualPath);
                                category.ImageName = Utilities.SaveFile(categoryImage, virtualPath, physicalPath, !string.IsNullOrWhiteSpace(category.ImageName) ? string.Concat(physicalPath, category.ImageName) : string.Empty);
                            }

                            category.CustomUrl = data.CustomUrl;
                            category.Name = data.Name;
                            category.RootCategoryId = data.RootCategoryId.HasValue ? data.RootCategoryId.Value : 0;
                            category.UpdatedDate = DateTime.UtcNow;
                            category.UpdatedBy = StaticValues.UserId;//Static UserId
                            _db.Entry(category).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Category", new { area = "ControlPanel", id = data.CategoryId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.CategoryId));
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
                    var category = await _db.Categories.FindAsync(id);

                    if (category == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }

                    var subcategory = await _db.Categories.Where(m => m.RootCategoryId == category.CategoryId)
                        .ToListAsync();

                    if (subcategory.Any())
                    {
                        return Json(new { status = false, message = ErrorMessage.ReferenceDataFound },
                            JsonRequestBehavior.AllowGet);
                    }

                    if (category.Sizes.Any())
                    {
                        return Json(new { status = false, message = ErrorMessage.ReferenceDataFound },
                            JsonRequestBehavior.AllowGet);
                    }

                    _db.Entry(category).State = EntityState.Deleted;
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

        private async Task<CategoryPageVm> GetModelData(long? id)
        {
            var response = new CategoryPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    var categories = await _db.Categories.Select(m => new CategoryVm
                    {
                        CategoryId = m.CategoryId,
                        RootCategoryId = m.RootCategoryId,
                        ImageName = m.ImageName,
                        Name = m.Name,
                        RootCategoryName = string.Empty,
                        IsActive = m.IsActive,
                        CustomUrl = m.CustomUrl
                    }).Where(m=> m.Name.ToLower() != "onesize").ToListAsync();

                    if (categories.Any())
                    {
                        foreach (var data in categories)
                        {
                            data.RootCategoryName =
                                categories.FirstOrDefault(m => m.CategoryId == data.RootCategoryId)?.Name;
                        }

                        response.categoryDetails = categories;
                    }
                    else
                    {
                        response.categoryDetails = new List<CategoryVm>();
                    }

                    if (id == null || !(id > 0) || response.categoryDetails.Any())
                    {
                        //Update operation data
                        var updateData = response.categoryDetails.FirstOrDefault(s => s.CategoryId == id);

                        if (updateData != null)
                        {
                            response.CategoryId = updateData.CategoryId;
                            response.RootCategoryId = updateData.RootCategoryId;
                            response.ImageName = updateData.ImageName;
                            response.Name = updateData.Name;
                            response.CustomUrl = updateData.CustomUrl;
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

        [HttpPost]
        public async Task<JsonResult> ActivateDeActivate(long id, bool status)
        {
            try
            {
                if (id != 0)
                {
                    using (_db = new DBEntities())
                    {
                        var category = await _db.Categories.FindAsync(id);
                        if (category != null)
                        {
                            category.IsActive = status;
                            _db.Entry(category).State = EntityState.Modified;
                            _db.SaveChanges();
                            var message = status ? SuccessMessage.Activated : SuccessMessage.DeActivated;
                            return Json(new { status = true, message, id = category.CategoryId, name = category.Name }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = ErrorMessage.DataNotFound }, JsonRequestBehavior.AllowGet);
        }
    }
}