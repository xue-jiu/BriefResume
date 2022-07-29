using BriefResume.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class SeekerRegisterDto : IValidatableObject
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ValidationCondition.IsEmail(Email))
            {
                yield return new ValidationResult("Email错误", new[] { nameof(Email) });//第二个参数提示哪个位置出错
            }
        }


    }

}
