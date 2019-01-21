using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdsServer
{
  public class RoleController : Controller
  {

    // ILogger<RoleController> _logger;

    // public RoleController(ILogger<RoleController> logger)
    // {
    //   _logger = logger;
    // }
    
  private readonly RoleManager<ApplicationRole> _roleMager;
  private readonly IConfigurationDbContext _configDbContext;
  public RoleController(RoleManager<ApplicationRole> roleMager,
  IConfigurationDbContext configDbContext){

    _roleMager=roleMager;
    _configDbContext=configDbContext;
  }
    public IActionResult Index(int? clientId)
    {
      ViewBag.ClientId=clientId;
      
      var roleList=_roleMager.Roles.Where(r=>r.Client.Equals((int)clientId)).ToList();
      return View(roleList);
    }
    public IActionResult Create(int? clientId)
    {
      ViewBag.ClientId=clientId;
      return View();
    }

  [HttpPost]
    public async Task<IActionResult> Create([Bind("Name")] ApplicationRole role,int ClientId)
    {
      var c= _configDbContext.Clients.Find(ClientId);
      role.Client=ClientId;
      // TODO Remove
      await _roleMager.CreateAsync(role);
      // await _roleMager.UpdateAsync(role);
      
    
      return RedirectToAction("Index",new{clientId=ClientId});
    }
  }
}