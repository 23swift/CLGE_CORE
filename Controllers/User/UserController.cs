using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using UserController.Models;

namespace IdsServer
{
    
    public class UserController : Controller
    {
       private readonly UserManager<ApplicationUser> _userManager;
        public UserController( UserManager<ApplicationUser> userManager) {

             _userManager = userManager;
         }
        public async Task<IActionResult> Index()
        {
            //TODO: Implement Realistic Implementation
             //TODO: Implement Realistic Implementation
           var recordList= await _userManager.Users.ToListAsync();
          ViewBag.userList=recordList;
          return View();
        }
          public async Task<IActionResult> Create()
        {
            //TODO: Implement Realistic Implementation
            
          await Task.Yield();
          return View();
        }
        // GET api/user

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,UserName")] ApplicationUser user)
        {
            //TODO: Implement Realistic Implementation

          var result = await _userManager.CreateAsync(user);
            
          return RedirectToAction("Index");
          
        }
        public async Task<IActionResult> Details(int? Id)
        {
            //TODO: Implement Realistic Implementation
          var appUser= await _userManager.FindByIdAsync(Id.ToString());
          
          ViewBag.tabId="details";
         
          return View(appUser);
        }
         public async Task<IActionResult> Roles()
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
          ViewBag.tabId="roles";
         
          return View();
        }
          public async Task<IActionResult> Applications(int? Id)
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
          ViewBag.tabId="application";
         
          return View();
        }

        public async Task<IActionResult> AddUserApplication()
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
         
         
          return View();
        }

        
        public async Task<IActionResult> AddUserGroup()
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
         
         
          return View();
        }
          public async Task<IActionResult> AddUserRole()
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
         
         
          return View();
        }
      
    }
}