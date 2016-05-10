using System.Collections.Generic;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.Framework.FrameworkDepartmentVMs
{
    public class FrameworkDepartmentVM : BaseCRUDVM<FrameworkDepartment>
    {
        public List<TreeSelectListItem> AllTreeCombo { get; set; }
        public List<ComboSelectListItem> AllCompanys { get; set; }

        public FrameworkDepartmentVM()
        {
            SetInclude(x => x.Company.MLContents, x => x.Parent);
        }
        public override DuplicatedInfo<FrameworkDepartment> SetDuplicatedCheck()
        {
            var rv = this.CreateFieldsInfo(SimpleField(x => x.DepartmentCode));
            rv.AddGroup(SubField(x => x.MLContents, y => y.DepartmentName, y => y.LanguageCode), SimpleField(x => x.CompanyID), SimpleField(x => x.ParentID));
            return rv;
        }
        public override void DoEdit(bool updateAllFields = false)
        {
            DC.UpdateProperty(Entity, x => x.ParentID);
            base.DoEdit(updateAllFields);
        }
        protected override void InitVM()
        {
            var ids = Entity.GetAllChildrenIDs(DC);
            AllCompanys = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
            AllTreeCombo = DC.Set<FrameworkDepartment>().GetTreeSelectListItemsForML(null, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
        }
    }
}