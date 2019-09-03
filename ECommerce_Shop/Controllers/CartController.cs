using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ECommerce_Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            try
            {
                using (db)
                {
                    var model = new List<CartVm>();
                    var cartCookie = CookieHelper.Get(StaticValues.CookieNameCartCookie);

                    if (!string.IsNullOrWhiteSpace(cartCookie))
                    {
                        model = db.CartDetails.Where(s => s.CartCookie == cartCookie).Select(s => new CartVm
                        {
                            ProductId = s.ProductId,
                            CartCookie = s.CartCookie,
                            ProductName = s.Product.ProductName,
                            Price = s.Price,
                            MRP = s.MRP,
                            Discount = s.Discount,
                            TAX = s.TAX,
                            SizeName = s.SizeName,
                            ContactId = s.ContactId,
                            CartDetailId = s.CartDetailId,
                            Quantity = s.Quantity,
                            ColorHex = s.Color.Hex,
                            ColorName = s.Color.Name,
                            ProductImage = s.Product.ProductImages.FirstOrDefault(k => k.CoverImage).ImageName,
                            OfferTitle = s.OfferTitle
                        }).ToList();
                    }

                    return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public ActionResult RemoveCartItem(long id)
        {
            try
            {
                using (db)
                {
                    var cookieCart = CookieHelper.Get(StaticValues.CookieNameCartCookie);

                    if (!string.IsNullOrWhiteSpace(cookieCart))
                    {
                        var cartItem = db.CartDetails.FirstOrDefault(s => s.CartCookie == cookieCart && s.CartDetailId == id);

                        if (cartItem != null)
                        {
                            db.CartDetails.Remove(cartItem);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public JsonResult Checkout(string name, string email, string phone)
        {
            try
            {
                using (db)
                {
                    var cookieCart = CookieHelper.Get(StaticValues.CookieNameCartCookie);

                    if (!string.IsNullOrWhiteSpace(cookieCart))
                    {
                        var contact = new Contact
                        {
                            Name = name,
                            CreatedDate = DateTime.UtcNow,
                            Email = email,
                            Mobile = phone,
                            IsProductInquiry = true
                        };
                        db.Contacts.Add(contact);
                        db.SaveChanges();

                        var cartDetails = db.CartDetails.Where(s => s.CartCookie == cookieCart).ToList();

                        if (cartDetails.Any())
                        {
                            foreach (var item in cartDetails)
                            {
                                item.ContactId = contact.ContactId;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        //Send Email
                        var tabledata = string.Empty;

                        if (cartDetails.Any())
                        {
                            tabledata =
                                "<table class=\"one-column\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100% \" style=\"border-spacing:0; border-left:1px solid #e8e7e5; border-right:1px solid #e8e7e5; border-bottom:1px solid #e8e7e5; border-top:1px solid #e8e7e5\" bgcolor=\"#FFFFFF\"> <tbody>";
                            double finalTotal = 0;

                            foreach (var item in cartDetails)
                            {
                                var offerApplied = string.Empty;
                                double price = item.Price;
                                price = price * item.Quantity;

                                if (item.Discount > 0)
                                {
                                    price = price - item.Discount;
                                    offerApplied = "<br / ><label style='color: green; font-weight: 400; display: block; font-size: 12px;'>Discount: Rs " + item.OfferTitle + "</label>";
                                }

                                double tax = (price * item.TAX) / 100;
                                price += tax;

                                tabledata += "<tr> " +
                                             "<td style=\"width: 65%; padding: 8px\"> " + item.Product.ProductName +
                                             "" + offerApplied + "</td> " +
                                             "<td style=\"width: 10%; padding: 8px;text-align: center\"> " + item.Product.Price +
                                             "</td> " +
                                             "<td style=\"width: 15%; padding: 8px; text-align: center\">Tax: " +
                                             item.TAX + "%</td> " +
                                             "<td style=\"width: 10%; padding: 8px; text-align: center\">x " +
                                             item.Quantity + "</td> " +
                                             "<td style=\"width: 10%; padding: 8px; text-align: right\"><b>Rs " +
                                             price + "</b></td> </tr>";

                                finalTotal += price;
                            }

                            tabledata +=
                                "<tr> <td colspan=\"5\" style=\"padding: 8px; text-align: right\"><b>Total Amount : Rs " +
                                finalTotal + "</b></td> </tr> </tbody> </table>";
                        }

                        var replacement = new Dictionary<string, string>
                            {
                                { "#name#", name},
                                { "#email#" , !string.IsNullOrEmpty(email)?"Email : " + email:"Email : No email provided"},
                                { "#phone#" , "Phone : " + phone},
                                { "#tabledata#", tabledata}
                            };

                        var emailTemplete =
                            CommonFunctions.GetEmailTemplete((int)EnumList.EmailTemplete.ContactProduct, true,
                                replacement);

                        if (emailTemplete != null)
                        {
                            if (!string.IsNullOrEmpty(email))
                            {
                                //Send to user
                                SendEmail.Send(emailTemplete.Subject, emailTemplete.Body, email);
                            }

                            //Send to admin
                            SendEmail.Send(emailTemplete.Subject, emailTemplete.Body,
                                @System.Web.HttpContext.Current.Application["email"].ToString());
                        }

                        CookieHelper.Delete(StaticValues.CookieNameCartCookie);
                    }

                    return Json(new { status = true, JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public JsonResult Inquiry(string name, string email, string phone)
        {
            try
            {
                using (db)
                {
                    var contact = new Contact
                    {
                        Name = name,
                        CreatedDate = DateTime.UtcNow,
                        Email = email,
                        Mobile = phone,
                        IsProductInquiry = false
                    };
                    db.Contacts.Add(contact);
                    db.SaveChanges();


                    var replacement = new Dictionary<string, string>
                    {
                        { "#name#", name},
                        { "#tabledata#", ""},
                        { "#email#" , !string.IsNullOrEmpty(email)?"Email : " + email:"Email : No email provided"},
                        { "#phone#" , "Phone : " + phone}
                    };

                    var emailTemplete = CommonFunctions.GetEmailTemplete((int)EnumList.EmailTemplete.ContactProduct, true, replacement);

                    if (emailTemplete != null)
                    {
                        if (!string.IsNullOrEmpty(email))
                        {
                            //Send to user
                            SendEmail.Send(emailTemplete.Subject, emailTemplete.Body, email);
                        }

                        //Send to admin
                        SendEmail.Send(emailTemplete.Subject, emailTemplete.Body, System.Web.HttpContext.Current.Application["email"].ToString());
                    }


                    return Json(new { status = true, JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, JsonRequestBehavior.AllowGet });
            }
        }
    }
}