using Stock.ViewModel;
using Stock.ViewModel.MaterialVMs;
using Stock.ViewModel.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalkingTec.Mvvm.Mvc;

namespace Stock.Controllers.Material
{
    public class ServiceController : BaseController
    {
        // GET: Service
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
    }
}