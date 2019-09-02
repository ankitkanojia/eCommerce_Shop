using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Helpers
{
    public static class StaticValues
    {
        public static string CookieNameCartCookie => "CartCookie";
        public static string CategoryImagePath => "~/Uploads/Category/";
        public static string AdvertisementImagePath => "~/Uploads/Advertisement/";
        public static string ProductImagePath => "~/Uploads/Product/";
        public static string ContainerImagePath => "~/Uploads/Image/";
        public static string ContainerVideoPath => "~/Uploads/Video/";
        public static string ConfigurationPath => "~/Uploads/Configuration/";
        public static string AvatarImagePath => "~/Uploads/Avatar/";

        public static string SliderImagePath => "~/Uploads/Slider/";
        public static string OfferImagePath => "~/Uploads/Offer/";
        public static string CoverImagePath => "~/Uploads/Cover/";
        public static string PartnerImagePath => "~/Uploads/Partner/";

        public static string configureLogoPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/logo")).GetFiles().Select(o => o.Name).FirstOrDefault();
        public static string configureWomenCoverPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/womencover")).GetFiles().Select(o => o.Name).FirstOrDefault();
        public static string configureMenCoverPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/mencover")).GetFiles().Select(o => o.Name).FirstOrDefault();
        public static string configureKidsCoverPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/kidscover")).GetFiles().Select(o => o.Name).FirstOrDefault();
        public static string configureUniformCoverPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/uniformcover")).GetFiles().Select(o => o.Name).FirstOrDefault();
        public static string configureStudioCoverPath => new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Configuration/studiocover")).GetFiles().Select(o => o.Name).FirstOrDefault();

        public static string DefaultAvatarImagePath => "~/Images/avatar.jpg";
        public static string DefaultProductImagePath => "/Images/Backend/defaultProduct.png";

        public static string SendGridAppKey => "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";

        public static string RoleDeveloper => "Admin";
        public static string UNIFORMCategory => "44";
        public static string STUDIOCategory => "45";
        public static string SERVICECategory => "5";
        public static string UserId => "8fc66af4-4bfc-419c-b791-ab36c41b6f0b";
        public static string CookieProfileImage => "ProfileImage";
        public static string CookieFullName => "CookieFullName";
        public static string CookieRoleName => "CookieRoleName";

        public static string EmailCopyrightName => "Riowebs System";
        public static string EmailCopyrightYear => Convert.ToString(DateTime.UtcNow.Year);
        public static string EmailWebsite => "https://www.riowebs.com";
        public static string EmailSignatureName => "Riowebs System";
        public static string EmailSignatureImg => "https://gallery.mailchimp.com/fdcaf86ecc5056741eb5cbc18/_compresseds/da24cf15-10e5-4af3-b8f7-56013fdde0e0.jpg";
        public static string EmaiLinkFacebook => "https://www.riowebs.com";
        public static string EmaiLinkTwitter => "https://www.riowebs.com";
        public static string EmaiLinkLinkedin => "https://www.riowebs.com";
        public static string EmaiLogoLight => "https://gallery.mailchimp.com/fdcaf86ecc5056741eb5cbc18/images/13f425ab-c680-4ae0-88de-7b493d95095f.jpg";
        public static string EmaiLogodDark => "https://gallery.mailchimp.com/fdcaf86ecc5056741eb5cbc18/images/dbe9c57f-5e00-4d9f-9719-5d36a9a02ebc.jpg";

        public static string SiteFirstName => ConfigurationManager.AppSettings["SiteFirstName"];
        public static string SiteLastName => ConfigurationManager.AppSettings["SiteLastName"];
        public static string DevelopementCompany => ConfigurationManager.AppSettings["DevelopementCompany"];
        public static string DevelopementCompanyURL => ConfigurationManager.AppSettings["DevelopementCompanyURL"];
        public static string MainLogo => ConfigurationManager.AppSettings["MainLogo"];
        public static string SubMainLogo => ConfigurationManager.AppSettings["SubMainLogo"];
        public static string feviconURL => ConfigurationManager.AppSettings["feviconURL"];
    }
}