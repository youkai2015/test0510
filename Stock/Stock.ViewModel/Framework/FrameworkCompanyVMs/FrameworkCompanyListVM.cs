using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.Framework.FrameworkCompanyVMs
{
    public class FrameworkCompanyListVM : BasePagedListVM<FrameworkCompany_ListView, FrameworkCompanySearcher>
    {
        public string test { get; set; }
        /// <summary>
        /// 页面显示按钮方案，如果需要增加Action类型则将按钮添加到此类中
        /// </summary>
        public FrameworkCompanyListVM()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.Create, Language.CreateCompany));
            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.Edit, Language.EditCompany));
            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.Delete, Language.DeleteCompany));
            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.Details, Language.CompanyDetail));
            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.BatchDelete, Language.BatchDeleteCompany));

            GridActions.Add(this.MakeStandardAction("FrameworkCompany", GridActionStandardTypesEnum.Import, Language.BatchImport));
            GridActions.Add(this.MakeStandardExportAction("aaabbb"));
        }

        /// <summary>
        /// 页面显示列表
        /// </summary>
        protected override void InitListVM()
        {
            List<IGridColumn<FrameworkCompany_ListView>> rv = new List<IGridColumn<FrameworkCompany_ListView>>();
            rv.Add(this.MakeGridColumn(x => x.CompanyCode));
            rv.Add(this.MakeGridColumn(x => x.CompanyName).SetWidth(250));
            rv.Add(this.MakeGridColumn(x => x.Count));
            rv.Add(this.MakeActionGridColumn(Width: 150));
            ListColumns = rv;
        }
        /// <summary>
        /// 查询结果
        /// </summary>
        public override IOrderedQueryable<FrameworkCompany_ListView> GetSearchQuery()
        {

            var query = DC.Set<FrameworkCompany>()
                .Where(x =>
                    ((string.IsNullOrEmpty(Searcher.CompanyCode)) || (x.CompanyCode.Contains(Searcher.CompanyCode))) &&
                    ((string.IsNullOrEmpty(Searcher.CompanyName)) || (x.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.CompanyName).FirstOrDefault().Contains(Searcher.CompanyName)))
                )
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.ID)
                .Select(x => new FrameworkCompany_ListView
                {
                    ID = x.ID,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.MLContents.Where(y => y.LanguageCode == CurrentLanguage.LanguageCode).Select(y => y.CompanyName).FirstOrDefault(),
                    Count = x.Departments.Count()
                })
                .OrderBy(x => x.CompanyCode);
            return query;
        }
    }

    /// <summary>
    /// 如果需要显示树类型的列表需要继承ITreeData<>接口，并实现Children,Parent,ParentID属性
    /// </summary>
    public class FrameworkCompany_ListView : BasePoco
    {
        [Display(Name = "CompanyCode", ResourceType = typeof(Language))]
        public string CompanyCode { get; set; }
        [Display(Name = "CompanyName", ResourceType = typeof(Language))]
        public string CompanyName { get; set; }
        [Display(Name = "ContainedDep", ResourceType = typeof(Language))]
        public int Count { get; set; }
    }
}