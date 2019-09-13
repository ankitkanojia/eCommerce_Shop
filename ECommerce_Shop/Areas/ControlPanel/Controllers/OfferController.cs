using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfferController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                var offerVm = await GetModelData(id);
                return View(offerVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(OfferVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var offer = new Offer
                        {
                            Title = data.Title,
                            OfferId = data.OfferId,
                            FlatDiscount = data.FlatDiscount,
                            DiscountId = data.DiscountId,
                            SpecificItems = data.SpecificItems,
                            DiscountPercentage = data.DiscountPercentage,
                            SpecificValue = data.SpecificValue,
                        };

                        _db.Entry(offer).State = EntityState.Added;
                        await _db.SaveChangesAsync();
                    }

                    TempData["Success"] = SuccessMessage.Added;
                    return RedirectToAction("Index", "Offer", new { area = "ControlPanel" });
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
        public async Task<ActionResult> Edit(OfferVm data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db = new DBEntities())
                    {
                        var offer = await _db.Offers.FindAsync(data.OfferId);
                        if (offer == null)
                        {
                            ModelState.AddModelError(ErrorMessage.DataNotFound, ErrorMessage.DataNotFound);
                        }
                        else
                        {
                            offer.OfferId = data.OfferId;
                            offer.FlatDiscount = data.FlatDiscount;
                            offer.DiscountId = data.DiscountId;
                            offer.SpecificItems = data.SpecificItems;
                            offer.DiscountPercentage = data.DiscountPercentage;
                            offer.SpecificValue = data.SpecificValue;
                            offer.Title = data.Title;

                            _db.Entry(offer).State = EntityState.Modified;
                            await _db.SaveChangesAsync();
                            TempData["Success"] = SuccessMessage.Updated;
                            return RedirectToAction("Index", "Offer", new { area = "ControlPanel", id = data.OfferId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View("Index", await GetModelData(data.OfferId));
        }

        [HttpPost]
        public JsonResult Delete(long id)
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
                    var offer = _db.Offers.Find(id);

                    if (offer == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _db.Products.Where(m => m.OfferId == id).ToList().ForEach((w) =>
                         {
                             w.OfferId = 0;
                         });
                        _db.Offers.Remove(offer);
                        _db.SaveChanges();
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

        private async Task<OfferPageVm> GetModelData(long? id)
        {
            var response = new OfferPageVm();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    var offer = await _db.Offers.Where(m => m.OfferId != 0).Select(m => new OfferVm
                    {
                        Title = m.Title,
                        OfferId = m.OfferId,
                        FlatDiscount = m.FlatDiscount,
                        DiscountId = m.DiscountId,
                        DiscountType = m.DiscountType.Type,
                        SpecificItems = m.SpecificItems,
                        DiscountPercentage = m.DiscountPercentage,
                        SpecificValue = m.SpecificValue
                    }).OrderByDescending(m => m.OfferId).ToListAsync();

                    if (offer.Any())
                    {
                        response.Collections = offer;

                        if (id == null || !(id > 0) || offer.Any())
                        {
                            //Update operation data
                            var updateData = offer.FirstOrDefault(s => s.OfferId == id);

                            if (updateData != null)
                            {
                                response.Title = updateData.Title;
                                response.OfferId = updateData.OfferId;
                                response.FlatDiscount = updateData.FlatDiscount;
                                response.DiscountId = updateData.DiscountId;
                                response.SpecificItems = updateData.SpecificItems;
                                response.DiscountPercentage = updateData.DiscountPercentage;
                                response.SpecificValue = updateData.SpecificValue;
                            }
                        }
                    }
                    else
                    {
                        response.Collections = new List<OfferVm>();
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