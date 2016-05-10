using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class MerchantListVM : BasePagedListVM<Merchant_ListView, MerchantSearcher>
    {
        public string SessionID { get; set; }

        /// <summary>
        /// 页面显示列表
        /// </summary>
        protected override void InitListVM()
        {
            List<IGridColumn<Merchant_ListView>> rv = new List<IGridColumn<Merchant_ListView>>();
            rv.Add(this.MakeGridColumn(x => x.MerchantName).SetWidth(150));
            rv.Add(this.MakeGridColumn(x => x.MerchantCode).SetWidth(300));
            ListColumns = rv;
        }
        /// <summary>
        /// 查询结果
        /// </summary>
        public override IOrderedQueryable<Merchant_ListView> GetSearchQuery()
        {
            var query = StockServerVM.GetMerchant(LoginUserInfo.ITCode, Searcher.CurrentPage.ToString(), Searcher.MerchantNo, Searcher.MerchantName);
            PassSearch = true;
            Searcher.TotalPages = query.TotalPages;
            Searcher.TotalRecords = query.TotalRecords;
            return query.EntityList.AsQueryable().OrderBy(x => x.MerchantCode);
        }
    }
    public class MerchantSearcher : BaseSearcher
    {
        [Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        [Display(Name = "商户编号")]
        public string MerchantNo { get; set; }
    }
}
