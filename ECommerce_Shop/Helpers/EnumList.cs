using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Helpers
{
    public static class EnumList
    {
        public enum EmailTemplete
        {
            VerificationEmail = 1,
            ResetPassword = 2,
            ContactProduct = 3
        }

        public enum ImageType
        {
            SLIDER = 1,
            WOMENCOVER = 2,
            MENCOVER = 3,
            KIDSCOVER = 4,
            STUDIOCOVER = 5,
            UNIFORMCOVER = 6,
            PARTNERS = 7,
            OFFERS = 8
        }

        public enum Configurations
        {
            LOGO = 1,
            WOMENCOVER = 2,
            MENCOVER = 3,
            KIDSCOVER = 4,
            UNIFORMCOVER = 5,
            STUDIOCOVER = 6,
            MOBILENO = 7,
            WHATSAPP = 8,
            EMAIL = 9,
            ABOUTUS = 10,
            TERMSANDCONDITIONS = 11,
            PRIVACYPOLICY = 12,
            FBLink = 13,
            TWLink = 14,
            INSLink = 15,
            LILink = 16
        }

        public enum DiscountTypes
        {
            FlatDiscount = 1,
            Percentage = 2,
            SpecificOffer = 3
        }
    }
}