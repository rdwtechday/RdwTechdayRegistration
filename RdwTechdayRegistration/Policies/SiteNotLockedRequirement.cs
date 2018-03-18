using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Policies
{
    public class SiteNotLockedRequirement : IAuthorizationRequirement
    {
        public bool ActionAllowed;
        public SiteNotLockedRequirement(bool actionAllowed = false)
        {
            ActionAllowed = actionAllowed;
        }
    }
}
