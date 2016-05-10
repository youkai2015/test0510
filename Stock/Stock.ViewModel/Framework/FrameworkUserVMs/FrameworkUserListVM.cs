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
    public class FrameworkUserListVM : BasePagedListVM<FrameworkUser_ListView, FrameworkUserSearcher>
    {
        public ColumnFormatCallBack<FrameworkUser_ListView> GetPhotoFunc { get; set; }

        /// <summary>
        /// 页面显示按钮方案，如果需要增加Action类型则将按钮添加到此类中
        /// </summary>
        public FrameworkUserListVM()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.Create, Language.CreateUser));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.Edit, Language.EditUser));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.Delete, Language.DeleteUser));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.Details, Language.UserDetail));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.BatchEdit, Language.BatchEditUser));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.BatchDelete, Language.BatchDeleteUser));
            GridActions.Add(this.MakeStandardAction("FrameworkUser", GridActionStandardTypesEnum.Import, Language.BatchImport));
            GridActions.Add(this.MakeStandardExportAction(exportType: ExportEnum.Excel));
            GridActions.Add(this.MakeStandardChartAction());
        }

        /// <summary>
        /// 页面显示列表
        /// </summary>
        protected override void InitListVM()
        {
            List<IGridColumn<FrameworkUser_ListView>> rv = new List<IGridColumn<FrameworkUser_ListView>>();
            if (this.SearcherMode != ListVMSearchModeEnum.CheckExport && this.SearcherMode != ListVMSearchModeEnum.Export && Configs.EnableSystemLoop && Configs.EnableOnlineCheck)
            {
                rv.Add(this.MakeGridColumn(x => x.OnLine).SetWidth(40).SetHeader("").SetFormat((item, value) => GetOnline(value)));
            }
            rv.Add(this.MakeGridColumn(x => x.ITCode));
            rv.Add(this.MakeGridColumn(x => x.Name));
            rv.Add(this.MakeGridColumn(x => x.CellPhone));
            rv.Add(this.MakeGridColumn(x => x.Email));
            rv.Add(this.MakeGridColumn(x => x.DepartmentName));
            rv.Add(this.MakeGridColumn(x => x.CompanyName));
            rv.Add(this.MakeGridColumn(x => x.Count, GetRolesName));
            rv.Add(this.MakeGridColumn(x => x.IsValid));
            rv.Add(this.MakeGridColumn(x => x.PhotoID, GetPhoto));
            rv.Add(this.MakeGridColumn(x => x.StartWorkDate).SetFormat((item, obj) => { return item.StartWorkDate == null ? "" : item.StartWorkDate.Value.ToString("yyyy-MM-dd"); }));
            rv.Add(this.MakeActionGridColumn(Width: 150));
            ListColumns = rv;
        }

        public string GetRolesName(FrameworkUser_ListView item, object val)
        {
            string rv = item.RoleNames.ToSpratedString(x => x);
            return rv;
        }


        public string GetOnline(object value)
        {
            bool online = (value as bool?).HasValue ? (value as bool?).Value : false;
            if (online == true)
            {
                return "<img src='/Content/Images/check.png'/>";
            }
            else
            {
                return "<p style='height:100%'></p>";
            }
        }

        public List<ColumnFormatInfo> GetPhoto(FrameworkUser_ListView item, object val)
        {
            if (item.PhotoID != null)
            {
                List<ColumnFormatInfo> rv = new List<ColumnFormatInfo>();
                rv.Add(ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Link, item.PhotoID.Value, 600, 600));
                rv.Add(ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Link, item.PhotoID.Value));
                return rv;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查询结果
        /// </summary>
        public override IOrderedQueryable<FrameworkUser_ListView> GetSearchQuery()
        {
            if (Searcher.CompanyID == null)
            {
                Searcher.CompanyID = new List<Guid>();
            }
            if (Searcher.DepartmentID == null)
            {
                Searcher.DepartmentID = new List<Guid>();
            }
            var query = DC.Set<FrameworkUser>()
                .Where(x =>
                    (string.IsNullOrEmpty(Searcher.ITCode) || x.ITCode.Contains(Searcher.ITCode)) &&
                    (string.IsNullOrEmpty(Searcher.Name) || x.Name.Contains(Searcher.Name)) &&
                    (string.IsNullOrEmpty(Searcher.Email) || x.Email.Contains(Searcher.Email)) &&
                    (Searcher.CompanyID.Count() == 0 || Searcher.CompanyID.Contains(x.Department.CompanyID.Value)) &&
                    (Searcher.DepartmentID.Count() == 0 || Searcher.DepartmentID.Contains(x.DepartmentID.Value)) &&
                    (Searcher.IsValid == null || x.IsValid == Searcher.IsValid)
                )
                .GroupJoin(DC.Set<FrameworkOnline>(), ok => ok.ITCode, ik => ik.ITCode, (user, online) => new { online = online.Select(a => a.IsOnLine).FirstOrDefault(), user = user })
                //.DPWhere(LoginUserInfo.DataPrivileges, x => x.user.Department.CompanyID, x => x.user.DepartmentID)
                .Select(x => new FrameworkUser_ListView
                {
                    ID = x.user.ID,
                    ITCode = x.user.ITCode,
                    Name = x.user.Name,
                    CellPhone = x.user.CellPhone,
                    Email = x.user.Email,
                    DepartmentName = x.user.Department.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.DepartmentName).FirstOrDefault(),
                    CompanyName = x.user.Department.Company.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.CompanyName).FirstOrDefault(),
                    IsValid = x.user.IsValid,
                    PhotoID = x.user.PhotoID,
                    Count = x.user.Roles.Count(),
                    StartWorkDate = x.user.StartWorkDate,
                    RoleNames = x.user.Roles.SelectMany(z => z.MLContents).Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(a => a.RoleName),
                    OnLine = x.online
                })
                .OrderByDescending(x => x.ITCode);
            return query;
        }

    }

    /// <summary>
    /// 如果需要显示树类型的列表需要继承ITreeData<>接口，并实现Children,Parent,ParentID属性
    /// </summary>
    public class FrameworkUser_ListView : BasePoco
    {
        [Display(Name = "ITCode")]
        public string ITCode { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Language))]
        public string Name { get; set; }
        [Display(Name = "CellPhone", ResourceType = typeof(Language))]
        public string CellPhone { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Language))]
        public string Email { get; set; }
        [Display(Name = "DepName", ResourceType = typeof(Language))]
        public string DepartmentName { get; set; }
        [Display(Name = "CompanyName", ResourceType = typeof(Language))]
        public string CompanyName { get; set; }
        [Display(Name = "Role", ResourceType = typeof(Language))]
        public int Count { get; set; }
        [Display(Name = "IsValid", ResourceType = typeof(Language))]
        public bool IsValid { get; set; }
        [Display(Name = "Photo", ResourceType = typeof(Language))]
        public Guid? PhotoID { get; set; }
        [Display(Name = "EnrollDate", ResourceType = typeof(Language))]
        public DateTime? StartWorkDate { get; set; }
        public IEnumerable<string> RoleNames { get; set; }

        [Display(Name = "OnLine", ResourceType = typeof(Language))]
        public bool? OnLine { get; set; }
    }
}