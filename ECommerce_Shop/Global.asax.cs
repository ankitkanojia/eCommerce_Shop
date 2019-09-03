using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;

namespace ECommerce_Shop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private DBEntities _db = new DBEntities();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (_db = new DBEntities())
            {
                var configurations = _db.Configurations.ToList();
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application["mobileno"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["whatsappno"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.WHATSAPP).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["email"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.EMAIL).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["fblink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.FBLink).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["twlink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.TWLink).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["inslink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.INSLink).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["lilink"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.LILink).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["aboutus"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.ABOUTUS).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["termsandconditions"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.MOBILENO) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.TERMSANDCONDITIONS).ConfigurationValue : "not found";
                System.Web.HttpContext.Current.Application["privacypolicy"] = configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.PRIVACYPOLICY) != null ? configurations.FirstOrDefault(m => m.ConfigurationId == (long)EnumList.Configurations.PRIVACYPOLICY).ConfigurationValue : "not found";
            }

        }
    }
}
