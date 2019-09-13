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
    public class DashboardController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index()
        {
            try
            {
                var contactVm = await GetModelData();
                return View(contactVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ContactVm>> GetModelData()
        {
            var response = new List<ContactVm>();

            try
            {
                using (_db = new DBEntities())
                {
                    //Model initialization with grid data
                    response = await _db.Contacts.Select(m => new ContactVm
                    {
                        Name = m.Name,
                        Email = m.Email,
                        ContactId = m.ContactId,
                        Mobile = m.Mobile,
                        IsProductInquiry = m.IsProductInquiry,
                        CreatedDate = m.CreatedDate
                    }).OrderByDescending(p=> p.CreatedDate).ToListAsync();

                    if (!response.Any())
                    {
                        response = new List<ContactVm>();
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
        public async Task<JsonResult> GetProductDetails(long id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { status = false, message = ErrorMessage.DataNotFound },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    using (_db = new DBEntities())
                    {
                        var contacts = await _db.Contacts.FirstOrDefaultAsync(k => k.ContactId == id);
                        var contactDetailsVm = new ContactVm();
                        if (contacts != null)
                        {
                            contactDetailsVm.Name = contacts.Name;
                            contactDetailsVm.Email = contacts.Email;
                            contactDetailsVm.Mobile = contacts.Mobile;
                            contactDetailsVm.TotalAmount = 0;
                        }
                        else
                        {
                            return Json(new { status = false, message = ErrorMessage.DataNotFound },
                                JsonRequestBehavior.AllowGet);
                        }

                        var cartDetailsVm = await _db.CartDetails.Where(m => m.ContactId == id).Select(m => new CartDetailVm
                        {
                            ColorName = m.Color.Name,
                            SizeName = m.SizeName,
                            Discount = m.Discount,
                            MRP = m.MRP,
                            Price = m.Price,
                            Quantity = m.Quantity,
                            TAX = m.TAX,
                            ProductName = m.Product.ProductName,
                            ProductId = m.ProductId,
                            ProductImage = StaticValues.DefaultProductImagePath,
                            ColorHex = m.Color.Hex
                        }).ToListAsync();

                        if (cartDetailsVm.Any())
                        {
                            var productImages = await _db.ProductImages.ToListAsync();
                            foreach (var cartDetail in cartDetailsVm)
                            {
                                var productImageDetails = productImages.FirstOrDefault(m =>
                                    m.CoverImage && m.ProductId == cartDetail.ProductId);
                                if (productImageDetails != null)
                                {
                                    cartDetail.ProductImage = "/Uploads/Product/" + productImageDetails.ImageName;
                                }

                                var discountAmount = (cartDetail.Price * (cartDetail.Discount / 100));
                                var discountedPrice = (cartDetail.Price - discountAmount);
                                var taxAmount = (discountedPrice * (cartDetail.TAX / 100));
                                cartDetail.PayableAmount = (discountedPrice + taxAmount) * cartDetail.Quantity;
                                contactDetailsVm.TotalAmount = contactDetailsVm.TotalAmount + cartDetail.PayableAmount;
                            }

                            return Json(new { status = true, message = SuccessMessage.Founded, contactDetails = contactDetailsVm, cartdetails = cartDetailsVm },
                                JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = false, message = ErrorMessage.DataNotFound },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false,
                    message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
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
                        var contact = await _db.Contacts.FindAsync(id);

                        if (contact == null)
                            return Json(new {status = false, message = ErrorMessage.DataNotFound},
                                JsonRequestBehavior.AllowGet);

                        if (contact.IsProductInquiry)
                        {
                            var cartDetails = await _db.CartDetails.Where(m => m.ContactId == id).ToListAsync();
                            if (cartDetails.Any())
                            {
                                _db.CartDetails.RemoveRange(cartDetails);
                            }
                        }
                        _db.Contacts.Remove(contact);
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