using BriefResume.ValidationAttributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class SeekerUpdateDto : IValidatableObject
    {
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }//此处需要用正则表达式添加数据验证,对应Identity的默认username要求
        [MaxLength(500)]
        public string Preference { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ValidationCondition.IsContainSpace(UserName))
            {
                yield return new ValidationResult("UserName不能含有空格", new[] { nameof(UserName) });//第二个参数提示哪个位置出错
            }
            if (ValidationCondition.IsEmail(Email))
            {
                yield return new ValidationResult("Email错误", new[] { nameof(Email) });
            }
            if (ValidationCondition.IsPhoneNumber(PhoneNumber))
            {
                yield return new ValidationResult("PhoneNumber错误", new[] { nameof(PhoneNumber) });
            }

        }





    }
}
