/*  Copyright (C) 2018, RDW 
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/.  
 *  
 */

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
