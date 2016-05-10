using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace Stock.Model.Material
{
    public class StockLog : BasePoco
    {
        //[Display(Name ="原库存")]
        //public int OldStockNumber { get; set; }
        //[Display(Name ="新库存")]
        //public int NewStockNumber { get; set; }

        [Display(Name = "数量")]
        public int StockNumber { get; set; }

        [Display(Name = "操作类型")]
        public string ActionType { get; set; }

        [Display(Name = "物料编号")]
        public string MaterialCode { get; set; }

        [Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        [Display(Name = "商户编码")]
        public string MerchatCode { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

    }
}
