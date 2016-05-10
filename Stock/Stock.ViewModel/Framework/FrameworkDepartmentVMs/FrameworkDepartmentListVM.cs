using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkDepartmentVMs
{
    public class FrameworkDepartmentListVM : BasePagedListVM<FrameworkDepartment_ListView, FrameworkDepartmentSearcher>
    {
        /// <summary>
        /// 页面显示按钮方案，如果需要增加Action类型则将按钮添加到此类中
        /// </summary>
        public FrameworkDepartmentListVM()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.Create, Language.CreateDep).SetParameterType(GridActionParameterTypesEnum.SingleIDWithNull));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.Edit, Language.EditDep));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.Delete, Language.DeleteDepartment));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.Details, Language.DepDetail));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.BatchEdit, Language.BatchEditDep));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.BatchDelete, Language.BatchDeleteDep));
            GridActions.Add(this.MakeStandardAction("FrameworkDepartment", GridActionStandardTypesEnum.Import, Language.BatchImport));
            GridActions.Add(this.MakeStandardExportAction());
            GridActions.Add(this.MakeStandardChartAction());
        }

        /// <summary>
        /// 页面显示列表
        /// </summary>
        protected override void InitListVM()
        {
            List<IGridColumn<FrameworkDepartment_ListView>> rv = new List<IGridColumn<FrameworkDepartment_ListView>>();
            rv.Add(this.MakeGridColumn(x => x.DepartmentCode).SetWidth(200));
            rv.Add(this.MakeGridColumn(x => x.CompanyName).SetSortable(false));
            rv.Add(this.MakeGridColumn(x => x.DepartmentName));
            rv.Add(this.MakeGridColumn(x => x.Count));
            rv.Add(this.MakeActionGridColumn(Width: 150));
            ListColumns = rv;
        }
        /// <summary>
        /// 查询结果
        /// </summary>
        public override IOrderedQueryable<FrameworkDepartment_ListView> GetSearchQuery()
        {
            var query = DC.Set<FrameworkDepartment>()
                .Where(x =>
                    ((Searcher.ParentID == null && x.ParentID == null) || (Searcher.ParentID != null && x.ParentID == Searcher.ParentID)) &&
                    ((string.IsNullOrEmpty(Searcher.DepartmentCode)) || (x.DepartmentCode.Contains(Searcher.DepartmentCode))) &&
                    ((Searcher.CompanyID == null) || (x.CompanyID == Searcher.CompanyID)) &&
                    ((string.IsNullOrEmpty(Searcher.DepartmentName)) || (x.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.DepartmentName).FirstOrDefault().Contains(Searcher.DepartmentName)))
                )
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.CompanyID)
                .Select(x => new FrameworkDepartment_ListView
                {
                    ID = x.ID,
                    DepartmentCode = x.DepartmentCode,
                    CompanyID = x.CompanyID,
                    DepartmentName = x.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.DepartmentName).FirstOrDefault(),
                    Count = x.Users.Count(),
                    HasChild = x.Children.Count() > 0 ? true : false,
                    ParentID = x.ParentID
                })
                .OrderBy(x => x.DepartmentCode);
            return query;
        }

        public override void AfterDoSearcher()
        {
            var companys = DC.Set<FrameworkCompany>().Include(x => x.MLContents).ToList();
            foreach (var item in EntityList)
            {
                item.CompanyName = companys.Where(x => x.ID == item.CompanyID).FirstOrDefault().MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.CompanyName).FirstOrDefault();
            }
        }
    }

    /// <summary>
    /// 如果需要显示树类型的列表需要继承ITreeData<>接口，并实现Children,Parent,ParentID属性
    /// </summary>
    public class FrameworkDepartment_ListView : BasePoco, ITreeView<FrameworkDepartment_ListView>
    {
        [Display(Name = "DepCode", ResourceType = typeof(Language))]
        public string DepartmentCode { get; set; }
        [Display(Name = "CompanyName", ResourceType = typeof(Language))]
        public string CompanyName { get; set; }

        public Guid? CompanyID { get; set; }
        [Display(Name = "DepName", ResourceType = typeof(Language))]
        public string DepartmentName { get; set; }
        [Display(Name = "ContainedUser", ResourceType = typeof(Language))]
        public int Count { get; set; }
        public bool HasChild { get; set; }
        public List<FrameworkDepartment_ListView> Children { get; set; }
        public FrameworkDepartment_ListView Parent { get; set; }
        public Guid? ParentID { get; set; }
        public IEnumerable<FrameworkDepartment_ListView> GetChildren()
        {
            return Children == null ? null : Children.AsEnumerable();
        }

    }

}