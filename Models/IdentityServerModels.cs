using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IdentityServer4;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdsServer.Data;
using System.Linq;
using System.Security.Claims;

namespace IdsServer.Models

{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        public string SubjectId{get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
         public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        // public virtual ICollection<UserGroup> UserGroups { get; set; }

    }
    public class ApplicationUserRole :IdentityRole<int>
    {

        
    }
    public class ApplicationUserClaim :IdentityUserClaim<int>
    {

        
    }

    public class ApplicationUserLogin:IdentityUserLogin<int>
    {

    }
       public class ApplicationUserToken:IdentityUserToken<int>
    {

    }
    public  class ApplicationUserStore :  UserStore<ApplicationUser, ApplicationUserRole, ApplicationDbContext, int>
    {
        private readonly ApplicationDbContext _Context;
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer)
            : base(context, describer)
        {
                    _Context=context;
        }
        // public override int ConvertIdFromString(string id)
        // {
        //     return new Guid(id);
        // }
        public override Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
                var user=_Context.Set<ApplicationUser>().Where(u=>u.UserName.Equals(userId)).FirstOrDefault();

                return Task.FromResult(user);
        }
      
      
    }
    // public class UserGroup
    // {
    //     public int Id { get; set; }
    //     public string GroupName { get; set; }
    //     public int UserId { get; set; }

    // }
}
