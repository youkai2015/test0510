using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;
using Stock.Resource;

namespace Stock.ViewModel.HomeVMs
{
    /// <summary>
    /// 登录VM
    /// </summary>
    public class LoginVM : BaseVM
    {
        public List<MoneyListClass> MoneyList { get; set; }
        public string loginName = "用户名：";
        public string loginPassword = "密　码：";
        public string ReturnPassword = "忘记密码？";
        public string loginTitle = "用户登录";
        public string loginBtn = "登录";

        [Display(Name = "ITCode")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        public string ITCode { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Password { get; set; }

        [Display(Name = "TheLanguage", ResourceType = typeof(Language))]
        public string TheLanguage { get; set; }
        public List<ComboSelectListItem> AllLanguages { get; set; }

        public string Redirect { get; set; }

        public string SessionID { get; set; }

        #region 测试使用
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        //零
        //一
        //二
        //三
        //四
        //五
        //六
        //七
        //八
        //九
        public enum TestEnum
        {
            T0 = 0,
            T1 = 1,
            T2 = 2,
            T3 = 3,
            T4 = 4,
            T5 = 5,
            T6 = 6,
            T7 = 7,
            T8 = 8,
            T9 = 9
        }
        public List<Guid> AllGuids { get; set; }

        public TestEnum? TestEnumCombo1 { get; set; }
        public TestEnum TestEnumCombo2 { get; set; }
        public List<string> CheckGroups { get; set; }

        public bool CheckBox { get; set; }

        public string TestHidden { get; set; }

        [Required()]
        public Guid? TestFileID { get; set; }

        public List<string> TestHiddenLsit { get; set; }

        [Display(Name = "测试时间")]
        public DateTime TestDateTime { get; set; }

        public List<TreeSelectListItem> AllTree { get; set; }
        public List<ComboSelectListItem> AllSelect { get; set; }

        public List<Guid> TreeGuidLists { get; set; }
        public Guid TreeGuid { get; set; }
        public List<ComboSelectListItem> All { get; set; }
        #endregion

        public LoginVM()
        {
        }

        protected override void InitVM()
        {
            //初始化所有语言的下拉列表
            AllLanguages = Configs.Languages.ToListItems(x => x.LanguageName, x => x.LanguageCode);
            //初始化当前语言
            TheLanguage = CurrentLanguage.LanguageCode;

            #region 测试代码    可以删除
            //All = new List<SelectListItem>();
            //All.Add(new SelectListItem() { Text = "一", Value = "1" });
            //All.Add(new SelectListItem() { Text = "二", Value = "2" });
            //All.Add(new SelectListItem() { Text = "三", Value = "3" });

            //var sss = DC.Set<FrameworkUser>().GetSelectListItems(null, x => x.ITCode, null, x => x.ID);
            #region 树形下拉框测试
            //AllTree = new List<TreeSelectListItem>();

            //var parent1 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent1", children = new List<TreeSelectListItem>(), leaf = false };
            //var parent2 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent2", children = new List<TreeSelectListItem>(), leaf = false };
            //var parent3 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "Parent3", children = new List<TreeSelectListItem>(), leaf = false };

            //parent1.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child1-1", leaf = true });

            //parent2.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child2-1", leaf = true });
            //parent2.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child2-2", leaf = true });

            //parent3.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-1", leaf = true });

            //var child11 = new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2", children = new List<TreeSelectListItem>(), leaf = false };
            //child11.children.Add(new TreeSelectListItem() { id = "0eec5896-0037-4c15-b596-5e57d6cae842", text = "child3-2-1", leaf = true });
            //child11.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2-2", leaf = true });
            //child11.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-2-3", leaf = true });

            //parent3.children.Add(child11);
            //parent3.children.Add(new TreeSelectListItem() { id = Guid.NewGuid().ToString(), text = "child3-3", leaf = true });

            //AllTree.Add(parent1);
            //AllTree.Add(parent2);
            //AllTree.Add(parent3);

            //AllTree = new List<TreeSelectListItem>();
            //DC.Set<FrameworkDepartment>().ToList().Select(x => new TreeSelectListItem() { ID = x.ID.ToString(), Text = x.DepartmentCode, Leaf = x.GetChildren().Count() == 0 ? true : false });

            //AllTree = DC.Set<FrameworkDepartment>().GetTreeSelectListItemsForML(null, x => x, x => x.DepartmentName, null);
            #endregion

            //TestDateTime = DateTime.Now;

            //TestFileID = Guid.NewGuid();
            //TestHiddenLsit = new List<string>();
            //TestHiddenLsit.Add("test1");
            //TestHiddenLsit.Add("test2");
            //TestHiddenLsit.Add("test3");
            //TestHiddenLsit.Add("test4");
            //TestHiddenLsit.Add("test5");
            //TestHiddenLsit.Add("test6");
            //TestHiddenLsit.Add("test7");
            //TestHiddenLsit.Add("test8");
            //TestHiddenLsit.Add("test9");

            //AllSelect = new List<SelectListItem>();
            //AllSelect.Add(new SelectListItem() { Text = "一", Value = "1" });
            //AllSelect.Add(new SelectListItem() { Text = "二", Value = "2" });
            //AllSelect.Add(new SelectListItem() { Text = "三", Value = "3" });
            //AllSelect.Add(new SelectListItem() { Text = "四", Value = "4" });
            //AllSelect.Add(new SelectListItem() { Text = "五", Value = "5" });
            //AllSelect.Add(new SelectListItem() { Text = "六", Value = "6" });
            //AllSelect.Add(new SelectListItem() { Text = "七", Value = "7" });
            //AllSelect.Add(new SelectListItem() { Text = "八", Value = "8" });
            //AllSelect.Add(new SelectListItem() { Text = "九", Value = "9" });
            #endregion
        }

        /// <summary>
        /// 进行登录
        /// </summary>
        /// <param name="OutsidePris">外部传递的页面权限</param>
        /// <returns>登录用户的信息</returns>
        public LoginUserInfo DoLogin(bool IgnorePris = false)
        {
            //根据用户名和密码查询用户
            var user = DC.Set<FrameworkUser>()
                .Where(x => x.ITCode.ToLower() == ITCode.ToLower() && x.Password.ToLower() == Password.ToLower() && x.IsValid == true)
                .Include(x => x.Roles)
                .Include(x => x.Department.Company)
                .SingleOrDefault();
            //如果没有找到则输出错误
            if (user == null)
            {
                MSD.AddModelError("", Language.LogonFail);
                return null;
            }

            var roleIDs = user.Roles.Select(x => x.ID).ToList();
            //查找登录用户的数据权限
            var dpris = DC.Set<DataPrivilege>()
                .Where(x => x.UserID == user.ID && x.DomainID == null)
                .ToList();
            //生成并返回登录用户信息
            LoginUserInfo rv = new LoginUserInfo();
            rv.ID = user.ID;
            rv.ITCode = user.ITCode;
            rv.Name = user.Name;
            rv.Roles = user.Roles;
            rv.DataPrivileges = dpris;
            if (IgnorePris == false)
            {
                //查找登录用户的页面权限
                var pris = DC.Set<FunctionPrivilege>()
                    .Where(x => x.UserID == user.ID || (x.RoleID != null && roleIDs.Contains(x.RoleID.Value)))
                    .ToList();
                rv.FunctionPrivileges = pris;
            }
            return rv;
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="itcode">ITCode</param>
        /// <param name="pwd">加密后的PWD</param>
        /// <returns>空为成功</returns>
        public string LoginUserValidate(string itcode, string pwd)
        {

            string msg = "";
            FrameworkUser user0 = DC.Set<FrameworkUser>().Where(x => x.ITCode.ToLower() == itcode.ToLower()).FirstOrDefault();

            if (user0 == null)//用户名错误
            {

                msg = "用户名或密码错误";
            }
            else
            {
                if (user0.IsValid == false)
                {
                    msg = "您的用户已被禁用";
                }
                else
                {
                    if (user0.Password != pwd)//密码错误
                    {
                        msg = "用户名或密码错误";
                    }
                }
            }

            return msg;
        }
    }

    public class MoneyListClass
    {
        public string testMoney { get; set; }
    }
}