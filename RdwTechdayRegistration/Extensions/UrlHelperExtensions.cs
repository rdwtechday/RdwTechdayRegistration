using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RdwTechdayRegistration.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string RegisterNonRDWCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.RegisterNonRdwCallback),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string LoginLink(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.Login),
                controller: "Account",
                values: null,
                protocol: scheme);
        }
        public static string PrivacyLink(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Action(
                action: nameof(HomeController.Privacy),
                controller: "Home",
                values: null,
                protocol: scheme);
        }

    }
}
