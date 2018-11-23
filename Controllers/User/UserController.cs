using Microsoft.AspNetCore.Mvc;

class UserController:Controller
{
    public IActionResult Index(){

        return View();
    }
}