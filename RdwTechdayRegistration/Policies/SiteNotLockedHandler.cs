using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using RdwTechdayRegistration.Utility;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Policies
{
    public class SiteNotLockedHandler : AuthorizationHandler<SiteNotLockedRequirement>
    {

        IProvider<ApplicationDbContext> scopedServiceProvider;

        public SiteNotLockedHandler(IProvider<ApplicationDbContext> scopedServiceProvider)
        {
            this.scopedServiceProvider = scopedServiceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   SiteNotLockedRequirement requirement)
        {
            if (context.User.IsInRole("Admin") || requirement.ActionAllowed)
            {
                context.Succeed(requirement);
            }
            else
            {
                ApplicationDbContext dbcontext = scopedServiceProvider.Get();
                Maxima maxima = await  dbcontext.Maxima.FirstOrDefaultAsync();

                if ( !maxima.SiteHasBeenLocked ) {
                    context.Succeed(requirement);
                }

             }
        }
    }
}