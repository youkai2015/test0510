using System;
using System.Web;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using Stock.ViewModel.HomeVMs;

namespace Stock.Controllers.Framework
{
    [ActionDescription("MainModule")]
    public class HomeController : BaseController
    {


        [ActionDescription("StartPage")]
        [AllRights]
        public ActionResult FrontPage()
        {
            return PartialView();
        }

        [ActionDescription("MainPage")]
        [AllRights]
        public ActionResult Index()
        {
            return View();
        }

        [ActionDescription("PopupPage")]
        [Public]
        public ActionResult PopUpIndex()
        {
            return View();
        }

        [ActionDescription("LeftMenu")]
        [AllRights]
        [NormalPartial]
        public ActionResult Menu()
        {
            var vm = CreateVM<MainMenuVM>();
            if (Configs.IsQuickDebug == true)
            {
                vm.MenuData = GetAllControllerModules().GetTree();
            }
            else
            {
                MenuVM menu = CreateVM<MenuVM>(values: x => x.FFMenus == BaseController.FFMenus);
                vm.MenuData = menu.GetMenus().GetTree(x => x.GetMLContentValue(y => y.PageName, null), x => x.Url, x => x.DomainID.ToString(), icon: x => x.IconID.ToString());
            }
            return PartialView(vm);
        }

        [ActionDescription("TopBar")]
        [NormalPartial]
        [AllRights]
        public ActionResult TopBar()
        {

            TopBarVM vm = CreateVM<TopBarVM>();
            return PartialView(vm);
        }

        [ActionDescription("ChangeLanguage")]
        [AllRights]
        public ActionResult ChangeLanguage(string LanguageCode)
        {
            HttpCookie cookie = new HttpCookie(Configs.CookiePre + "FFLanguage", LanguageCode);
            cookie.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Add(cookie);
            return null;
        }

    }
}