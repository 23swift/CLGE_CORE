using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdsServer
{
    public class ProfileService: IProfileService
    {
          protected UserManager<ApplicationUser> _userManager;
            private  RoleManager<ApplicationRole> _roleManager;
            public ProfileService(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
            {
                _userManager = userManager;
                _roleManager=roleManager;
            }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
             var user = await _userManager.GetUserAsync(context.Subject);
             var roles = await _userManager.GetRolesAsync(user);
            var roleClaims=new List<Claim>();
             foreach (var item in roles)
             {

                 roleClaims.AddRange(
                        _roleManager.GetClaimsAsync(_roleManager.FindByNameAsync(item).Result).Result

                 );
             }
            

             var claimsNames = new List<string>();
                claimsNames.AddRange(new[]{
                    
                //    "Permission",
                //     "username",
                "dashboard",
                "route",
                "rank",
                "group",
                "system",
                "userId",
                
                   
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Name
                });

                context.RequestedClaimTypes = claimsNames;
                context.AddRequestedClaims(context.Subject.Claims);
                var claims = new List<Claim>();
                claims.Add(new Claim("username", user.UserName));
                // claims.Add(new Claim( JwtClaimTypes.Role, "AO Checker" ));
                //  claims.Add(new Claim( JwtClaimTypes.Name, "Bob Smith" ));
              
                // context.AddRequestedClaims(roleClaims);
                // context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

                 context.IsActive = (user != null);
        }
    }
}