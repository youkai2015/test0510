using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using Stock.Resource;
using Stock.ViewModel.HomeVMs;

namespace Stock.Controllers.Framework
{
    [AllRights]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    [ActionDescription("Login")]
    public class LoginController : BaseController
    {
        [ActionDescription("Login")]
        [PopUp]
        [Public]
        public ActionResult Login()
        {
            LoginVM vm = CreateVM<LoginVM>();
            vm.Redirect = Request["rd"];
            if (Configs.IsQuickDebug == true)
            {
                vm.ITCode = "admin";
                vm.Password = "000000";
            }
            return PartialView(vm);
        }

        [HttpPost]
        [PopUp]
        [Public]
        [ActionDescription("Login")]
        public ActionResult Login(LoginVM vm)
        {
            var user = vm.DoLogin();
            if (user == null)
            {
                return PartialView(vm);
            }
            else
            {
                LoginUserInfo = user;
                string url = "";
                if (!string.IsNullOrEmpty(vm.Redirect))
                {
                    url = vm.Redirect;
                }
                else
                {
                    url = "/";
                }
                url = Configs.VirtualDir + url;
                return Content("<script>window.location.replace('" + HttpUtility.UrlDecode(url) + "');</script>");
            }
        }

        [ActionDescription("Logout")]
        [PopUp]
        public ActionResult Logout()
        {
            LoginUserInfo = null;
            Session.Abandon();
            var url = "/Login/Login";
            return Content("<script>window.top.location.href = '" + url + "';</script>");
        }

        [ActionDescription("ChangeLanguage")]
        [PopUp]
        [HttpPost]
        [Public]
        public ActionResult LoginLanguage(LoginVM vm)
        {
            CultureInfo ci = new CultureInfo(vm.TheLanguage);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            HttpCookie cookie = new HttpCookie(Configs.CookiePre + "FFLanguage", vm.TheLanguage);
            cookie.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Add(cookie);
            ModelState.Clear();
            return PartialView("Login", vm);
        }

        [ActionDescription("EditPassword")]
        public ActionResult ChangePassword()
        {
            ChangePasswordVM vm = new ChangePasswordVM();
            vm.ITCode = LoginUserInfo.ITCode;
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("EditPassword")]
        public ActionResult ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoChange();
                return FFResult().CloseDialog().Alert(Language.OperationSucc);
            }
        }

        [ActionDescription("找回密码")]
        [Public]
        [PopUp]
        public ActionResult ReturnPassword()
        {
            ReturnPasswordVM vm = CreateVM<ReturnPasswordVM>();
            return PartialView(vm);
        }

        [ActionDescription("找回密码")]
        [Public]
        [PopUp]
        [HttpPost]
        public ActionResult ReturnPassword(ReturnPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.SendMail(vm.loginName);
                return FFResult().CloseDialog().Alert(Language.OperationSucc);
            }
        }

        [Public]
        [PopUp]
        [HttpGet]
        public JsonResult ValidateLoginUser(string ITCode, string Msg)
        {
            LoginVM lv = new LoginVM();
            Msg = Utils.GetMD5String(Msg);
            //加密时需要处理此处代码
            string m = lv.LoginUserValidate(ITCode, Msg);
            return Json(m, JsonRequestBehavior.AllowGet);
        }
    }
}