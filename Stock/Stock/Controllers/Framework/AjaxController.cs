using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using Stock.Model;

namespace Stock.Controllers.Framework
{
    [ActionDescription("Ajax")]
    [AllRights()]
    public class AjaxController : BaseController
    {
        [ActionDescription("GetDepartmentByCompany")]
        public JsonResult GetDepartmentByCompany(Guid? id)
        {
            var rv = DC.Set<FrameworkDepartment>().GetTreeSelectListItemsForML(x => x.CompanyID == id, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }

        [ActionDescription("GetDepartmentByCompanies")]
        public JsonResult GetDepartmentByCompanies(List<Guid> id)
        {
            List<ComboSelectListItem> rv = null;
            if (id != null)
            {
                rv = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(x => (id.Contains(x.CompanyID.Value)), x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
            }
            else
            {
                rv = DC.Set<FrameworkDepartment>().GetSelectListItemsForML(null, x => x, y => y.DepartmentName, LoginUserInfo.DataPrivileges);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [ActionDescription("GetItemsByTableName")]
        public JsonResult GetItemsByTableName(string name)
        {
            var rv = new List<ComboSelectListItem>();
            var dps = BaseVM.AllDPS.Where(x => x.ModelName == name).SingleOrDefault();
            if (dps != null)
            {
                rv = dps.GetItemList(DC, null);
            }
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [GetUser("通过工厂获取用户")]
        public ActionResult GetFactoryUsers()
        {
            return View();
        }
        [GetUser("获取用户")]
        public ActionResult GetUsersByID()
        {
            return View();
        }

        [Public]
        public JsonResult GetUser(string id)
        {
            var rv = new List<ComboSelectListItem>();
            rv = DC.Set<FrameworkUser>().GetSelectListItems(x => x.ITCode == id, x => x.Name, null, x => x.ITCode);
            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        //[Public]
        //public JsonResult GetTest1(string id)
        //{
        //    var rv = new List<SelectListItem>();
        //    rv = typeof(CodeArt.Models.Framework.UserTypeEnum).ToListItems();
        //    return Json(rv, JsonRequestBehavior.AllowGet);
        //}
        //[Public]
        //public JsonResult GetTest2(string id)
        //{
        //    var AllTree = new List<TreeSelectListItem>();

        //    var parent1 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent1", children = new List<TreeSelectListItem>(), leaf = false };
        //    var parent2 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent2", children = new List<TreeSelectListItem>(), leaf = false };
        //    var parent3 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent3", children = new List<TreeSelectListItem>(), leaf = false };

        //    parent1.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child1-1", leaf = true });

        //    parent2.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child2-1", leaf = true });
        //    parent2.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child2-2", leaf = true });

        //    parent3.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-1", leaf = true });

        //    var child11 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2", children = new List<TreeSelectListItem>(), leaf = false };
        //    child11.children.Add(new TreeSelectListItem() { id = "0eec5896-0037-4c15-b596-5e57d6cae842", text = "child3-2-1", leaf = true });
        //    child11.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2-2", leaf = true });
        //    child11.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2-3", leaf = true });

        //    parent3.children.Add(child11);
        //    parent3.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-3", leaf = true });

        //    AllTree.Add(parent1);
        //    AllTree.Add(parent2);
        //    AllTree.Add(parent3);
        //    return Json(AllTree, JsonRequestBehavior.AllowGet);
        //}
    }
}