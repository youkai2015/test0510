using System.Collections.Generic;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.HomeVMs
{
    /// <summary>
    /// 首页上方标题栏VM
    /// </summary>
    public class TopBarVM : BaseVM
    {
        //登录用户名
        public string UserName { get; set; }
        //系统支持语言的下拉菜单数据
        public List<ComboSelectListItem> AllLanguages { get; set; }
        //当前语言
        public string CurrentLanguageCode { get; set; }

        protected override void InitVM()
        {
            //如果有登录用户信息，则给用户名赋值
            if (LoginUserInfo != null)
            {
                UserName = LoginUserInfo.Name;
            }
            //初始化系统支持的语言的下拉菜单
            AllLanguages = Configs.Languages.ToListItems(x => x.LanguageName, x => x.LanguageCode);
            //初始化当前语言
            CurrentLanguageCode = CurrentLanguage.LanguageCode;
        }
    }
}