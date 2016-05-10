using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkDepartmentVMs
{
    public class FrameworkDepartmentSearcher : BaseSearcher
    {
        [Display(Name = "DepCode", ResourceType = typeof(Language))]
        public string DepartmentCode { get; set; }
        [Display(Name = "DepName", ResourceType = typeof(Language))]
        public string DepartmentName { get; set; }

        [Display(Name = "Company", ResourceType = typeof(Language))]
        public Guid? CompanyID { get; set; }
        public List<ComboSelectListItem> AllCompanys { get; set; }


        protected override void InitVM()
        {
            AllCompanys = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
        }
    }
}