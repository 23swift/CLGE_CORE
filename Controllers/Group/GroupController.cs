using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdsServer
{
  public class GroupController : Controller
  {

    // ILogger<GroupController> _logger;

    // public GroupController(ILogger<GroupController> logger)
    // {
    //   _logger = logger;
    // }

    public IActionResult Index()
    {
      
      return View();
    }
  }
}