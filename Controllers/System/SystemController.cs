using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
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
      var clientApp = _configDbContext.Clients.Include("Properties").Where(c=>c.Id.Equals((int)clientId)).FirstOrDefault();

      if (clientApp.Properties != null) { clientProperties.AddRange (clientApp.Properties); }

      ViewBag.systemName = clientApp.ClientName;
      ViewBag.clientId = clientApp.Id;
      return View (clientProperties);
    }

    public IActionResult NewModule (int? clientId) {
      //TODO: Implement Realistic Implementation
      var clientApp = _configDbContext.Clients.Where(c=>c.Id.Equals((int)clientId)).FirstOrDefault();
      ViewBag.ClientId = clientId;
      ViewBag.systemName=clientApp.ClientName;
      return View ();
    }
    [HttpPost]
    public IActionResult NewModule (IdentityServer4.EntityFramework.Entities.ClientProperty property, int? clientId) {
      if (ModelState.IsValid) {
        
        
        var clientApp = _configDbContext.Clients.Find (clientId);
        clientApp.Properties=new List<ClientProperty>();
        clientApp.Properties.Add(property);
        _configDbContext.SaveChanges();

      }

      //TODO: Implement Realistic Implementation
      return RedirectToAction("Modules",new{clientId=clientId});
    }

    public IActionResult RegisterSystem()
    {
      //TODO: Implement Realistic Implementation
      return View();
    }
  }
}