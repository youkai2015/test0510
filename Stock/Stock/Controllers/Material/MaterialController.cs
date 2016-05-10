using Stock.DataAccess;
using Stock.Resource;
using Stock.ViewModel.MaterialVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Mvc;

namespace Stock.Controllers.Material
{

    [ActionDescription("物料管理")]
    public class MaterialController : BaseController
    {
        // GET: Material
        [ActionDescription("列表")]
        public ActionResult Index()
        {
            var vm = CreateVM<MaterialListVM>();
            return PartialView(vm);
        }

        #region Create
        [ActionDescription("新建")]

        public ActionResult Create()
        {
            var vm = CreateVM<MaterialVM>();
            return PartialView(vm);
        }
        [ActionDescription("新建")]
        [HttpPost]
        public ActionResult Create(MaterialVM vm)
        {

            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region Edit
        [ActionDescription("修改")]
        public ActionResult Edit(Guid id, string MerchantCode)
        {
            //if (MerchantCode != null)
            //{
            //    DC = new DataContext(MerchantCode);
            //}
            var vm = CreateVM<MaterialVM>(id);
            return PartialView(vm);
        }
        [ActionDescription("修改")]
        [HttpPost]
        public ActionResult Edit(MaterialVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {

                vm.DoEdit();
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region Delete
        [ActionDescription("删除")]

        public ActionResult Delete(Guid id, string MerchantCode)
        {
            //if (MerchantCode != null)
            //{
            //    DC = new DataContext(MerchantCode);
            //}
            var vm = CreateVM<MaterialVM>(id);
            return PartialView(vm);
        }
        [ActionDescription("删除")]
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection noUser)
        {
            #region
            //string MerchantCode = Request["Entity.MerchantCode"].ToString();//获取商户编号
            //if (MerchantCode != null)
            //{
            //    DC = new DataContext(MerchantCode);
            //}
            #endregion
            var vm = CreateVM<MaterialVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                vm = CreateVM<MaterialVM>(id);
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 批量导入
        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData()
        {
            var vm = CreateVM<MaterialImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData(MaterialImportVM vm, FormCollection noUse)
        {
            if (vm.ErrorListVM.ErrorList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Language.OperationSucc + " " + vm.EntityList.Count.ToString() + " " + Language.Record);
            }
        }
        #endregion

        #region 选择子物料

        [ActionDescription("选择子物料")]
        public ActionResult SelectMaterial(Guid id,string MerchantCode)
        {
            var vm = CreateVM<SelectMaterialVM>(id);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("选择子物料")]
        public ActionResult SelectMaterial(SelectMaterialVM vm)
        {
            vm.AddChildMaterial();
            return FFResult().CloseDialog().Alert("操作成功");
        }
        #endregion
        #region 选择商家
        [ActionDescription("选择商家")]
        public ActionResult SelectMerchant()
        {
            var vm = CreateVM<MerchantListVM>();
            vm.Searcher.MerchantName = HttpUtility.UrlEncode(vm.Searcher.MerchantName);
            vm.SessionID = Request.Cookies["ASP.NET_SessionId"].Value;
            return PartialView(vm);
        }
        #endregion
    }
}