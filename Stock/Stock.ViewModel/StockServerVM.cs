using Component.Service;
using Stock.ViewModel.MaterialVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel
{
    public class StockServerVM
    {
        private StockServerVM()
        {


        }
        #region 获取商户信息
        public static Merchant_ListView_AdminService GetMerchant(string SessionID, string CurrentPage, string MerchantCode, string MerchantName)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("SessionID", SessionID); //本地测试使用
            //parms.Add("SessionID", SessionID); //发布时使用
            parms.Add("CurrentPage", CurrentPage);
            parms.Add("MerchantCode", MerchantCode);
            parms.Add("MerchantName", MerchantName);
            var query = ServiceHelper.CallService<Merchant_ListView_AdminService>(ServiceHelper.BaseService, "GetMerchant", WalkingTec.Mvvm.Abstraction.HttpMethodEnum.POST, parms);
            return query;
        }
        #endregion
    }
}
