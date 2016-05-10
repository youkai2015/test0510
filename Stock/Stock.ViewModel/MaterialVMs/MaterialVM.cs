using Component.Log;
using Stock.Model.Material;
using Stock.ViewModel.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class MaterialVM : BaseCRUDVM<Material>
    {


        public override void DoAdd()
        {
            //获取商户信息
            //var query = StockServerVM.GetMerchant("", "1", Entity.MerchantCode, "");
            //if (query != null)
            //{
            //    Entity.MerchantID = query.EntityList[0].ID.ToString();
            //    Entity.MerchantCode = query.EntityList[0].MerchantCode;
            //}

            base.DoAdd();
            RedisVM.AddStock(Entity.MerchantCode, this.DC);
        }


        public override void DoEdit(bool updateAllFields = false)
        {
            //  DC = CreateDC(Entity.MerchantCode);
            StockLog s = new StockLog();

            base.DoEdit(updateAllFields);
            RedisVM.AddStock(Entity.MerchantCode, this.DC);
        }
        public override void DoDelete()
        {
            // DC = CreateDC(Entity.MerchantCode);
            base.DoDelete();
            RedisVM.RemoveStock(Entity.MerchantCode, Entity.MaterialCode);
        }

        #region 检测物料是否存在
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MerchantCode">商户编码</param>
        /// <param name="MaterialCode">物料编码</param>
        /// <returns></returns>
        public TestingMaterialResult TestingMaterial(string MerchantCode, string MaterialCode)
        {
            DC = CreateDC(MerchantCode);

            List<string> list = new List<string>();
            string[] MaterialCodeList = MaterialCode.Split(',');
            for (int i = 0; i < MaterialCodeList.Length; i++)
            {
                string strMaterialCode = MaterialCodeList[i].ToString();
                var query = DC.Set<Material>().Where(x => x.MaterialCode == strMaterialCode).FirstOrDefault();

                if (query == null)
                {
                    list.Add(strMaterialCode);
                }
            }
            if (list.Count > 0)
            {
                return new TestingMaterialResult { Result = 0, Message = "物料不存在", ErrowList = string.Join(",", list) };
            }
            else
            {
                return new TestingMaterialResult { Result = 1, Message = "成功", ErrowList = null };
            }
        }
        #endregion

        #region 获取库存
        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="MerchantCode">商户编码</param>
        /// <param name="MaterialCode">物料编码</param>
        /// <returns></returns>
        public int GetStockNumber(string MerchantCode, string MaterialCode)
        {
            DC = CreateDC(MerchantCode);
            var query = DC.Set<Material>().Where(x => x.MaterialCode == MaterialCode).FirstOrDefault();
            if (query == null)
            {
                return 0;
            }
            else
            {
                RedisVM.AddStock(query.MerchantCode, query.MaterialCode, int.Parse(query.StockNumber?.ToString()));
                return int.Parse(query.StockNumber?.ToString());
            }

        }
        #endregion

        #region 库存同步
        /// <summary>
        /// 同步库存
        /// </summary>
        /// <param name="MerchantCode">商户编码</param>
        /// <param name="MaterialCode">物理编码</param>
        /// <param name="GoodsCount">数量</param>
        /// <param name="OrderSubmitOrCanceled">true:提交(减库存加占用),false:取消(减少占用还库存)</param>
        /// <returns>1:成功；0：失败</returns>
        public int SynchronousStockMySql(string MerchantCode, string MaterialCode, int GoodsCount, bool OrderSubmitOrCanceled)
        {
            DC = CreateDC(MerchantCode);
            var query = DC.Set<Material>().Where(x => x.MaterialCode == MaterialCode && x.MerchantCode == MerchantCode).FirstOrDefault();
            if (OrderSubmitOrCanceled)
            {
                if (query.StockNumber < GoodsCount)
                {
                    CommonLog.Error(LogNameEnum.Stock, $"{query.MaterialCode}:库存不足");
                    return 0;
                }
                else
                {
                    query.StockNumber = query.StockNumber - GoodsCount;
                    query.OccupyNumber = query.OccupyNumber + GoodsCount;
                    DC.UpdateProperty(query, "StockNumber");
                    DC.UpdateProperty(query, "OccupyNumber");
                    //DC.UpdateEntity(query);
                    DC.SaveChanges();
                    RedisVM.AddStock(MerchantCode, MaterialCode, int.Parse(query.StockNumber.ToString()), query.OccupyNumber);
                    return 1;
                }
            }
            else
            {
                //减占用，还库存
                query.StockNumber = query.StockNumber + GoodsCount;
                query.OccupyNumber = query.OccupyNumber - GoodsCount;
                DC.UpdateEntity(query);
                DC.SaveChanges();
                //return new TestingMaterialHelper { Result = 1, Message = "成功" };
                RedisVM.AddStock(MerchantCode, MaterialCode, int.Parse(query.StockNumber.ToString()), query.OccupyNumber);
                return 1;
            }
        }
        #endregion

        #region 第三方库存同步
        /// <summary>
        /// 第三方库存同步
        /// </summary>
        /// <param name="MerchantCode">商户编码</param>
        /// <param name="MaterialCode">物料编码</param>
        /// <param name="StockNumber">库存数量</param>
        /// <param name="Discountnum">库存比例</param>
        /// <returns></returns>
        public int SynchronousStock(string MerchantCode, string MaterialCode, int StockNumber, decimal Discountnum)
        {
            DC = CreateDC(MerchantCode);
            var query = DC.Set<Material>().Where(x => x.MerchantCode == MerchantCode & x.MaterialCode == MaterialCode).FirstOrDefault();
            //不存在
            if (query == null)
            {
                CommonLog.Info(LogNameEnum.Stock, $"{query.MaterialCode}:物料不存在");
                return 0;
            }
            else//存在
            {
                query.StockNumber = (int)(StockNumber * Discountnum);
                DC.UpdateEntity(query);
                base.DoEdit();
                RedisVM.AddStock(MerchantCode, MaterialCode, StockNumber, Discountnum);
                return 1;
            }
        }
        #endregion
    }
    //检测物料返回信息帮助类
    public class TestingMaterialHelper
    {
        public int Result { get; set; }

        public string Message { get; set; }
    }


    public class TestingMaterialResult
    {
        public int Result { get; set; }

        public string Message { get; set; }


        public string ErrowList { get; set; }

    }

}
