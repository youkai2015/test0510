using Component.Service;
using Stock.Model.Material;
using Stock.ViewModel.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;

namespace Stock.ViewModel.MaterialVMs
{
    public class MaterialTemplateVM : BaseTemplateVM
    {
        public List<ComboSelectListItem> AllMerchant = new List<ComboSelectListItem>();
        public ExcelPropety GoodsName = new ExcelPropety { ColumnName = "商户名称", DataType = ColumnDataType.Text, CharCount = 30 };
        public ExcelPropety MaterialCode = new ExcelPropety { ColumnName = "物料编码", DataType = ColumnDataType.Text, CharCount = 30 };
        public ExcelPropety Specification = new ExcelPropety { ColumnName = "规格", DataType = ColumnDataType.Text, CharCount = 30 };
       // public ExcelPropety StockNumber = new ExcelPropety { ColumnName = "库存", DataType = ColumnDataType.Number, CharCount = 15 };
        public ExcelPropety MerchantCode = new ExcelPropety { ColumnName = "商户", DataType = ColumnDataType.ComboBox, CharCount = 30 };
        public ExcelPropety IncreaseNumber = new ExcelPropety { ColumnName = "新增库存", DataType = ColumnDataType.Number, CharCount = 15 };
        public ExcelPropety ReduceNumber = new ExcelPropety { ColumnName = "减少库存", DataType = ColumnDataType.Number, CharCount = 15 };
        //public ExcelPropety CompanyName = new ExcelPropety { ColumnName = Utils.GetResourceText("CompanyName"), DataType = ColumnDataType.Text, SubTableType = typeof(FrameworkCompanyMLContent) };
        public override void InitExcelData()
        {
            Merchant_ListView_AdminService query = NewMethod(null, "1", null, null);
            foreach (var item in query.EntityList)
            {
                AllMerchant.Add(new ComboSelectListItem { Text = item.MerchantName, Value = item.MerchantCode });
            }
            MerchantCode.ListItems = AllMerchant;
        }

        private static Merchant_ListView_AdminService NewMethod(string sessionID, string CurrentPage, string MerchatCode, string MerchatName)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("SessionID", "2m4cysux52wn0hk52v3jz10n");
            parms.Add("CurrentPage", CurrentPage);
            parms.Add("MerchantCode", MerchatCode);
            parms.Add("MerchantName", MerchatName);
            var query = ServiceHelper.CallService<Merchant_ListView_AdminService>(ServiceHelper.BaseService, "GetMerchant", WalkingTec.Mvvm.Abstraction.HttpMethodEnum.POST, parms);
            return query;
        }
    }
    public class MaterialImportVM : BaseImportVM<MaterialTemplateVM, Material>
    {
        private static Merchant_ListView_AdminService NewMethod(string sessionID, string CurrentPage, string MerchatCode, string MerchatName)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("SessionID", "2m4cysux52wn0hk52v3jz10n");
            parms.Add("CurrentPage", CurrentPage);
            parms.Add("MerchantCode", MerchatCode);
            parms.Add("MerchantName", MerchatName);
            var query = ServiceHelper.CallService<Merchant_ListView_AdminService>(ServiceHelper.BaseService, "GetMerchant", WalkingTec.Mvvm.Abstraction.HttpMethodEnum.POST, parms);
            return query;
        }
        public override bool BatchSaveData()
        {
            //如果没有赋值，则调用赋值函数
            if (isEntityListSet == false)
            {
                SetEntityList();
            }
            //如果在复制过程中已经有错误，则直接输出错误
            if (ErrorListVM.ErrorList.Count > 0)
            {
                return false;
            }
            
            foreach (var item in EntityList)
            {
                var entity = DC.Set<Material>().Where(x =>
                    x.MaterialCode == item.MaterialCode && x.MerchantCode == item.MerchantCode).FirstOrDefault();
                if (entity != null)
                {
                    //StockLog osl = new StockLog();
                    //osl.OldStockNumber = int.Parse(entity.StockNumber.ToString());
                    //osl.NewStockNumber = int.Parse(item.StockNumber.ToString());
                    //osl.PlatformStockID = entity.ID;
                    //osl.MaterialCode = entity.MaterialCode;
                    //osl.Platform = entity.Platform;
                    //osl.Factory = entity.Factory;
                    //osl.Warehouse = entity.Warehouse;
                    //osl.SalesOrganization = entity.SalesOrganization;
                    //osl.NewStockNumber = entity.StockNumber + Convert.ToInt32(item.IncreaseNumber) - Convert.ToInt32(item.ReduceNumber);
                    //osl.FreezeNumber = entity.FreezeNumber;
                    //osl.IncreaseNumber = item.IncreaseNumber;
                    //osl.ReduceNumber = item.ReduceNumber;
                    //osl.WarningNumber = item.WarningNumber;
                    //osl.CreateBy = LoginUserInfo.ITCode;
                    //osl.CreateTime = System.DateTime.Now;
                    //osl.UpdateBy = LoginUserInfo.ITCode;
                    //osl.UpdateTime = System.DateTime.Now;
                    DC = CreateDC(entity.MerchantCode);
                    entity.StockNumber = entity.StockNumber+item.StockNumber;
                    entity.UpdateBy = LoginUserInfo.ITCode;
                    entity.UpdateTime = DateTime.Now;
                    DC.UpdateEntity(entity);
                    DC.SaveChanges();
                    RedisVM.AddStock(entity.MerchantCode,this.DC);

                }
                else
                {
                    var query = NewMethod(null, "1", item.MerchantCode, null);
                    DC = CreateDC(item.MerchantCode);
                    if (query != null)
                    {
                        item.MerchantID = query.EntityList[0].ID.ToString();
                        item.MerchantName = query.EntityList[0].MerchantName;
                    }
                    DC.AddEntity(item);
                    DC.SaveChanges();
                    //PlatformStockLog osl = new PlatformStockLog();
                    //osl.OldStockNumber = 0;
                    //osl.GoodsMaterialID = item.GoodsMaterialID;
                    //osl.PlatformStockID = item.ID;
                    //osl.MaterialCode = item.MaterialCode;
                    //osl.Platform = item.Platform;
                    //osl.Factory = item.Factory;
                    //osl.Warehouse = item.Warehouse;
                    //osl.SalesOrganization = item.SalesOrganization;
                    //osl.NewStockNumber = item.StockNumber;
                    //osl.FreezeNumber = item.FreezeNumber;
                    //osl.IncreaseNumber = 0;
                    //osl.ReduceNumber = 0;
                    //osl.WarningNumber = item.WarningNumber;
                    //osl.CreateBy = LoginUserInfo.ITCode;
                    //osl.CreateTime = System.DateTime.Now;
                    //osl.UpdateBy = LoginUserInfo.ITCode;
                    //osl.UpdateTime = System.DateTime.Now;
                    //osl.PlatformCode = item.PlatformCode;
                    //osl.SalesType = item.SalesType;
                    //osl.IsGift = item.IsGift;
                    //DC.AddEntity(osl);
                    RedisVM.AddStock(item.MerchantCode,this.DC);
                }
            }
            return true;
        }
        public override void SetEntityList()
        {
            EntityList = new List<Material>();
            foreach (var item in TemplateData)
            {
                Material entity = new Material();
                entity.CreateBy = LoginUserInfo?.ITCode;
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = DateTime.Now;
                entity.GoodsName = item.GoodsName?.Value.ToString();
                entity.MaterialCode = item.MaterialCode == null ? "" : item.MaterialCode.Value.ToString();
                entity.Specification = item.Specification == null ? "" : item.Specification.Value.ToString();
                entity.MerchantCode = item.MerchantCode == null ? "" : item.MerchantCode.Value.ToString();
                int IncreaseNumber = item.IncreaseNumber == null ? 0 : int.Parse(item.IncreaseNumber.Value.ToString());
                int ReduceNumber = item.ReduceNumber == null ? 0 : int.Parse(item.ReduceNumber.Value.ToString());
                entity.StockNumber = IncreaseNumber - ReduceNumber;
                EntityList.Add(entity);
            }
          
            isEntityListSet = true;
            // base.SetEntityList();
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TemplateData.Count();

            if (ErrorListVM.ErrorList.Count < 1)
            {
                if (!isEntityListSet)
                {
                    SetEntityList();
                }
                foreach (var item in EntityList)
                {


                    if (string.IsNullOrEmpty(item.MaterialCode))
                    {
                        ErrorListVM.ErrorList.Add(new ErrorMessage { Message = "[物料编号]为必填项", Index = item.ExcelIndex });
                    }
                    if (string.IsNullOrEmpty(item.MerchantCode))
                    {
                        ErrorListVM.ErrorList.Add(new ErrorMessage { Message = "[商户]为必填项", Index = item.ExcelIndex });
                    }
                    DC = CreateDC(item.MerchantCode);
                    int? entity = DC.Set<Material>().Where(x =>
                    x.MaterialCode == item.MaterialCode && x.MerchantCode == item.MerchantCode).Select(x=>x.StockNumber).FirstOrDefault();
                    if (entity!=null)
                    {
                        int stocknumber = Convert.ToInt32(entity.ToString()) + int.Parse(item.StockNumber?.ToString());
                        if (stocknumber<0)
                        {
                            ErrorListVM.ErrorList.Add(new ErrorMessage { Message = "库存不能为负数", Index = item.ExcelIndex });
                        }
                    }
                    else
                    {
                        if (item.StockNumber < 0)
                        {
                            ErrorListVM.ErrorList.Add(new ErrorMessage { Message = "库存不能为负数", Index = item.ExcelIndex });
                        }
                    }
                    
                }
            }

            return base.Validate(validationContext);

        }

    }
}
