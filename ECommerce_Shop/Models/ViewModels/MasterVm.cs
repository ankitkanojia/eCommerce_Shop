using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Models.ViewModels
{
    //Email Template
    public class EmailTemplateVm
    {
        public long EmailTempleteId { get; set; }

        [Required]
        [Display(Name = "TemplateName")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "Tags")]
        public string HashTag { get; set; }

        [Required]
        [Display(Name = "From")]
        public string FromId { get; set; }
        public string BccId { get; set; }
        public bool IsActive { get; set; }
    }

    //Brand
    public class BrandVm
    {
        public long BrandId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class BrandPageVm
    {
        public long BrandId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public List<BrandVm> brandDetails { get; set; }
    }

    //Color
    public class ColorVm
    {
        public long ColorId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hex")]
        public string Hex { get; set; }

        public bool IsChecked { get; set; }
    }

    public class ColorPageVm
    {
        public long ColorId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hex")]
        public string Hex { get; set; }

        public List<ColorVm> colorDetails { get; set; }
    }

    //Size
    public class SizeVm
    {
        public long SizeId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

        public bool IsChecked { get; set; }
    }

    public class SizePageVm
    {
        public long SizeId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public long CategoryId { get; set; }

        public List<SizeVm> sizeDetails { get; set; }
    }

    //Category
    public class CategoryVm
    {
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "ImageName")]
        public string ImageName { get; set; }

        [Display(Name = "RootCategoryName")]
        public string RootCategoryName { get; set; }
        public string CustomUrl { get; set; }
        public long? RootCategoryId { get; set; }

        public bool IsActive { get; set; }
    }

    public class CategoryPageVm
    {
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "ImageName")]
        public string ImageName { get; set; }
        public string CustomUrl { get; set; }
        public long? RootCategoryId { get; set; }

        public List<CategoryVm> categoryDetails { get; set; }
    }

    //Video
    public class ContainerVm
    {
        public long VideoId { get; set; }
        public long CategoryId { get; set; }
        public string VideoName { get; set; }
        public string ImageName { get; set; }
        public HttpPostedFileBase UploadedImage { get; set; }
        public HttpPostedFileBase UploadedVideo { get; set; }
        public bool IsVideo { get; set; }
    }

    public class ContainerPageVm
    {   
        public long VideoId { get; set; }
        public long CategoryId { get; set; }
        public string VideoName { get; set; }
        public string ImageName { get; set; }
        public HttpPostedFileBase UploadedImage { get; set; }
        public HttpPostedFileBase UploadedVideo { get; set; }
        public bool IsVideo { get; set; }
        public string CategoryName { get; set; }
        public List<ContainerVm> videoDetails { get; set; }
    }

    //FAQ
    public class FAQVm
    {
        public long FAQId { get; set; }

        [Display(Name = "Question")]
        public string Question { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }

        public DateTime Date { get; set; }
    }

    //Filter
    public class FilterDataVm
    {
        public long FilterDataId { get; set; }
        public long CategoryId { get; set; }
        public long FilterTitleId { get; set; }
        public string TitleName { get; set; }
        public string FilterName { get; set; }
        public bool IsChecked { get; set; }
    }

    

    public class FilterTitleVm
    {
        public long FilterTitleId { get; set; }
        public string Name { get; set; }
    }


    //Container
    public class FileManagerVm
    {
        public long ImageId { get; set; }
        public long typeId { get; set; }
        public string ImageName { get; set; }
    }

    public class FileManagerPageVm
    {
        public long ImageId { get; set; }
        public long typeId { get; set; }
        public string type { get; set; }
        public string imagePath { get; set; }
        public string ImageName { get; set; }
        public List<FileManagerVm> containerDetails { get; set; }
    }

    public class ConfigurationVm
    {
        public long ConfigurationId { get; set; }
        public string ConfigurationType { get; set; }
        public string ConfigurationValue { get; set; }
        public HttpPostedFileBase logoContainer { get; set; }
        public HttpPostedFileBase womenContainer { get; set; }
        public HttpPostedFileBase menContainer { get; set; }
        public HttpPostedFileBase kidsContainer { get; set; }
        public HttpPostedFileBase uniformContainer { get; set; }
        public HttpPostedFileBase studioContainer { get; set; }
        public string mobileno { get; set; }
        public string whatsappno { get; set; }
        public string email { get; set; }
        public string about { get; set; }
        public string terms { get; set; }
        public string privacypolicy { get; set; }
        public string fblink { get; set; }
        public string twlink { get; set; }
        public string inslink { get; set; }
        public string lilink { get; set; }
    }

    //Collection Point
    public class CollectionPointVm
    {
        public long CollectionPointId { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }

    public class CollectionPointPageVm
    {
        public long CollectionPointId { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public List<CollectionPointVm> Collections { get; set; }
    }

    //Advertisement
    public class AdvertisementVm
    {
        public long AdvertisementId { get; set; }
        public string LinkUrl { get; set; }
        public string HImageUrl { get; set; }
        public string VImageUrl { get; set; }
    }

    public class AdvertisementPageVm
    {
        public long AdvertisementId { get; set; }
        public string LinkUrl { get; set; }
        public string HImageUrl { get; set; }
        public string VImageUrl { get; set; }

        public List<AdvertisementVm> Collections { get; set; }
    }

    //Offer
    public class OfferVm
    {
        public long OfferId { get; set; }
        public string Title { get; set; }
        public string DiscountType { get; set; }
        public Nullable<long> DiscountId { get; set; }
        public Nullable<long> DiscountPercentage { get; set; }
        public Nullable<long> FlatDiscount { get; set; }
        public Nullable<long> SpecificItems { get; set; }
        public Nullable<long> SpecificValue { get; set; }
    }

    public class OfferPageVm
    {
        public long OfferId { get; set; }
        public string Title { get; set; }
        public string DiscountType { get; set; }
        public Nullable<long> DiscountId { get; set; }
        public Nullable<long> DiscountPercentage { get; set; }
        public Nullable<long> FlatDiscount { get; set; }
        public Nullable<long> SpecificItems { get; set; }
        public Nullable<long> SpecificValue { get; set; }

        public List<OfferVm> Collections { get; set; }
    }

}