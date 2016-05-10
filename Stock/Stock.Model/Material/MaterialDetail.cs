using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace Stock.Model.Material
{
    public class MaterialDetail: BasePoco
    {
        [Display(Name = "物料")]
        public Guid? MaterialID { get; set; }
        public Material Material { get; set; }

        [Display(Name = "子物料")]
        public Guid? ChildMaterialID { get; set; }

        [Display(Name = "子物料编号")]
        [StringLength(500)]
        public string ChildMaterialCode { get; set; }

        [Display(Name = "物料规格")]
        [StringLength(200)]
        public string Specification { get; set; }
    }
}
