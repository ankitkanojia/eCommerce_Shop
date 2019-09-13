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
    public class CollectionPointController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var collectionPointVm = await GetModelData(id);
                return View(collectionPointVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CollectionPointVm data, HttpPostedFileBase categoryImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var collectionPoint = new CollectionPoint
                        {
                            Address = data.Address
                        };
                        _db.Entry(collectionPoint).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "CollectionPoint", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(CollectionPointVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var collectionPoint = await _db.CollectionPoints.FindAsync(data.CollectionPointId);
                        if (collectionPoint == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            collectionPoint.Address = data.Address;
                            _db.Entry(collectionPoint).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "CollectionPoint", new { area = "ControlPanel", id = data.CollectionPointId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.CollectionPointId));
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
                    var collectionPoint = await _db.CollectionPoints.FindAsync(id);

                    if (collectionPoint == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _db.Entry(collectionPoint).State = EntityState.Deleted;
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

        private async Task<CollectionPointPageVm> GetModelData(long? id)
        {
            var response = new CollectionPointPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    var collectionPoint = await _db.CollectionPoints.Select(m => new CollectionPointVm
                    {
                        Address = m.Address,
                        CollectionPointId = m.CollectionPointId
                    }).OrderBy(m => m.Address).ToListAsync();

                    if (collectionPoint.Any())
                    {
                        response.Collections = collectionPoint;

                        if (id == null || !(id > 0) || collectionPoint.Any())
                        {
                            //Update operation data
                            var updateData = collectionPoint.FirstOrDefault(s => s.CollectionPointId == id);

                            if (updateData != null)
                            {
                                response.Address = updateData.Address;
                                response.CollectionPointId = updateData.CollectionPointId;
                            }
                        }
                    }
                    else
                    {
                        response.Collections = new List<CollectionPointVm>();
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