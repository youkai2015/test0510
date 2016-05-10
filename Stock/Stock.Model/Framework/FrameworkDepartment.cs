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
    public class FrameworkDepartment : BasePoco, IMLData<FrameworkDepartmentMLContent>, ITreeData<FrameworkDepartment>
    {
        [Display(Name = "DepCode", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(6, MinimumLength = 6)]
        public string DepartmentCode { get; set; }
        public UserTypeEnum UserType { get; set; }

        [Required]
        [Display(Name = "Company", ResourceType = typeof(Language))]
        public Guid? CompanyID { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Language))]
        public FrameworkCompany Company { get; set; }

        [Display(Name = "ContainedUser", ResourceType = typeof(Language))]
        public List<FrameworkUser> Users { get; set; }

        #region IMLData成员

        public List<FrameworkDepartmentMLContent> MLContents { get; set; }
        public IEnumerable<FrameworkDepartmentMLContent> GetMLContents()
        {
            return MLContents == null ? null : MLContents.AsEnumerable();
        }


        #endregion

        #region ITreeData成员

        [Display(Name = "ChildDep", ResourceType = typeof(Language))]
        public List<FrameworkDepartment> Children { get; set; }
        [Display(Name = "ParentDep", ResourceType = typeof(Language))]
        public FrameworkDepartment Parent { get; set; }
        [Display(Name = "ParentDep", ResourceType = typeof(Language))]
        public Guid? ParentID { get; set; }
        public IEnumerable<FrameworkDepartment> GetChildren()
        {
            return Children == null ? null : Children.AsEnumerable();
        }

        #endregion
    }

    [Serializable]
    public class FrameworkDepartmentMLContent : MLContent<FrameworkDepartment>
    {
        [Display(Name = "DepName", ResourceType = typeof(Language))]
        [StringLength(50)]
        [MLRequired(AllLanguageRequired = false)]
        public string DepartmentName { get; set; }
    }
}
