using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdsServer.Data;
using IdsServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdsServer {
   [Authorize]
  public class RoleController : Controller {

    // ILogger<RoleController> _logger;

    // public RoleController(ILogger<RoleController> logger)
    // {
    //   _logger = logger;
    // }

    private readonly RoleManager<ApplicationRole> _roleMager;
    private readonly IConfigurationDbContext _configDbContext;
    private readonly ApplicationDbContext _appDbContext;
    public RoleController (RoleManager<ApplicationRole> roleMager,
      IConfigurationDbContext configDbContext, ApplicationDbContext appDbContext) {

      _roleMager = roleMager;
      _configDbContext = configDbContext;
      _appDbContext = appDbContext;
    }
    public IActionResult Index (int? clientId) {
      if (clientId == null) {

        return NotFound ();
      }
      var clientApp = _configDbContext.Clients.Where (c => c.Id.Equals ((int) clientId)).FirstOrDefault ();
      if (clientApp == null) {

        return NotFound ();
      }

      ViewBag.clientId = clientId;
      ViewBag.systemName = clientApp.ClientName;
      var roleList = _roleMager.Roles.Where (r => r.Client.Equals ((int) clientId)).ToList ();
      if (roleList == null) {

        return NotFound ();
      }
      return View (roleList);
    }
    public IActionResult Create (int? clientId) {
      var clientApp = _configDbContext.Clients.Where (c => c.Id.Equals ((int) clientId)).FirstOrDefault ();
      ViewBag.ClientId = clientId;
      ViewBag.systemName = clientApp.ClientName;
      return View ();
    }

    [HttpPost]
    public async Task<IActionResult> Create ([Bind ("Name")] ApplicationRole role, int ClientId) {
      var c = _configDbContext.Clients.Find (ClientId);
      role.Client = ClientId;
      //  role.RoleClaims = new List<ApplicationRoleClaim> {
      //                       new ApplicationRoleClaim { ClaimType = "access", ClaimValue = "aoEncoder", Name = "AO Encoder Route", Description = "" },
      //                       new ApplicationRoleClaim { ClaimType = "access", ClaimValue = "aoEncoderRpt", Name = "AO Encoder Report", Description = "" },
      //                   };

      await _roleMager.CreateAsync (role);
      //await _appDbContext.Roles.AddAsync(role);
      // await _appDbContext.SaveChangesAsync();
      // await _roleMager.CreateAsync (role);
      //  await _roleMager.AddClaimAsync (role, new Claim ( "dashboard", "aoDasboard"));
      // await _roleMager.AddClaimAsync (role, new Claim ("route", "newAff"));
      // await _roleMager.AddClaimAsync (role, new Claim ("route", "branchAf"));

      // await _roleMager.UpdateAsync(role);

      return RedirectToAction ("Index", new { clientId = ClientId });
    }

    public IActionResult RoleAccess (int? clientId, int? roleId) {
      if (clientId == null || roleId == null) {
        return NotFound ();
      }
      var role = _roleMager.Roles.Include ("RoleClaims").Where (r => r.Id.Equals (roleId)).FirstOrDefault ();
      // List<ClientProperty> clientProperties = new List<ClientProperty> ();
      var roleClaims = role.RoleClaims.ToList ();
      // List<SelectListItem> accessList=
      var clientApp = _configDbContext.Clients.Include ("Properties").Where (c => c.Id.Equals ((int) clientId)).FirstOrDefault ();

      List<SelectListItem> accessList = clientApp.Properties.Select (p => new SelectListItem {
        Text = p.Key, Value = p.Value, Selected = false
      }).ToList ();
      accessList = GetSelectedItems (accessList, roleClaims);
      // if (clientApp.Properties != null) { clientProperties.AddRange (clientApp.Properties); }
      ModuleSelectListViewModel moduleSelectListViewModel = new ModuleSelectListViewModel {
        roleId = (int) roleId, SelectedModules = accessList

      };

      ViewBag.systemName = clientApp.ClientName;
      // ViewBag.moduleList = clientProperties;
      ViewBag.moduleList = moduleSelectListViewModel;
      var claimLis = role.RoleClaims.ToList ();
      ViewBag.clientId = clientId;
      ViewBag.systemName = clientApp.ClientName;
      ViewBag.roleName = role.Name;
      ViewBag.roleId = roleId;
      //TODO: Implement Realistic Implementation
      return View (claimLis);
    }

    [HttpPost]
    public IActionResult CreateRoleAccess (ModuleSelectListViewModel selectedAccess, int? clientId, int? roleId) {
      if (clientId == null || roleId == null) {
        return NotFound ();
      }
      var role = _roleMager.Roles.Include ("RoleClaims").Where (r => r.Id.Equals (roleId)).FirstOrDefault ();
      // var role=_roleMager("RoleClaims").FindByIdAsync(((int)roleId).ToString());
      if( role.RoleClaims!=null){role.RoleClaims.Clear();}
      if (selectedAccess.SelectedModules != null) {
        foreach (var item in selectedAccess.SelectedModules) {
          if (item.Selected) {
            role.RoleClaims.Add (new ApplicationRoleClaim {
              Name = item.Text, ClaimType = "access", ClaimValue = item.Value
            });

            // _roleMager.AddClaimAsync(role,new Claim("access",item.Value ));

          }

        }

        _roleMager.UpdateAsync (role);

      }
      ViewBag.clientId = clientId;
      ViewBag.roleId = roleId;
      return RedirectToAction ("RoleAccess", new { clientId = clientId,roleId=roleId });
    }
    
    
  
    public IActionResult Edit(int? clientId, int? roleId)
    {
      //TODO: Implement Realistic Implementation
       var role = _roleMager.Roles.First(r=>r.Id.Equals((int)roleId));
      ViewBag.ClientId=clientId;

      return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
     public IActionResult Edit([Bind ("Name,Id,ConcurrencyStamp,Client")] ApplicationRole roleVm, int? clientId)
    {
      //TODO: Implement Realistic Implementation
      
      _roleMager.UpdateAsync(roleVm);
       
      ViewBag.ClientId=clientId;

      return RedirectToAction ("Index", new { clientId = clientId});
    }
    private List<SelectListItem> GetSelectedItems (List<SelectListItem> masterList, List<ApplicationRoleClaim> roleClaims) {
      var result = masterList;

      foreach (var item in masterList) {
        foreach (var roleClaim in roleClaims) {
          if (item.Value.Equals (roleClaim.ClaimValue)) {

            item.Selected = true;
          }
        }

      }
      return result;
    }
  }
}