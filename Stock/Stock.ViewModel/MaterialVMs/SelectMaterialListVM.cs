using Stock.Model.Material;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class SelectMaterialListVM : BasePagedListVM<Material_ListView, MaterialSelectSearcher>
    {
        public SelectMaterialListVM()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(this.MakeScriptGridAction("Material", "SelectMaterial", "选择", "Select('$ID$');return false;", showInRow: true, hideOnToolBar: true));
        }
        protected override void InitListVM()
        {
            List<IGridColumn<Material_ListView>> rv = new List<IGridColumn<Material_ListView>>();
            rv.Add(this.MakeActionGridColumn());
            rv.Add(this.MakeGridColumn(x => x.MaterialCode).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.GoodsName).SetWidth(400));
            rv.Add(this.MakeGridColumn(x => x.Specification).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.MerchantCode).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.MerchantName).SetWidth(150));
            ListColumns = rv;
        }
        public override IOrderedQueryable<Material_ListView> GetSearchQuery()
        {
            var query = DC.Set<Material>().Where(x =>
            (string.IsNullOrEmpty(Searcher.GName) || x.GoodsName.Contains(Searcher.GName)) &&
            (string.IsNullOrEmpty(Searcher.Material) || x.MaterialCode.Contains(Searcher.Material)) &&
            (string.IsNullOrEmpty(Searcher.Spec) || x.Specification.Contains(Searcher.Spec)) &&
            this.DC.CSName==x.MerchantCode&&
            x.ID != Searcher.ParentID
            ).Select(x => new Material_ListView
            {
                ID = x.ID,
                GoodsName = x.GoodsName,
                MaterialCode = x.MaterialCode,
                Specification = x.Specification,
                MerchantCode = x.MerchantCode,
                MerchantName = x.MerchantName
            }).OrderBy(x => x.GoodsName);
            return query;
        }
    }
    public class MaterialSelectSearcher : BaseSearcher
    {
        [Display(Name = "商品名称")]
        public string GName { get; set; }

        [Display(Name = "规格")]
        public string Spec { get; set; }

        [Display(Name = "物料编号")]
        public string Material { get; set; }
        [Display(Name = "商户编码")]
        public string MerchantCode { get; set; }
    }
}
