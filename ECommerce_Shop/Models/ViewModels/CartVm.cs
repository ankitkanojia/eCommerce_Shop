using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Models.ViewModels
{
    public class CartVm
    {
        public long CartDetailId { get; set; }
        public long ContactId { get; set; }
        public string CartCookie { get; set; }
        public string OfferTitle { get; set; }
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double MRP { get; set; }
        public double Price { get; set; }
        public double TAX { get; set; }
        public double Discount { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
    }
}