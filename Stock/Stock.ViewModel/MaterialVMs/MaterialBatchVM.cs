using Stock.Model.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class MaterialBatchVM : BaseBatchVM<Material, Material_BatchEdit>
    {
        public MaterialBatchVM()
        {
            ListVM = new MaterialListVM();
            LinkedVM = new Material_BatchEdit();
        }
    }
    public class Material_BatchEdit: BaseVM
    {



    }
}
