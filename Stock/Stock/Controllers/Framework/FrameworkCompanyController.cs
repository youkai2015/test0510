using System;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using Stock.Resource;
using Stock.ViewModel.Framework.FrameworkCompanyVMs;

namespace Stock.Controllers.Framework
{
    [ActionDescription("CompanyManagement")]
    public class FrameworkCompanyController : BaseController
    {
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<FrameworkCompanyListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Create")]
        public ActionResult Create()
        {
            var vm = CreateVM<FrameworkCompanyVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(FrameworkCompanyVM vm)
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

        [ActionDescription("Edit")]
        public ActionResult Edit(Guid id)
        {
            var vm = CreateVM<FrameworkCompanyVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        public ActionResult Edit(FrameworkCompanyVM vm)
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

        [ActionDescription("Delete")]
        public ActionResult Delete(Guid id)
        {
            var vm = CreateVM<FrameworkCompanyVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection noUser)
        {
            var vm = CreateVM<FrameworkCompanyVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                DC = DC.ReCreate();
                vm = CreateVM<FrameworkCompanyVM>(id);
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }

        [ActionDescription("Details")]
        public PartialViewResult Details(Guid id)
        {
            var vm = CreateVM<FrameworkCompanyVM>(id);
            return PartialView("Details", vm);
        }

        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(Guid[] IDs)
        {
            var vm = CreateVM<FrameworkCompanyBatchVM>(IDs: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(FrameworkCompanyBatchVM vm, FormCollection noUse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }

        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData()
        {
            var vm = CreateVM<FrameworkCompanyImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData(FrameworkCompanyImportVM vm, FormCollection noUse)
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

    }
}