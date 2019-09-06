using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    public class AuthController : Controller
    {
        #region --> Class members & constructor

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AuthController()
        {
        }

        public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        #endregion


        #region --> Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


        //private async Task Registration()
        //{
        //    var user = new ApplicationUser { UserName = "admin@bosskinds.com", Email = "admin@bosskinds.com", FullName = "bosskinds" };
        //    var result = await UserManager.CreateAsync(user, "Bosskinds@1620");

        //    if (result.Succeeded)
        //    {
        //        //Add role
        //        var resultRole = UserManager.AddToRole(user.Id, StaticValues.RoleDeveloper);

        //        if (resultRole.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //        }
        //    }
        //}

        #region --> Login

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {

            return View(new LoginVm { IsShowSendVerificationEmail = false, ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            //Add this to check if the email was confirmed.
            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                ModelState.AddModelError("", "You need to confirm your email.");
                model.IsShowSendVerificationEmail = true;
                model.UserId = user.Id;
                return View(model);
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return View("Lockout", new LockoutVm { Email = user.Email, UnlockDate = user.LockoutEndDateUtc });
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    CookieHelper.Set(StaticValues.CookieProfileImage, user.ProfileImage);
                    CookieHelper.Set(StaticValues.CookieFullName, user.FullName);
                    var userRole = await UserManager.GetRolesAsync(user.Id);
                    if (userRole.Any())
                    {
                        CookieHelper.Set(StaticValues.CookieRoleName, string.Join(",", userRole));
                    }

                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout", new LockoutVm { Email = user.Email, UnlockDate = user.LockoutEndDateUtc });
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { model.ReturnUrl, model.RememberMe });
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        #endregion

        #region --> Forgot Password

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordVm { IsShowSuccessMessage = false });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordVm model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(new ForgotPasswordVm { IsShowSuccessMessage = true });
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var replacement = new Dictionary<string, string>
                {
                    { "#name#", user.UserName},
                    { "#resetlink#", callbackUrl}
                };

                var emailTemplete = CommonFunctions.GetEmailTemplete((int)EnumList.EmailTemplete.ResetPassword, true, replacement);

                await UserManager.SendEmailAsync(user.Id, emailTemplete.Subject, emailTemplete.Body);

                return View(new ForgotPasswordVm { IsShowSuccessMessage = true });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region --> Reset Password

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Login", "Home");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Home");
            }
            AddErrors(result);
            return View();
        }
        #endregion

        #region --> LogOff | Logout
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Auth");
        }
        #endregion
    }
}