using Component.Redis;
using Component.Model.RedisEntity;
using Stock.Model.Material;
using Stock.ViewModel.Helper;
using Stock.ViewModel.MaterialVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.Redis
{
    public class RedisVM : BaseVM
    {

        //conntionString = System.Configuration.ConfigurationManager.AppSettings[configKey];
        public static string StockRedisRegion = "GoodsSaleStockTest";
        public static string StockChildRedisRegion = "MaterialRelationTest";

        /// <summary>
        /// 增加修改库存信息
        /// </summary>
        /// <param name="MateMerchantCode">商户编码</param>
        /// <param name="dc"></param>
        public static void AddStock(string MerchantCode, IDataContext dc)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            var queryList = dc.Set<Material>().Where(x => x.MerchantCode == MerchantCode).ToList();
            foreach (var item in queryList)
            {
                StockRedis stockNumber = RedisHelper.GetRedis<StockRedis>(conntionString, StockRedisRegion, item.MaterialCode + ";" + item.MerchantCode);
                if (stockNumber != null)
                {
                    stockNumber.StockNumber = int.Parse(item.StockNumber.ToString());
                    RedisHelper.AddRedis<StockRedis>(conntionString, StockRedisRegion, stockNumber.MaterialCode + ";" + item.MerchantCode, stockNumber);
                }
                else
                {
                    StockRedis s = new StockRedis();
                    s.MaterialCode = item.MaterialCode;
                    s.OccupyNumber = 0;
                    s.StockNumber = int.Parse(item.StockNumber.ToString());
                    RedisHelper.AddRedis<StockRedis>(conntionString, StockRedisRegion, item.MaterialCode + ";" + item.MerchantCode, s);
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="MateMerchantCode"></param>
        /// <param name="MaterialCode"></param>
        public static void RemoveStock(string MerchantCode, string MaterialCode)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            RedisHelper.Remove(conntionString, StockRedisRegion, MaterialCode + ";" + MerchantCode);
        }

        /// <summary>
        /// 添加子物料关系
        /// </summary>
        /// <param name="MateMerchantCode"></param>
        /// <param name="MaterialCode"></param>
        /// <param name="dc"></param>
        public static void ChildMaterial(string MerchantCode, string MaterialCode, IDataContext dc)
        {
            var query = (from m in dc.Set<Material>()
                         join mc in dc.Set<MaterialDetail>()
                         on m.ID equals mc.MaterialID
                         where m.MaterialCode == MaterialCode
                         select new
                         {
                             m.MerchantCode,
                             mc.ChildMaterialCode
                         }
                       ).ToList();
            if (query != null)
            {
                string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
                MaterialChildRedis ma = new MaterialChildRedis();
                ma.MaterialCode = MaterialCode;
                List<string> ChildMaterialCode = new List<string>();
                foreach (var item in query)
                {
                    ChildMaterialCode.Add(item.ChildMaterialCode);
                }
                RedisHelper.AddRedis<MaterialChildRedis>(conntionString, StockChildRedisRegion, MaterialCode + ";" + MerchantCode, ma);
            }
        }

        public static int StockGet(List<string> materialCodelist, string MerchantCode)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            List<int> stocklist = new List<int>();
            if (materialCodelist != null)
            {
                foreach (var item in materialCodelist)
                {
                    StockRedis stock = RedisHelper.GetRedis<StockRedis>(StockChildRedisRegion, item);
                    if (stock != null)
                    {
                        stocklist.Add(stock.StockNumber);
                    }
                }
            }

            if (stocklist.Count() > 0)
            {
                return stocklist.Min();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        ///获取库存数量
        /// </summary>
        /// <param name="MateMerchantCode"></param>
        /// <param name="MaterialCode"></param>
        /// <returns></returns>
        public static int? GetStockNumber(string MerchantCode, string MaterialCode)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            var ChildMaterialCode = RedisHelper.GetRedis<MaterialChildRedis>(StockChildRedisRegion, MaterialCode + ";" + MerchantCode);
            if (ChildMaterialCode != null)
            {
                return StockGet(ChildMaterialCode.ChildMaterialList, MerchantCode);
            }
            else
            {
                StockRedis stockNumber = RedisHelper.GetRedis<StockRedis>(conntionString, StockRedisRegion, MaterialCode + ";" + MerchantCode);
                if (stockNumber == null)
                {
                    return null;
                }
                else
                {
                    return stockNumber.StockNumber;
                }
            }

        }

        /// <summary>
        /// 单条增加库存
        /// </summary>
        /// <param name="MerchantCode"></param>
        /// <param name="MaterialCode"></param>
        /// <param name="StockNumber"></param>
        public static void AddStock(string MerchantCode, string MaterialCode, int StockNumber)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            bool b = RedisHelper.AddRedis<StockRedis>(conntionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, new StockRedis { MaterialCode = MaterialCode, OccupyNumber = 0, StockNumber = StockNumber });
            if (b)
            {
                StockLog stocklog = new StockLog()
                {
                    ID = Guid.NewGuid(),
                    ActionType = "增加",
                    MaterialCode = MaterialCode,
                    MerchatCode = MerchantCode,
                    StockNumber = StockNumber,
                    Remark = "新增库存",
                    CreateTime = DateTime.Now,
                };
            }

        }
        public static void AddStock(string MerchantCode, string MaterialCode, int StockNumber, int OccupyNumber)
        {
            string conntionString = RedisConfigHelper.GetConnection(MerchantCode);
            RedisHelper.AddRedis<StockRedis>(conntionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, new StockRedis { MaterialCode = MaterialCode, OccupyNumber = OccupyNumber, StockNumber = StockNumber });

        }
        public static void AddStock(string MerchantCode, string MaterialCode, int StockNumber, decimal Discountnum)
        {
            string connectionString = RedisConfigHelper.GetConnection(MerchantCode);
            var query = RedisHelper.GetRedis<StockRedis>(connectionString, StockRedisRegion, MerchantCode + ";" + MaterialCode);

            if (query != null)
            {
                query.StockNumber = (int)(StockNumber * Discountnum);
                RedisHelper.AddRedis<StockRedis>(connectionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, query);
            }
            else
            {
                RedisHelper.AddRedis<StockRedis>(connectionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, new StockRedis { MaterialCode = MaterialCode, OccupyNumber = 0, StockNumber = (int)(StockNumber * Discountnum) });
            }

        }

        /// <summary>
        /// 库存扣减
        /// </summary>
        /// <param name="MerchantCode">商户编号</param>
        /// <param name="MaterialCode">物料编号</param>
        /// <param name="GoodsCount">数量</param>
        /// <param name="OrderSubmitOrCanceled">true:提交订单，false:取消订单</param>
        public static TestingMaterialHelper SynchronousStock(string MerchantCode, string MaterialCode, int GoodsCount, bool OrderSubmitOrCanceled)
        {
            string connectionString = RedisConfigHelper.GetConnection(MerchantCode);
            StockRedis query = RedisHelper.GetRedis<StockRedis>(connectionString, StockRedisRegion, MaterialCode + ";" + MerchantCode);
            if (OrderSubmitOrCanceled)//提交订单
            {
                if (query != null)
                {
                    if (query.StockNumber < GoodsCount)
                    {
                        return new TestingMaterialHelper { Result = 0, Message = "库存不足" };
                    }
                    else
                    {
                        query.OccupyNumber = query.OccupyNumber + GoodsCount;
                        query.StockNumber = query.StockNumber - GoodsCount;
                        RedisHelper.AddRedis<StockRedis>(connectionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, query);
                        return new TestingMaterialHelper { Result = 1, Message = "成功" };
                    }
                }
                else
                {
                    return new TestingMaterialHelper { Result = 2, Message = "库存不存在" };
                }
            }
            else//取消订单
            {
                query.OccupyNumber = query.OccupyNumber - GoodsCount;
                query.StockNumber = query.StockNumber + GoodsCount;
                RedisHelper.AddRedis<StockRedis>(connectionString, StockRedisRegion, MaterialCode + ";" + MerchantCode, query);
                return new TestingMaterialHelper { Result = 1, Message = "成功" };
            }


        }
    }
}
