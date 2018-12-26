using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class UserController:Controller
{
    [HttpGet]
   public async Task<IActionResult> Index()
   {
       //TODO: Implement Realistic Implementation
     await Task.Yield();
     return View();
   }
}