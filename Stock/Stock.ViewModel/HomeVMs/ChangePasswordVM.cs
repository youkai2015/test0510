using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.HomeVMs
{
    /// <summary>
    /// 更改密码VM
    /// </summary>
    public class ChangePasswordVM : BaseVM
    {
        [Display(Name = "ITCode")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string ITCode { get; set; }

        [Display(Name = "CurrentPassword", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string OldPassword { get; set; }

        [Display(Name = "CreatePassword", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string NewPassword { get; set; }

        [Display(Name = "NewPasswrodConfirm", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string NewPasswordComfirm { get; set; }

        /// <summary>
        /// 自定义验证函数，验证原密码是否正确，并验证两次新密码是否输入一致
        /// </summary>
        /// <param name="validationContext">验证环境</param>
        /// <returns>验证结果</returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> rv = new List<ValidationResult>();
            //检查原密码是否正确，如不正确则输出错误
            if (DC.Set<FrameworkUser>().Where(x => x.ITCode.ToLower() == ITCode.ToLower()).Select(x => x.Password).SingleOrDefault() != OldPassword)
            {
                rv.Add(new ValidationResult(Language.CurrentPasswordWrong, new[] { "OldPassword" }));
            }
            //检查两次新密码是否输入一致，如不一致则输出错误
            if (NewPassword != NewPasswordComfirm)
            {
                rv.Add(new ValidationResult(Language.ConfirmedPasswordWrong, new[] { "NewPasswordComfirm" }));
            }
            return rv;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public void DoChange()
        {
            var user = DC.Set<FrameworkUser>().Where(x => x.ITCode.ToLower() == ITCode.ToLower()).SingleOrDefault();
            if (user != null)
            {
                user.Password = NewPassword;
            }
            DC.SaveChanges();
        }
    }
}