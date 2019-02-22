using System;
using System.ComponentModel.DataAnnotations;

namespace CLGE_CORE.Models
{
    public class SystemViewModel
    {
        [Required]
        public string SystemName { get; set; }
        public Guid SystemId { get; set; }
        public string Description { get; set; }
        [Required]
        public string ClientUri { get; set; }
        public bool Configure { get; set; }
        
    }
}