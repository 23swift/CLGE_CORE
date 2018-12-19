using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

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

    // public class UserGroup
    // {
    //     public int Id { get; set; }
    //     public string GroupName { get; set; }
    //     public int UserId { get; set; }

    // }
}
