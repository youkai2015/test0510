using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkUserVMs
{
    public class FrameworkUserSearcher : BaseSearcher
    {
        //[Display(Name = "开始时间")]
        //public DateTime? StartCreateTime { get; set; }
        //[Display(Name = "结束时间")]
        //public DateTime? EndCreateTime { get; set; }
        //[Display(Name = "搜索类型")]
        //public int Type { get; set; }

        [Display(Name = "ITCode")]
        public string ITCode { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Language))]
        public string Name { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Language))]
        public string Email { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Language))]
        public List<Guid> CompanyID { get; set; }
        public List<ComboSelectListItem> AllCompanys { get; set; }
        [Display(Name = "Department", ResourceType = typeof(Language))]
        public List<Guid> DepartmentID { get; set; }
        public List<ComboSelectListItem> AllDepartments { get; set; }

        public FrameworkUserSearcher()
        {
            IsValid = true;
        }

        protected override void InitVM()
        {
            // AllCompanys = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
            //AllDepartments = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(null, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
        }

    }
}