using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.EntityFramework.Interfaces;

namespace IdsServer
{
  public class SystemController : Controller
  {

    // ILogger<SystemController> _logger;

    // public SystemController(ILogger<SystemController> logger)
    // {
    //   _logger = logger;
    // }
private readonly IConfigurationDbContext _configDbContext;
public SystemController(IConfigurationDbContext configDbContext){

    _configDbContext=configDbContext;
}
    public IActionResult Index()
    {
      var clients=_configDbContext.Clients.ToList();
      ViewBag.clients=clients;
      return View();
    }
  }
}
