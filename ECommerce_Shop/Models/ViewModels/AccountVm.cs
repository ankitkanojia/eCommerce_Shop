using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECommerce_Shop.Helpers;

namespace ECommerce_Shop.Models.ViewModels
{
    public class LockoutVm
    {
        public string Email { get; set; }
        public DateTime? UnlockDate { get; set; }
    }

    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = ValidationMessages.PasswordLength, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = ValidationMessages.ComparePassword)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class ForgotPasswordVm
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsShowSuccessMessage { get; set; }
    }

    public class LoginVm
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public bool IsShowSendVerificationEmail { get; set; }

        public string UserId { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = ValidationMessages.PasswordLength, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = ValidationMessages.ComparePassword)]
        public string ConfirmPassword { get; set; }

        [MustBeTrue(ErrorMessage = ValidationMessages.TermsAccept)]
        public bool IsAcceptTnC { get; set; }

        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        public bool IsShowEmailConfirm { get; set; }

    }

    public class MyProfileVm
    {
        public string Email { get; set; }

        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ProfileImage")]
        public string ProfileImage { get; set; }


        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Tagline")]
        public string Tagline { get; set; }

        [Display(Name = "Gender")]
        [StringLength(6, ErrorMessage = ValidationMessages.StringLength)]
        public string Gender { get; set; }

        [Display(Name = "DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

    }

    public class ChangePasswordVm
    {
        [Required]
        [StringLength(100, ErrorMessage = ValidationMessages.PasswordLength, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = ValidationMessages.PasswordLength, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = ValidationMessages.ComparePassword)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}