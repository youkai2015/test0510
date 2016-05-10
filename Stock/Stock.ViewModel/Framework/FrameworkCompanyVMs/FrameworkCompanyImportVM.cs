using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.Framework.FrameworkCompanyVMs
{
    public class FrameworkCompanyTemplateVM : BaseTemplateVM
    {
        public ExcelPropety CompanyCode = new ExcelPropety { ColumnName = Utils.GetResourceText("CompanyCode"), DataType = ColumnDataType.Text };
        public ExcelPropety CompanyName = new ExcelPropety { ColumnName = Utils.GetResourceText("CompanyName"), DataType = ColumnDataType.Text, SubTableType = typeof(FrameworkCompanyMLContent) };
    }

    public class FrameworkCompanyImportVM : BaseImportVM<FrameworkCompanyTemplateVM, FrameworkCompany>
    {
        public override DuplicatedInfo<FrameworkCompany> SetDuplicatedCheck()
        {
            var rv = this.CreateFieldsInfo(SimpleField(x => x.CompanyCode));
            return rv;
        }
    }

}