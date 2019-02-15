using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdsServer.Models
{

        public class SystemSelectionViewModel
        {
            public List<SystemEditorViewModel> Application { get; set; }
            public SystemSelectionViewModel()
            {
                this.Application = new List<SystemEditorViewModel>();
            }
            public IEnumerable<int> getSelectedIds()
            {
                // Return an Enumerable containing the Id's of the selected people:
                return (from p in this.Application where p.Selected select p.Id).ToList();
            }
        }
        public class SystemEditorViewModel
        {
            public bool Selected { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string ClientId { get; set; }
        }

        public class RoleSelectionViewModel
        {
            public List<RoleEditorViewModel> Role { get; set; }
            public RoleSelectionViewModel()
            {
                this.Role = new List<RoleEditorViewModel>();
            }
          
        }
        public class RoleEditorViewModel
        {
            public bool Selected { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string ClientId { get; set; }
        }

        public class ModuleSelectListViewModel
        {
                // public ModuleSelectListViewModel(){
                //     this.SelectedModules=new List<SelectListItem>();
                // }
            
            public int roleId { get; set; }
           public List<SelectListItem> SelectedModules{get;set;}
            
        }
}
