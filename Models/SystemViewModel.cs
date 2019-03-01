using System;
using System.ComponentModel.DataAnnotations;

namespace CLGE_CORE.Models
{
    public class SystemViewModel
    {
        public int Id { get; set; }
        [Required]
        public string SystemName { get; set; }
        public Guid SystemId { get; set; }
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^(http|https)://[a-zA-Z0-9]+$", 
         ErrorMessage = "*Please follow the correct format(http://<HostName> or https://<HostaName>).")]
        public string ClientUri { get; set; }
        public bool Configure { get; set; }
        
    }
}