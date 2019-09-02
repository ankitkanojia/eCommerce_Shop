using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Models.ViewModels
{
    //Contact - Inquiry
    public class ContactVm
    {
        public long ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public double TotalAmount { get; set; }
        public bool IsProductInquiry { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CartDetailVm> CartDetails { get; set; }
    }

    public class CartDetailVm
    {
        public long CartDetailId { get; set; }
        public long ContactId { get; set; }
        public string CartCookie { get; set; }
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public double MRP { get; set; }
        public double Price { get; set; }
        public double PayableAmount { get; set; }
        public double TAX { get; set; }
        public double Discount { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
    }
}