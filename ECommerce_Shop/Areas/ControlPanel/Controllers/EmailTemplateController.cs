using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmailTemplateController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index()
        {
            try
            {
                using (_db = new DBEntities())
                {
                    var emailTemplates = await _db.EmailTempletes.Select(m => new EmailTemplateVm
                    {
                        EmailTempleteId = m.EmailTempleteId,
                        Name = m.Name,
                        Subject = m.Subject,
                        Body = m.Body,
                        HashTag = m.HashTag,
                        FromId = m.FromId,
                        BccId = m.BccId,
                        IsActive = m.IsActive
                    }).ToListAsync();

                    if (!emailTemplates.Any())
                    {
                        emailTemplates = new List<EmailTemplateVm>();
                    }

                    return View(emailTemplates);
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
            return View(new EmailTemplateVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(EmailTemplateVm data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }

                using (_db = new DBEntities())
                {
                    var emailTemplate = new EmailTemplete
                    {
                        Name = data.Name,
                        Subject = data.Subject,
                        Body = data.Body,
                        FromId = data.FromId,
                        BccId = data.BccId,
                        HashTag = data.HashTag,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = User.Identity.GetUserId()
                    };
                    _db.Entry(emailTemplate).State = EntityState.Added;
                    await _db.SaveChangesAsync();
                }

                TempData["Success"] = SuccessMessage.Added;
                return RedirectToAction("Index", "EmailTemplate", new { area = "ControlPanel" });
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return View();
            }
        }

        public async Task<ActionResult> Edit(long id)
        {
            try
            {
                using (_db = new DBEntities())
                {
                    var emailTemplate = await _db.EmailTempletes.FindAsync(id);
                    if (emailTemplate == null)
                    {
                        ModelState.AddModelError("", ErrorMessage.DataNotFound);
                        return View();
                    }
                    else
                    {
                        var emailTemplateVm = new EmailTemplateVm
                        {
                            EmailTempleteId = emailTemplate.EmailTempleteId,
                            Name = emailTemplate.Name,
                            Subject = emailTemplate.Subject,
                            Body = emailTemplate.Body,
                            HashTag = emailTemplate.HashTag,
                            FromId = emailTemplate.FromId,
                            BccId = emailTemplate.BccId
                        };
                        return View(emailTemplateVm);
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(EmailTemplateVm data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }

                using (_db = new DBEntities())
                {
                    var emailTemplate = await _db.EmailTempletes.FindAsync(data.EmailTempleteId);
                    if (emailTemplate == null)
                    {
                        ModelState.AddModelError("", ErrorMessage.DataNotFound);
                        return View();
                    }
                    else
                    {
                        emailTemplate.Name = data.Name;
                        emailTemplate.Subject = data.Subject;
                        emailTemplate.Body = data.Body;
                        emailTemplate.FromId = data.FromId;
                        emailTemplate.BccId = data.BccId;
                        emailTemplate.HashTag = data.HashTag;
                        emailTemplate.UpdatedDate = DateTime.UtcNow;
                        emailTemplate.UpdatedBy = StaticValues.UserId;//Static UserId
                        _db.Entry(emailTemplate).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }

                TempData["Success"] = SuccessMessage.Updated;
                return RedirectToAction("Index", "EmailTemplate", new { area = "ControlPanel" });
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return View();
            }
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
                        var emailTemplate = await _db.EmailTempletes.FindAsync(id);
                        if (emailTemplate != null)
                        {
                            emailTemplate.IsActive = status;
                            _db.Entry(emailTemplate).State = EntityState.Modified;
                            _db.SaveChanges();
                            var message = status ? SuccessMessage.Activated : SuccessMessage.DeActivated;
                            return Json(new { status = true, message }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            try
            {
                if (id != 0)
                {
                    using (_db = new DBEntities())
                    {
                        var emailTemplate = await _db.EmailTempletes.FindAsync(id);
                        if (emailTemplate != null)
                        {
                            _db.Entry(emailTemplate).State = EntityState.Deleted;
                            await _db.SaveChangesAsync();
                            return Json(new { status = true, message = SuccessMessage.Deleted }, JsonRequestBehavior.AllowGet);
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