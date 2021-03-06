﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using IdsServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdsServer.Models

{
    // Add profile data for application users by adding properties to the ApplicationUser class
    // public class ApplicationUser : IdentityUser<int>
    public class ApplicationUser : IdentityUser<int> {
        public string SubjectId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        //  public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        // public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<AppUserClient> Clients { get; set; }

    }
    public class AppUserClient {
        public int id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //[ForeignKey("Client")]
        public int ClientId { get; set; }
        // public virtual Client Client{get;set;}

    }

    public class AppClientClaim : ClientClaim {
        public string Descrption { get; set; }

    }
    // public class ApplicationRole :IdentityRole<int>
    public class ApplicationRole : IdentityRole<int> {
    public ApplicationRole() : base() { }
    public ApplicationRole(string name) : base(name) { }
        //    

        // [ForeignKey("Clients")]
        // public int ClientId{get;set;}

        public int Client { get; set; }
        public string Description { get; set; }
        // public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

    }
    public class ApplicationRoleClaim : IdentityRoleClaim<int> {

        //    

        // [ForeignKey("Clients")]
        // public int ClientId{get;set;}
        public virtual ApplicationRole ApplicationRole { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }

    }
    public class ApplicationUserRole : IdentityUserRole<int> {

    }

    public class ApplicationUserClaim : IdentityUserClaim<int> {

    }

    public class ApplicationUserLogin : IdentityUserLogin<int> {

    }
    public class ApplicationUserToken : IdentityUserToken<int> {

    }
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, int> {
        private readonly ApplicationDbContext _Context;
        public ApplicationUserStore (ApplicationDbContext context, IdentityErrorDescriber describer) : base (context, describer) {
            _Context = context;
        }
        // public override int ConvertIdFromString(string id)
        // {
        //     return new Guid(id);
        // }
        public override Task<ApplicationUser> FindByIdAsync (string userId, CancellationToken cancellationToken = default (CancellationToken)) {
            var user = _Context.Set<ApplicationUser> ().Where (u => u.UserName.Equals (userId)).FirstOrDefault ();

            return Task.FromResult (user);
        }

    }
    // public class UserGroup
    // {
    //     public int Id { get; set; }
    //     public string GroupName { get; set; }
    //     public int UserId { get; set; }

    // }

    public class ApplicationUserManager : UserManager<ApplicationUser> {
        public ApplicationUserManager (IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base (store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }
    }

    // public class ApplicationRoleManager : RoleManager<ApplicationRole>
    // {


    //     // public static ApplicationRoleManager Create (
    //     //     IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context) {
    //     //     return new ApplicationRoleManager (
    //     //         new RoleStore<ApplicationRole> (context.Get<ApplicationDbContext> ()));
    //     // }
    //     public ApplicationRoleManager(IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
    //     {
    //     }


    // }

}