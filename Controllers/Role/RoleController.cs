using System;
using System.Linq;
using System.Threading.Tasks;
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
    
  private readonly RoleManager<ApplicationUserRole> _roleMager;
  public RoleController(RoleManager<ApplicationUserRole> roleMager){
    _roleMager=roleMager;
  }
    public IActionResult Index(int? Id)
    {
      ViewBag.ClientId=Id;
      return View();
    }
    public IActionResult Create(int? Id)
    {
      ViewBag.ClientId=Id;
      return View();
    }

  [HttpPost]
    public async Task<IActionResult> Create(ApplicationUserRole role)
    {
      
    
      // TODO Remove
      await _roleMager.CreateAsync(role);
      
    
      return RedirectToAction("Index");
    }
  }
}