using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class MaterialSearcher : BaseSearcher
    {
        [Display(Name = "商户")]
        public string MerchantCode { get; set; }
        public List<ComboSelectListItem> AllMerchant = new List<ComboSelectListItem>();

        [Display(Name = "物料编号")]
        public string MaterialCode { get; set; }
        [Display(Name = "商品名称")]
        public string GoodsName { get; set; }
        protected override void InitVM()
        {
            //获取商户信息
            var query = StockServerVM.GetMerchant(LoginUserInfo.ITCode, "1", "", "");
            foreach (var item in query.EntityList)
            {
                AllMerchant.Add(new ComboSelectListItem { Text = item.MerchantName, Value = item.MerchantCode });
            }
        }

    }
    #region 商户信息列表


    public class Merchant_ListView : BasePoco
    {
        [Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        [Display(Name = "商户编号")]
        public string MerchantCode { get; set; }
    }

    public class Merchant_ListView_AdminService
    {
        public List<Merchant_ListView> EntityList { get; set; }
        public int TotalPages { get; set; }
        public long TotalRecords { get; set; }
    }
    #endregion
}
