using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Resource;

namespace Stock.Model
{
    [Serializable]
    public class FrameworkCompany : BasePoco, IMLData<FrameworkCompanyMLContent>
    {
        [Display(Name = "CompanyCode", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(6, MinimumLength = 6)]
        public string CompanyCode { get; set; }


        [Display(Name = "ContainedDep", ResourceType = typeof(Language))]
        public List<FrameworkDepartment> Departments { get; set; }

        public List<CompanyPhoto> Photos { get; set; }

        #region IMLData成员

        public List<FrameworkCompanyMLContent> MLContents { get; set; }
        public IEnumerable<FrameworkCompanyMLContent> GetMLContents()
        {
            return MLContents == null ? null : MLContents.AsEnumerable();
        }

        #endregion
    }

    [Serializable]
    public class FrameworkCompanyMLContent : MLContent<FrameworkCompany>
    {
        [Display(Name = "CompanyName", ResourceType = typeof(Language))]
        [StringLength(100)]
        [MLRequired(AllLanguageRequired = false)]
        public string CompanyName { get; set; }

        [Display(Name = "CompanyRemark", ResourceType = typeof(Language))]
        public string CompanyRemark { get; set; }
    }
}
