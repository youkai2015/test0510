using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkUserVMs
{
    public class FrameworkUserVM : BaseCRUDVM<FrameworkUser>
    {
        [Display(Name = "Company", ResourceType = typeof(Language))]
        public Guid? CompanyID { get; set; }
        public List<ComboSelectListItem> AllCompanys { get; set; }
        public List<ComboSelectListItem> AllDepartments { get; set; }
        [Display(Name = "AllRole", ResourceType = typeof(Language))]
        public List<ComboSelectListItem> AllRoles { get; set; }
        [Display(Name = "Role", ResourceType = typeof(Language))]
        public List<Guid> SelectedRolesIDs { get; set; }
        public FrameworkUserVM()
        {
            SetInclude(x => x.Department.Company.MLContents, x => x.Department.MLContents, x => x.Roles);
        }
        public override DuplicatedInfo<FrameworkUser> SetDuplicatedCheck()
        {
            var rv = this.CreateFieldsInfo(SimpleField(x => x.ITCode));
            return rv;
        }

        protected override void InitVM()
        {
            CompanyID = Entity.Department == null ? null : Entity.Department.CompanyID;
            AllCompanys = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
            AllDepartments = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(x => x.CompanyID == CompanyID, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
            SelectedRolesIDs = Entity.Roles.Select(x => x.ID).ToList();
            AllRoles = DC.Set<FrameworkRole>().GetSelectListItemsForML(null, x => x, y => y.RoleName, LoginUserInfo.DataPrivileges);
        }


        protected override void ReInitVM()
        {
            AllCompanys = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
            AllDepartments = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(x => x.CompanyID == CompanyID, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
            AllRoles = DC.Set<FrameworkRole>().GetSelectListItemsForML(null, x => x, y => y.RoleName, LoginUserInfo.DataPrivileges);
        }

        public override void DoAdd()
        {
            DC.ChangeRelationTo(Entity, x => x.Roles, null, SelectedRolesIDs);
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            var OriginalRolesIDs = DC.Set<FrameworkUser>().Where(x => x.ID == Entity.ID).SelectMany(x => x.Roles).Select(x => x.ID).ToList();
            DC.ChangeRelationTo(Entity, x => x.Roles, OriginalRolesIDs, SelectedRolesIDs);
            base.DoEdit(updateAllFields);
        }

        //public List<ComboSelectListItem> GetRoles(string text)
        //{
        //    var d = DC.Set<FrameworkRole>().Where(x => x.MLContents.Where(y => y.RoleName.Contains(text)).Count() > 0).GetSelectListItemsForML(null, x => x, y => y.RoleName, LoginUserInfo.DataPrivileges);
        //    return d;
        //}
    }
}