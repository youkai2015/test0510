namespace Stock.DataAccess.Migrations
{
    using MySql.Data.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using WalkingTec.Mvvm.Abstraction;
    using WalkingTec.Mvvm.Core;
    using Stock.Model;

    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public static List<FrameworkModule> AllModules { get; set; }
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            var cs = Configs.ConnectStrings.Where(x => x.Name.ToLower() == "default" && x.ProviderName == "MySql.Data.MySqlClient").FirstOrDefault();
            if (cs != null)
            {
                SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
                SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHC(conn, schema));
            }
        }

        protected override void Seed(DataContext con)
        {
            IDataContext context = con as IDataContext;
            context = context.ReCreate();
            if (context.Set<FrameworkUser>().ToList().Count() > 0)
            {
                return;
            }
            try
            {
                context = context.ReCreate();
                for (int i = 0; i < 1000; i++)
                {
                    context.Set<EncHash>().Add(new EncHash { Key = Guid.NewGuid(), Hash = i });
                }
                context.SaveChanges();

                #region 模块初始化
                context = context.ReCreate();
                foreach (var module in AllModules)
                {
                    module.CreateTime = DateTime.Now;
                    context.Set<FrameworkModule>().Add(module);
                }
                context.SaveChanges();

                #endregion

                #region 角色初始化

                context = context.ReCreate();
                FrameworkRole adminRole = new FrameworkRole
                {
                    RoleCode = "000001",
                    CreateTime = DateTime.Now,
                    MLContents = new List<FrameworkRoleMLContent>
                    {
                        new FrameworkRoleMLContent{ LanguageCode="zh-cn", RoleName="管理员"},
                        new FrameworkRoleMLContent{ LanguageCode="en", RoleName = "Administrator"}
                    }
                };

                context.Set<FrameworkRole>().Add(adminRole);
                context.SaveChanges();

                #endregion

                #region 用户初始化

                context = context.ReCreate();
                context.Set<FrameworkRole>().Attach(adminRole);
                FrameworkUser adminUser = new FrameworkUser
                {
                    ITCode = "admin",
                    Password = "000000",
                    StartWorkDate = DateTime.Now,
                    Roles = new List<FrameworkRole>(new FrameworkRole[] { adminRole }),
                    Name = "Admin",
                    CreateTime = DateTime.Now,
                    IsValid = true
                };
                context.Set<FrameworkUser>().Add(adminUser);
                context.SaveChanges();
                #endregion

                #region 菜单初始化

                context = context.ReCreate();
                context.Set<FrameworkRole>().Attach(adminRole);

                #region 系统管理

                FrameworkMenu systemManagement = GetFolderMenu("zh-cn,系统管理;en,System Management", new List<FrameworkRole> { adminRole }, null);

                FrameworkMenu logList = GetMenu(AllModules, "Admin", "ActionLog", "Index", new List<FrameworkRole> { adminRole }, null, 1);
                FrameworkMenu companyList = GetMenu(AllModules, null, "FrameworkCompany", "Index", new List<FrameworkRole> { adminRole }, null, 2);
                FrameworkMenu departmentList = GetMenu(AllModules, null, "FrameworkDepartment", "Index", new List<FrameworkRole> { adminRole }, null, 3);
                FrameworkMenu roleList = GetMenu(AllModules, "Admin", "FrameworkRole", "Index", new List<FrameworkRole> { adminRole }, null, 4);
                FrameworkMenu userList = GetMenu(AllModules, null, "FrameworkUser", "Index", new List<FrameworkRole> { adminRole }, null, 5);
                FrameworkMenu menuList = GetMenu(AllModules, "Admin", "FrameworkMenu", "Index", new List<FrameworkRole> { adminRole }, null, 6);
                FrameworkMenu dpList = GetMenu(AllModules, "Admin", "DataPrivilege", "Index", new List<FrameworkRole> { adminRole }, null, 7);
                FrameworkMenu domainList = GetMenu(AllModules, "Admin", "FrameworkDomain", "Index", new List<FrameworkRole> { adminRole }, null, 8);
                FrameworkMenu emailLogList = GetMenu(AllModules, "Admin", "EmailLog", "Index", new List<FrameworkRole> { adminRole }, null, 9);
                FrameworkMenu emailTemplateList = GetMenu(AllModules, "Admin", "EmailTemplate", "Index", new List<FrameworkRole> { adminRole }, null, 10);
                FrameworkMenu noticeList = GetMenu(AllModules, "Admin", "FrameworkNotice", "Index", new List<FrameworkRole> { adminRole }, null, 11);
                FrameworkMenu monitorList = GetMenu(AllModules, "Admin", "FrameworkServiceMonitor", "Index", new List<FrameworkRole> { adminRole }, null, 12);
                //添加HomeController下的通用方法，主要是一些导出的通用函数
                var ExportActs = AllModules.Where(x => x.ClassName == "Home" && x.Area != null && x.Area.Prefix.ToLower() == "webapi").SelectMany(x => x.Actions).ToList();
                foreach (var exp in ExportActs)
                {
                    context.Set<FrameworkMenu>().Add(GetMenuFromAction(exp, false, new List<FrameworkRole> { adminRole }, null));
                }

                List<FrameworkMenu> ToAdd = new List<FrameworkMenu>
                {
                    logList, companyList, departmentList, roleList, userList, menuList, dpList, domainList,emailLogList,emailTemplateList,noticeList,monitorList
                };

                foreach (var item in ToAdd)
                {
                    if (item != null)
                    {
                        systemManagement.Children.Add(item);
                    }
                }

                #endregion

                context.Set<FrameworkMenu>().Add(systemManagement);
                context.SaveChanges();

                #endregion

                #region 数据权限初始化
                //context = context.ReCreate();
                //context.DataPrivileges.Add(
                //    new DataPrivilege
                //    {
                //        TableName = "FrameworkCompany",
                //        User = adminUser,
                //        CreateTime = DateTime.Now
                //    }
                //    );
                //context.Set<DataPrivilege>().Add(
                //    new DataPrivilege
                //    {
                //        TableName = "FrameworkDepartment",
                //        UserID = adminUser.ID,
                //        CreateTime = DateTime.Now
                //    }
                //    );
                //context.SaveChanges();
                #endregion

                #region 框架必须的存储过程初始化

                //SeedDataAndSP(con);

                #endregion

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 框架必须的存储过程初始化
        /// </summary>
        /// <param name="con"></param>
        //private void SeedDataAndSP(DataContext con)
        //{
        //    string rootPath = AppDomain.CurrentDomain.BaseDirectory;

        //    IDataContext context = con as IDataContext;
        //    context = context.ReCreate();
        //    string sql = string.Empty;
        //    string fullPath = string.Empty;
        //    if (context.Database.Connection is System.Data.SqlClient.SqlConnection)
        //    {
        //        fullPath = rootPath + "SQLScripts\\MSSQL";
        //    }
        //    else if (context.Database.Connection is MySql.Data.MySqlClient.MySqlConnection)
        //    {
        //        fullPath = rootPath + "SQLScripts\\MySQL";
        //    }
        //    else
        //    {
        //        return;
        //    }

        //    if (Directory.Exists(fullPath))
        //    {
        //        List<string> fileList = null;
        //        fileList = Utils.GetAllFilePathRecursion(fullPath, fileList);

        //        foreach (var file in fileList)
        //        {
        //            if (file.ToLower().EndsWith(".sql"))
        //            {
        //                sql = Utils.ReadTxt(file);
        //                try
        //                {
        //                    context.Database.RunSql(sql);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}

        private FrameworkMenu GetFolderMenu(string FolderText, List<FrameworkRole> allowedRoles, List<FrameworkUser> allowedUsers, bool isShowOnMenu = true, bool isInherite = false)
        {
            FrameworkMenu menu = new FrameworkMenu
            {
                MLContents = new List<FrameworkMenuMLContent>(),
                Children = new List<FrameworkMenu>(),
                Privileges = new List<FunctionPrivilege>(),
                ShowOnMenu = isShowOnMenu,
                IsInherit = isInherite,
                IsInside = true,
                FolderOnly = true,
                IsPublic = false,
                CreateTime = DateTime.Now,
                DisplayOrder = 1
            };

            string[] pair = FolderText.Split(';');

            foreach (var ml in pair)
            {
                string[] temp = ml.Split(',');
                if (temp.Length == 2)
                {
                    menu.MLContents.Add(new FrameworkMenuMLContent { LanguageCode = temp[0], PageName = temp[1] });
                }
            }

            if (allowedRoles != null)
            {
                foreach (var role in allowedRoles)
                {
                    menu.Privileges.Add(new FunctionPrivilege { RoleID = role.ID, Allowed = true });

                }
            }
            if (allowedUsers != null)
            {
                foreach (var user in allowedUsers)
                {
                    menu.Privileges.Add(new FunctionPrivilege { UserID = user.ID, Allowed = true });
                }
            }

            return menu;
        }

        private FrameworkMenu GetMenu(List<FrameworkModule> allModules, string areaName, string controllerName, string actionName, List<FrameworkRole> allowedRoles, List<FrameworkUser> allowedUsers, int displayOrder)
        {
            var acts = allModules.Where(x => x.ClassName == controllerName && (areaName == null || x.Area.Prefix.ToLower() == areaName.ToLower())).SelectMany(x => x.Actions).ToList();
            var act = acts.Where(x => x.MethodName == actionName).SingleOrDefault();
            var rest = acts.Where(x => x.MethodName != actionName).ToList();
            FrameworkMenu menu = GetMenuFromAction(act, true, allowedRoles, allowedUsers, displayOrder);
            if (menu != null)
            {
                for (int i = 0; i < rest.Count; i++)
                {
                    if (rest[i] != null)
                    {
                        menu.Children.Add(GetMenuFromAction(rest[i], false, allowedRoles, allowedUsers, (i + 1)));
                    }
                }
            }
            return menu;
        }

        private FrameworkMenu GetMenuFromAction(FrameworkAction act, bool isMainLink, List<FrameworkRole> allowedRoles, List<FrameworkUser> allowedUsers, int displayOrder = 1)
        {
            if (act == null)
            {
                return null;
            }
            FrameworkMenu menu = new FrameworkMenu
            {
                ActionID = act.ID,
                ModuleID = act.ModuleID,
                Url = "/" + act.Module.ClassName + "/" + act.MethodName,
                Privileges = new List<FunctionPrivilege>(),
                ShowOnMenu = isMainLink,
                FolderOnly = false,
                MLContents = new List<FrameworkMenuMLContent>(),
                Children = new List<FrameworkMenu>(),
                IsInherit = true,
                IsPublic = false,
                IsInside = true,
                DisplayOrder = displayOrder,
                CreateTime = DateTime.Now
            };
            if (act.Module.Area != null)
            {
                menu.Url = "/" + act.Module.Area.Prefix + menu.Url;
            }
            if (isMainLink)
            {
                foreach (var ml in act.Module.MLContents)
                {
                    menu.MLContents.Add(new FrameworkMenuMLContent { LanguageCode = ml.LanguageCode, PageName = ml.ModuleName, ModuleName = ml.ModuleName, ActionName = act.MLContents.Where(x => x.LanguageCode == ml.LanguageCode).Select(x => x.ActionName).FirstOrDefault() });
                }
            }
            else
            {
                foreach (var ml in act.MLContents)
                {
                    menu.MLContents.Add(new FrameworkMenuMLContent { LanguageCode = ml.LanguageCode, PageName = ml.ActionName, ActionName = ml.ActionName, ModuleName = act.Module.MLContents.Where(x => x.LanguageCode == ml.LanguageCode).Select(x => x.ModuleName).FirstOrDefault() });
                }
            }
            if (allowedRoles != null)
            {
                foreach (var role in allowedRoles)
                {
                    menu.Privileges.Add(new FunctionPrivilege { RoleID = role.ID, Allowed = true });

                }
            }
            if (allowedUsers != null)
            {
                foreach (var user in allowedUsers)
                {
                    menu.Privileges.Add(new FunctionPrivilege { UserID = user.ID, Allowed = true });
                }
            }
            return menu;
        }
    }
}
