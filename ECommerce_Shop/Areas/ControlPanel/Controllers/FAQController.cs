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
    //[Authorize(Roles = "Admin")]
    public class FaqController : Controller
    {
        private DBEntities db = new DBEntities();

        public async Task<ActionResult> Index()
        {
            try
            {
                using (db = new DBEntities())
                {
                    var faqVm = await db.FAQs.Select(m => new FAQVm()
                    {
                        FAQId = m.FAQId,
                        Question = m.Question,
                        Answer = m.Answer,
                        Date = (m.UpdatedDate.HasValue && m.UpdatedDate.Value != DateTime.MinValue) ? m.UpdatedDate.Value : m.CreatedDate
                    }).ToListAsync();

                    if (!faqVm.Any())
                    {
                        faqVm = new List<FAQVm>();
                    }

                    return View(faqVm);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult Add()
        {
            return View(new FAQVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FAQVm data)
        {
            try
            {
                using (db = new DBEntities())
                {
                    if (!string.IsNullOrEmpty(data.Question) && !string.IsNullOrEmpty(data.Answer))
                    {
                        var faq = new FAQ
                        {
                            Question = data.Question,
                            Answer = data.Answer,
                            CreatedDate = DateTime.UtcNow
                        };

                        db.FAQs.Add(faq);
                        await db.SaveChangesAsync();
                        TempData["Success"] = SuccessMessage.Added;
                        return RedirectToAction("Index", "Faq", new { area = "ControlPanel" });
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View();
        }

        public async Task<ActionResult> Edit(long id)
        {
            try
            {
                using (db = new DBEntities())
                {
                    if (id != 0)
                    {
                        var faqs = await db.FAQs.FindAsync(id);
                        var faqVm = new FAQVm();
                        if (faqs == null)
                        {
                            return View(faqVm);
                        }

                        faqVm.FAQId = faqs.FAQId;
                        faqVm.Question = faqs.Question;
                        faqVm.Answer = faqs.Answer;
                        return View(faqVm);
                    }
                    else
                    {
                        TempData["Error"] = ErrorMessage.DataNotFound;
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FAQVm data)
        {
            try
            {
                using (db = new DBEntities())
                {
                    if (!string.IsNullOrEmpty(data.Question) && !string.IsNullOrEmpty(data.Answer))
                    {
                        var faq = await db.FAQs.FindAsync(data.FAQId);
                        if (faq != null)
                        {
                            faq.Question = data.Question;
                            faq.Answer = data.Answer;
                            faq.UpdatedDate = DateTime.UtcNow;
                        }
                        await db.SaveChangesAsync();
                        TempData["Success"] = SuccessMessage.Updated;
                        return RedirectToAction("Index", "Faq", new { area = "ControlPanel" });
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return View();
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

                using (db = new DBEntities())
                {
                    var faq = await db.FAQs.FindAsync(id);

                    if (faq == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }

                    db.Entry(faq).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                    return Json(new { status = true, message = SuccessMessage.Deleted }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}