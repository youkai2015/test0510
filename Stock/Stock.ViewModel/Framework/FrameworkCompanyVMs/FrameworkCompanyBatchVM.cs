using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.Framework.FrameworkCompanyVMs
{
    public class FrameworkCompanyBatchVM : BaseBatchVM<FrameworkCompany, BaseVM>
    {
        public FrameworkCompanyBatchVM()
        {
            ListVM = new FrameworkCompanyListVM();
        }
    }

}
