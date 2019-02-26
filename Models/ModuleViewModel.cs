

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CLGE_CORE.Models
{
    public class ModuleViewModel: IValidatableObject

    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Description")] 
        public string Key { get; set; }
         [Required]
         [DisplayName("Code")] 
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}