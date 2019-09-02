using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ECommerce_Shop.Models.ViewModels
{
    public class ProductVm
    {
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public long? OfferId { get; set; }
        public string OfferTitle { get; set; }
        public long CategoryId { get; set; }
        public long FilterDataId { get; set; }
        public long? SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SKU { get; set; }
        public double MRP { get; set; }
        public double Price { get; set; }
        public double TAX { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public double Discount { get; set; }
        public DateTime? DiscountStartOn { get; set; }
        public DateTime? DiscountEndOn { get; set; }
        public string Tag { get; set; }
        public bool IsActive { get; set; }
        public List<ProductDetailsVm> ProductDetails { get; set; }
        public List<ProductFeatureDetailsVm> ProductFeatureDetails { get; set; }
        public List<ProductImageArrayVm> ProductImagesDetails { get; set; }
        public string CoverImage { get; set; }
        public string ProductImages { get; set; }
        public DateTime CreatedDate{ get; set; }
    }

    public class ProductDetailsVm
    {
        public long SizeId { get; set; }
        public long ColorId { get; set; }
        public int StockQty { get; set; }
    }

    public class ProductFeatureDetailsVm
    {
        public string FeatureValue { get; set; }
        public string FeatureType { get; set; }
    }

    public class ProductImageArrayVm
    {
        public bool iscover { get; set; }
        public bool issaved { get; set; }
        public bool isdeleted { get; set; }
        public string currentId { get; set; }
        public string extension { get; set; }
        public string imageSrc { get; set; }

        public long ProductImageId { get; set; }
        public long key { get; set; }
        public long ProductId { get; set; }
        public string ImageName { get; set; }
    }
}