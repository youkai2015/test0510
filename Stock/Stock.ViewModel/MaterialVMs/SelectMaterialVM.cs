using Stock.Model.Material;
using Stock.ViewModel.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class SelectMaterialVM : BaseCRUDVM<Material>
    {
        public SelectMaterialListVM SelectMaterialListVM { get; set; }
        protected override void InitVM()
        {
            Entity.MaterialDetails = DC.Set<MaterialDetail>().Where(x => x.MaterialID == Entity.ID).ToList();
            SelectMaterialListVM = new SelectMaterialListVM();
            SelectMaterialListVM.Searcher = new MaterialSelectSearcher();
            SelectMaterialListVM.CopyContext(this);
            SelectMaterialListVM.Searcher.CopyContext(this);
            SelectMaterialListVM.Searcher.ParentID = Entity.ID;
            SelectMaterialListVM.DoSearch();

        }
        public void AddChildMaterial()
        {
            var oldList = DC.Set<MaterialDetail>().Where(x => x.MaterialID == Entity.ID).ToList();
            foreach (var item in oldList)
            {
                DC.DeleteEntity(item);
            }
            if (Entity.MaterialDetails != null && Entity.MaterialDetails.Count > 0)
            {
                foreach (var item in Entity.MaterialDetails)
                {
                    DC.AddEntity(item);
                }
            }
            DC.SaveChanges();
            RedisVM.ChildMaterial(Entity.MerchantCode,Entity.MaterialCode,this.DC);
        }

    }
}
