using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace Stock.Model.Material
{
    public class Material: BasePoco
    {
        [Display(Name = "商品名称")]
        public string GoodsName { get; set; }
        [Display(Name = "物料编号")]
        public string MaterialCode { get; set; }
        [Display(Name = "规格")]
        public string Specification { get; set; }
        [Display(Name = "商户")]
        public string MerchantID { get; set; }

        [Display(Name = "商户编号")]
        public string MerchantCode { get; set; }

        [Display(Name = "商户名称")]
        [StringLength(200)]
        public string MerchantName { get; set; }

        [Display(Name = "库存")]
        public int? StockNumber { get; set; }


        [Display(Name ="占用")]
        public int OccupyNumber { get; set; }
        public List<MaterialDetail> MaterialDetails { get; set; }

        //public Guid? PicID { get; set; }

        //public FileAttachment Pic { get; set; }

    }
}
