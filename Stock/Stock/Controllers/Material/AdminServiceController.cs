using Stock.ViewModel;
using Stock.ViewModel.MaterialVMs;
using Stock.ViewModel.Redis;
using System.Web.Mvc;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Mvc;

namespace Stock.Controllers.Material
{
    [Public]
    [AllRights]
    public class AdminServiceController : BaseController
    {

        /// <summary>
        /// 物料验证
        /// </summary>
        /// <param name="MeMerchantCode">商户编码</param>
        /// <param name="MaterCode">物料编码</param>
        /// <returns></returns>
        public JsonResult TestingMaterial(string MerchantCode, string MaterialCode)
        {

            bool b = MySqlConfigVM.TestingCongfig(MerchantCode);
            if (!b)
            {
                return Json(new TestingMaterialResult { Result = 3, Message = "商户不存在", ErrowList = null }, JsonRequestBehavior.AllowGet);
            }
            var vm = CreateVM<MaterialVM>();
            var result = vm.TestingMaterial(MerchantCode, MaterialCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="MerchatCode">商户编号</param>
        /// <param name="CurrentPage">页数</param>
        /// <returns></returns>
        public JsonResult GetMaterialList(string MerchantCode, string CurrentPage)
        {
            bool b = MySqlConfigVM.TestingCongfig(MerchantCode);
            if (!b)
            {
                return Json(new TestingMaterialHelper { Result = 3, Message = "商户不存在" }, JsonRequestBehavior.AllowGet);
            }
            var vm = CreateVM<MaterialListVM>();
            vm.Searcher.MerchantCode = MerchantCode;
            vm.Searcher.CurrentPage = int.Parse(CurrentPage);
            vm.DoSearch();
            var rv = vm.GetMaterialList();
            return Json(rv, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取库存数量
        /// </summary>
        /// <param name="MerchaantCode"></param>
        /// <returns></returns>
        public int GetStockNumber(string MerchantCode, string MaterialCode)
        {
            bool b = MySqlConfigVM.TestingCongfig(MerchantCode);
            if (!b)
            {
                return 0;
            }
            var query = RedisVM.GetStockNumber(MerchantCode, MaterialCode);
            if (query == null)
            {
                var vm = CreateVM<MaterialVM>();
                return vm.GetStockNumber(MerchantCode, MaterialCode);

            }
            else
            {
                return int.Parse(query?.ToString());
            }

        }



        /// <summary>
        /// 同步库存
        /// </summary>
        /// <returns>1：成功；0：失败</returns>

        public int SynchronousStock(string MerchantCode, string MaterialCode, int GoodsCount, bool OrderSubmitOrCanceled)
        {
            TestingMaterialHelper query = RedisVM.SynchronousStock(MerchantCode, MaterialCode, GoodsCount, OrderSubmitOrCanceled);
            if (query.Result == 2)//Redis库存不存在
            {
                var vm = CreateVM<MaterialVM>();
                var result = vm.SynchronousStockMySql(MerchantCode, MaterialCode, GoodsCount, OrderSubmitOrCanceled);
                return result;
            }
            else if (query.Result == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 第三方库存同步
        /// </summary>
        /// <param name="MerchantCode">商户编码</param>
        /// <param name="MaterialCode">物料编码</param>
        /// <param name="StockNumber">库存数量</param>
        /// <param name="Discountnum">比例</param>
        /// <returns></returns>
        public int SynchronousStockThird(string MerchantCode, string MaterialCode, int StockNumber, decimal Discountnum)
        {
            var vm = CreateVM<MaterialVM>();
            return vm.SynchronousStock(MerchantCode, MaterialCode, StockNumber, Discountnum);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}