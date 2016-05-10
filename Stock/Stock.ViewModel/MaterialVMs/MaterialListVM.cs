using Stock.Model.Material;
using Stock.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using System;
namespace Stock.ViewModel.MaterialVMs
{
    public class MaterialListVM : BasePagedListVM<Material_ListView, MaterialSearcher>
    {
        public MaterialListVM()
        {
            GridActions = new List<GridAction>();
            GridActions.Add(this.MakeStandardAction("Material", GridActionStandardTypesEnum.Create, "新建", whereStr: x => x.MerchantCode));
            GridActions.Add(this.MakeStandardAction("Material", GridActionStandardTypesEnum.Edit, "修改", whereStr: x => x.MerchantCode));
            GridActions.Add(this.MakeStandardAction("Material", GridActionStandardTypesEnum.Delete, "删除", whereStr: x => x.MerchantCode));
            //GridActions.Add(this.MakeScriptGridAction("Material", "SelectMaterial", "编辑子物料", "SelectMaterial('$ID$');return false;", showInRow: true, hideOnToolBar: true));
            GridActions.Add(this.MakeGridAction("编辑子物料", "编辑子物料", "Material", "SelectMaterial", GridActionParameterTypesEnum.NoID, true,null,true,null,null,null,true,null,false,true,x=>x.ID,x=>x.MerchantCode));
            GridActions.Add(this.MakeStandardAction("Material", GridActionStandardTypesEnum.Import, Language.BatchImport));
            GridActions.Add(this.MakeStandardExportAction());//"test"
        }
        protected override void InitListVM()
        {
            List<IGridColumn<Material_ListView>> rv = new List<IGridColumn<Material_ListView>>();
            //  rv.Add(this.MakeGridColumn(x => x.MerchantID, (item, val) => ReturnHtmlLink(item)).SetWidth(150).SetHeader("操作"));
            rv.Add(this.MakeActionGridColumn(Width: 150));
            rv.Add(this.MakeGridColumn(x => x.GoodsName).SetWidth(400));
            rv.Add(this.MakeGridColumn(x => x.MaterialCode).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.Specification).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.StockNumber));
            rv.Add(this.MakeGridColumn(x => x.MerchantName));
            rv.Add(this.MakeGridColumn(x => x.MerchantCode));
            rv.Add(this.MakeGridColumn(x => x.OccupyNumber));
            ListColumns = rv;
        }


        public MaterialList GetMaterialList()
        {
            var rv = new MaterialList();
            rv.EntityList = EntityList;
            rv.TotalPages = Searcher.TotalPages;
            rv.TotalRecords = Searcher.TotalRecords;
            return rv;

        }
        #region 之前版本
        //private List<ColumnFormatInfo> ReturnHtmlLink(Material_ListView item)
        //{
        //    List<ColumnFormatInfo> rv = new List<ColumnFormatInfo>();
        //    rv.Add(ColumnFormatInfo.MakeDialogButton(ButtonTypesEnum.Link, "/Material/Edit?id=" + item.ID + "&MerchantCode=" + item.MerchantCode + "", null, null, "Edit", "修改", "修改", null, true));
        //    rv.Add(ColumnFormatInfo.MakeDialogButton(ButtonTypesEnum.Link, "/Material/Delete?id=" + item.ID + "&MerchantCode=" + item.MerchantCode + "", null, null, "Delete", "删除", "删除", null, true));
        //    //  rv.Add(ColumnFormatInfo.MakeDialogButton(ButtonTypesEnum.Link, "/Material/Details?id=" + item.ID + "&MerchantCode=" + item.MerchantCode + "", null, null, "Details", "详细", "详细", null, true));
        //    return rv;
        //}
        #endregion
        public override IOrderedQueryable<Material_ListView> GetSearchQuery()
        {
            var query = DC.Set<Material>().Where(x =>
                (string.IsNullOrEmpty(Searcher.MaterialCode) || Searcher.MaterialCode == x.MaterialCode) &&
                (string.IsNullOrEmpty(Searcher.GoodsName) || x.GoodsName.Contains(Searcher.GoodsName)) &&
                 Searcher.MerchantCode == x.MerchantCode
            ).Select(x => new Material_ListView
            {
                ID = x.ID,
                GoodsName = x.GoodsName,
                MaterialCode = x.MaterialCode,
                Specification = x.Specification,
                MerchantID = x.MerchantID,
                MerchantCode = x.MerchantCode,
                MerchantName = x.MerchantName,
                StockNumber = x.StockNumber,
                OccupyNumber = x.OccupyNumber,
               // PicID=x.PicID,
                CreateTime=x.CreateTime

            }).OrderByDescending(x => x.CreateTime);
            return query;
        }
    }
    public class MaterialList
    {
        public List<Material_ListView> EntityList { get; set; }
        public int TotalPages { get; set; }
        public long TotalRecords { get; set; }

    }
    public class Material_ListView : BasePoco
    {
        [Display(Name = "商品名称")]
        public string GoodsName { get; set; }

        [Display(Name = "物料编号")]
        public string MaterialCode { get; set; }

        [Display(Name = "物料规格")]
        public string Specification { get; set; }

        [Display(Name = "销售库存")]
        public int? StockNumber { get; set; }

        [Display(Name = "商户")]
        public string MerchantID { get; set; }

        [Display(Name = "商户编号")]
        public string MerchantCode { get; set; }

        [Display(Name = "商户名称")]
        [StringLength(200)]
        public string MerchantName { get; set; }
        [Display(Name = "占用")]
        public int OccupyNumber { get; set; }


      //  public Guid? PicID { get; set; }

    }
}
