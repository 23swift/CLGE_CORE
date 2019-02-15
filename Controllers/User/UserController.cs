using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdsServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using UserController.Models;

namespace IdsServer
{
    
    public class UserController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleMager;
       private readonly UserManager<ApplicationUser> _userManager;
       private readonly IConfigurationDbContext _configDbContext;
       private readonly IPersistedGrantDbContext _persistedDbContext;

      //  private readonly UserStore<ApplicationUser> _userStore;
        public UserController( UserManager<ApplicationUser> userManager,IConfigurationDbContext configDbContext,
        IPersistedGrantDbContext persistedDbContext,RoleManager<ApplicationRole> roleMager) {

             _userManager = userManager;
             _configDbContext=configDbContext;
             _persistedDbContext=persistedDbContext;
             _roleMager=roleMager;
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
                IActionResult ActionResult=RedirectToAction("Index");
                var clientList=new List<AppUserClient>();
                // clientList.Add(new AppUserClient{
                //     ClientId="map"

                // });
                // user.Clients=clientList;
                var result = await _userManager.CreateAsync(user);
                if(!result.Succeeded){

                    // throw new  Exception(result.Errors.FirstOrDefault().ToString());
                    ViewBag.message=result.Errors.FirstOrDefault().Description;
                    ActionResult=View(user);
                }




                    return ActionResult;
                
            

          
            
          
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("FirstName,LastName,UserName,Id")] ApplicationUser user)
        {
            //TODO: Implement Realistic Implementation
              var u= await _userManager.FindByIdAsync(user.UserName);
              u.FirstName=user.FirstName;
              u.LastName=user.LastName;
              u.UserName=user.UserName;


          var result = await _userManager.UpdateAsync(u);
            
          return RedirectToAction("Index");
          
        }
        public async Task<IActionResult> Details(int? Id)
        {
            //TODO: Implement Realistic Implementation
          var appUser=  await _userManager.Users.Where(u=>u.Id.Equals((int)Id)).FirstOrDefaultAsync();
          
          ViewBag.tabId="details";
         
          return View(appUser);
        }
         public async Task<IActionResult> Roles()
        {
          await Task.Yield();
          ViewBag.tabId="roles";
         
          return View();
        }
          public async Task<IActionResult> Applications(int? Id)
        {
            //TODO: Implement Realistic Implementation
            var appUser= await _userManager.Users.Include(c=>c.Clients).FirstAsync(u=>u.Id.Equals((int) Id));
           var clientList= appUser.Clients.ToList();
          // await Task.Yield();
          ViewBag.tabId="application";
         
          return View(clientList);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserApplication(int Id)
        {
            //TODO: Implement Realistic Implementation
            ViewBag.header="Assign Application to User";
            var applicationList=await _configDbContext.Clients.ToListAsync();
            IdsServer.Models.SystemSelectionViewModel selectionViewModel=new SystemSelectionViewModel();
            

            foreach (var item in applicationList)
            {
                selectionViewModel.Application.Add(new Models.SystemEditorViewModel{
                  Name=item.ClientName,ClientId=item.ClientId,
                  Id=item.Id
                });
            }
          
         
         
          return View(selectionViewModel);
        }
        [HttpPost]
    public async Task<IActionResult> AssignApplication(IdsServer.Models.SystemSelectionViewModel selection,int Id)
            {
                //TODO: Implement Realistic Implementation
                ViewBag.header="Assign Application to User";
              var appUser= await _userManager.Users.Include(c=>c.Clients).FirstAsync(u=>u.Id.Equals((int) Id));
              
                PersistedGrant pg=new PersistedGrant();

                foreach (var item in selection.Application)
                {
                  
                  //   pg.ClientId=item.Id.ToString();
                  //  _persistedDbContext.PersistedGrants.Add(pg);
                    if(item.Selected){
                      appUser.Clients.Add(new AppUserClient{
                        ClientId=item.Id
                        
                      });
                    }
                }
              await _userManager.UpdateAsync(appUser);
              //  await _persistedDbContext.SaveChangesAsync();
              //  _persistedDbContext.PersistedGrants.
              
            
            
              return RedirectToAction("Index");
            }

        
        public async Task<IActionResult> AddUserGroup()
        {
            //TODO: Implement Realistic Implementation
          await Task.Yield();
         
         
          return View();
        }
          public async Task<IActionResult> AddUserRole(int? clientId)
        {
             //TODO: Implement Realistic Implementation
             ViewBag.ClientId=(int)clientId;
              var roleListViewModel= new List<RoleEditorViewModel>();
             var roleList=_roleMager.Roles.Where(r=>r.Client.Equals((int)clientId)).ToList();
             foreach (var item in roleList)
             {
                 roleListViewModel.Add( new RoleEditorViewModel{
                    Name=item.Name,Id=item.Id

                 } );
             }

          await Task.Yield();
          
         
         
          return View(roleListViewModel);
        }

        [HttpPost]
           public async Task<IActionResult> AddUserRole(List<RoleEditorViewModel> roles,int? Id)
        {
          var user=_userManager.FindByIdAsync(((int)Id).ToString()).Result;
         
            foreach (var item in roles)
            {
              if(item.Selected){
                
               await _userManager.AddToRoleAsync(user,item.Name);       
              //  await  _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, item.Name));
              }
                
            }
          

          return RedirectToAction("Applications",new{Id=Id});
        }
      
    }
}