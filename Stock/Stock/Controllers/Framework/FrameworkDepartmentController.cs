using System;
using System.Linq;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Mvc;
using Stock.Model;
using Stock.Resource;
using Stock.ViewModel.Framework.FrameworkDepartmentVMs;

namespace Stock.Controllers.Framework
{
    [ActionDescription("DepManagement")]
    public class FrameworkDepartmentController : BaseController
    {
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<FrameworkDepartmentListVM>();
            return PartialView(vm);
        }
        [HttpPost]
        [ActionDescription("Search")]
        public ActionResult Index(FrameworkDepartmentListVM vm)
        {
            return PartialView(vm);
        }

        [ActionDescription("Create")]
        public ActionResult Create(Guid? id)
        {
            var vm = CreateVM<FrameworkDepartmentVM>();
            if (id != null)
            {
                vm.Entity.CompanyID = DC.Set<FrameworkDepartment>().Where(x => x.ID == id).Select(x => x.CompanyID).FirstOrDefault();
                vm.Entity.ParentID = id;
            }
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(FrameworkDepartmentVM vm)
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
            var vm = CreateVM<FrameworkDepartmentVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        public ActionResult Edit(FrameworkDepartmentVM vm)
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
            var vm = CreateVM<FrameworkDepartmentVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection noUser)
        {
            var vm = CreateVM<FrameworkDepartmentVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
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
            var v = CreateVM<FrameworkDepartmentVM>(id);
            return PartialView("Details", v);
        }

        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(Guid[] IDs)
        {
            var vm = CreateVM<FrameworkDepartmentBatchVM>(IDs: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(FrameworkDepartmentBatchVM vm, FormCollection noUse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                vm.DoReInit();
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Language.OperationSucc);

            }
        }

        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(Guid[] IDs)
        {
            var vm = CreateVM<FrameworkDepartmentBatchVM>(IDs: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(FrameworkDepartmentBatchVM vm, FormCollection noUse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Language.OperationSucc);
            }
        }

        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData()
        {
            var vm = CreateVM<FrameworkDepartmentImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData(FrameworkDepartmentImportVM vm, FormCollection noUse)
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