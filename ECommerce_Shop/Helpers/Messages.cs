using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Helpers
{
    public static class ValidationMessages
    {
        public const string PasswordLength = "The {0} must be at least {2} characters long!";
        public const string ComparePassword = "The password and confirmation password do not match!";
        public const string TermsAccept = "You gotta tick the terms and condition box!";
        public const string StringLength = "Maximum {0} character allowed!";
    }

    public static class SuccessMessage
    {
        public const string Updated = "Data updated succssfully!";
        public const string Added = "Data added succssfully!";
        public const string Activated = "Data activated succssfully!";
        public const string DeActivated = "Data deactivated succssfully!";
        public const string Deleted = "Data deleted succssfully!";
        public const string Cloned = "Data cloned succssfully!";
        public const string Founded = "Data founded succssfully!";
        public const string ImageAdded = "Image added succssfully!";
        public const string ImageDelete = "Image deleted succssfully!";
    }

    public static class ErrorMessage
    {
        public const string SessionExpired = "Session expired! Please login again!";
        public const string DataNotFound = "Data not found!";
        public const string ReferenceDataFound = "Reference data(s) found, \n Data can not deleted!";
    }
}
