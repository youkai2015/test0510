using System;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Mvc;
using Stock.Resource;
using Stock.ViewModel.Framework.FrameworkUserVMs;

namespace Stock.Controllers.Framework
{
    [ActionDescription("UserManagement")]
    public class FrameworkUserController : BaseController
    {
        #region 动态列测试
        //[ActionDescription("Search")]
        //[HttpPost]
        //public ActionResult Index(FrameworkUserListVM vm)
        //{
        //    return PartialView(vm);
        //} 
        #endregion
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<FrameworkUserListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Create")]
        public ActionResult Create()
        {
            var vm = CreateVM<FrameworkUserVM>();
            #region 邮件测试
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("username", LoginUserInfo.ITCode);
            //dic.Add("company", "联想利泰");
            //dic.Add("IP", "192.168.1.1");
            //dic.Add("uid", LoginUserInfo.Name);
            //dic.Add("id", LoginUserInfo.ID.ToString());
            //Guid id1 = Guid.Parse("049100F5-D06C-4400-9DB9-B5D6A8B1B354");
            //Guid id2 = Guid.Parse("6D793439-6BB0-47FC-8656-0A686E959C1C");

            //var fa = DC.Set<FileAttachment>().Where(x => x.ID == id1 || x.ID == id2).ToList();
            //vm.AddEmail("emailtest", dic, "712327435@qq.com", System.Net.Mail.MailPriority.Normal, fileAttachment: fa); 
            #endregion
            return PartialView(vm);
        }

        [HttpPost]
        [FixConnection(DBOperationEnum.Write, "default")]
        [ActionDescription("Create")]
        public ActionResult Create(FrameworkUserVM vm)
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
            var vm = CreateVM<FrameworkUserVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        public ActionResult Edit(FrameworkUserVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                return FFResult().CloseDialog().RefreshGridRow(typeof(FrameworkUserListVM), vm.Entity.ID);
            }
        }

        [ActionDescription("Delete")]
        public ActionResult Delete(Guid id)
        {
            var vm = CreateVM<FrameworkUserVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection noUser)
        {
            var vm = CreateVM<FrameworkUserVM>(id);
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
            var v = CreateVM<FrameworkUserVM>(id);
            return PartialView("Details", v);
        }

        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(Guid[] IDs)
        {
            var vm = CreateVM<FrameworkUserBatchVM>(IDs: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(FrameworkUserBatchVM vm, FormCollection noUse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
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
            var vm = CreateVM<FrameworkUserBatchVM>(IDs: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(FrameworkUserBatchVM vm, FormCollection noUse)
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
            var vm = CreateVM<FrameworkUserImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchImport")]
        public ActionResult ImportExcelData(FrameworkUserImportVM vm, FormCollection noUse)
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