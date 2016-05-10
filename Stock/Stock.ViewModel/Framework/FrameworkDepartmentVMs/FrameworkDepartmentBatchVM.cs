using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkDepartmentVMs
{
    public class FrameworkDepartmentBatchVM : BaseBatchVM<FrameworkDepartment, FrameworkDepartment_BatchEdit>
    {
        public List<ComboSelectListItem> AllParents { get; set; }
        public FrameworkDepartmentBatchVM()
        {
            ListVM = new FrameworkDepartmentListVM();
            LinkedVM = new FrameworkDepartment_BatchEdit();
        }

        protected override void InitVM()
        {
            //AllParents = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(x => !IDs.Contains(x.ID), x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
        }
    }

    /// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class FrameworkDepartment_BatchEdit : BaseVM
    {
        [Display(Name = "ParentDep", ResourceType = typeof(Language))]
        public long? ParentID { get; set; }

    }

}
