using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.HomeVMs
{
    public class ReturnPasswordVM : BaseVM
    {
        [Display(Name = "用户名", Description = "用户名")]
        [Required]
        public string loginName { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var rv = new List<ValidationResult>();

            var user = DC.Set<FrameworkUser>().Where(x => x.ITCode == loginName && x.IsValid == true).SingleOrDefault();

            if (user == null)
                rv.Add(new ValidationResult("用户名不存在！", new string[] { "loginName" }));
            else if (user.Email == null || user.Email == "")
                rv.Add(new ValidationResult("此用户未配置邮箱！", new string[] { "loginName" }));

            rv.AddRange(base.Validate(validationContext));
            return rv;

        }

        public void SendMail(string ITCode)
        {
            var user = DC.Set<FrameworkUser>().Where(x => x.ITCode == loginName && x.IsValid == true).SingleOrDefault();

            SimpleLoginUserITCode UserITCode = new SimpleLoginUserITCode()
            {
                ITCode = user.ITCode,
                Password = user.Password,
                Email = user.Email
            };

            //密码解密
            //string password = Utils.DecryptString(UserITCode.Password);
            string password = UserITCode.Password;

            //发送邮件
            //AddEmail("找回密码", false, "您找回的密码是 : " + password, UserITCode.Email);
        }
    }

    public class SimpleLoginUserITCode : BasePoco
    {
        [Display(Name = "ITCode")]
        [Required(AllowEmptyStrings = false)]

        [StringLength(50)]
        public string ITCode { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
