using System;
using System.Collections.Generic;
using System.Linq;
using CLGE_CORE.Models;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdsServer {
  [Authorize]
  public class SystemController : Controller {

    // ILogger<SystemController> _logger;

    // public SystemController(ILogger<SystemController> logger)
    // {
    //   _logger = logger;
    // }
    private readonly IConfigurationDbContext _configDbContext;
    public SystemController (IConfigurationDbContext configDbContext) {

      _configDbContext = configDbContext;
    }
    public IActionResult Index () {
      var clients = _configDbContext.Clients.ToList ();
      ViewBag.clients = clients;
      return View ();
    }

    public IActionResult Modules (int? clientId) {
      //TODO: Implement Realistic Implementation
      List<ClientProperty> clientProperties = new List<ClientProperty> ();
      var clientApp = _configDbContext.Clients.Include ("Properties").Where (c => c.Id.Equals ((int) clientId)).FirstOrDefault ();

      if (clientApp.Properties != null) { clientProperties.AddRange (clientApp.Properties); }

      ViewBag.systemName = clientApp.ClientName;
      ViewBag.clientId = clientApp.Id;
      return View (clientProperties);
    }

    public IActionResult NewModule (int? clientId) {
      //TODO: Implement Realistic Implementation
      var clientApp = _configDbContext.Clients.Where (c => c.Id.Equals ((int) clientId)).FirstOrDefault ();
      ViewBag.ClientId = clientId;
      ViewBag.systemName = clientApp.ClientName;
      return View ();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NewModule (IdentityServer4.EntityFramework.Entities.ClientProperty property, int? clientId) {
      if (ModelState.IsValid) {

        var clientApp = _configDbContext.Clients.Find (clientId);
        clientApp.Properties = new List<ClientProperty> ();
        clientApp.Properties.Add (property);
        _configDbContext.SaveChanges ();

      }

      //TODO: Implement Realistic Implementation
      return RedirectToAction ("Modules", new { clientId = clientId });
    }

    public IActionResult RegisterSystem () {
      //TODO: Implement Realistic Implementation
      return View ();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RegisterSystem (SystemViewModel systemViewModel) {
      if (ModelState.IsValid) {

        var client = CreateNewClient (systemViewModel).ToEntity ();

        _configDbContext.Clients.Add (client);
        _configDbContext.SaveChanges ();

        if (systemViewModel.Configure) {
          return RedirectToAction ("UpdateDetails", new { clientId = client.Id });
        } else {
          return RedirectToAction ("Index");
        }
      }

      //TODO: Implement Realistic Implementation

      return View (systemViewModel);
    }
    public IActionResult UpdateDetails (int? clientId) {
      //TODO: Implement Realistic Implementation
      var clientApp = _configDbContext.Clients.Find (clientId);
      var systemViewModel = new SystemViewModel {
        Id = clientApp.Id, ClientUri = clientApp.ClientUri, SystemName = clientApp.ClientName,
        Description = clientApp.Description

      };
      ViewBag.clientId = clientId;
      ViewBag.systemName = clientApp.ClientName;
      return View (systemViewModel);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult UpdateDetails (SystemViewModel systemViewModel) {
      //TODO: Implement Realistic Implementation
      if (ModelState.IsValid) {
        var clientApp = _configDbContext.Clients.Find (systemViewModel.Id);

        clientApp.ClientUri = systemViewModel.ClientUri;
        clientApp.ClientName = systemViewModel.SystemName;
        clientApp.Description = systemViewModel.Description;
        _configDbContext.Clients.Update (clientApp);
        _configDbContext.SaveChanges ();
        ViewBag.clientId = systemViewModel.Id;
        ViewBag.systemName = clientApp.ClientName;
        return RedirectToAction ("Index");
      }
      return View (systemViewModel);

    }
    private IdentityServer4.Models.Client CreateNewClient (SystemViewModel systemViewModel) {

      var client = new IdentityServer4.Models.Client {
        ClientId = Guid.NewGuid ().ToString (),
        ClientName = systemViewModel.SystemName,
        AllowedGrantTypes = GrantTypes.Hybrid,
        ClientUri = systemViewModel.ClientUri,
        Description = systemViewModel.Description,
        ClientSecrets = {
        new IdentityServer4.Models.Secret ("secret".Sha256 ())
        },

        RedirectUris = { systemViewModel.ClientUri + "/signin-oidc" },
        PostLogoutRedirectUris = { systemViewModel.ClientUri + "/signout-callback-oidc" },

        AllowedScopes = {
        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,

        "api1",
        "access.profile",
        "role",
        "token"
        },

        AllowOfflineAccess = true,
        AllowAccessTokensViaBrowser = true
      };

      return client;
    }
  }
}