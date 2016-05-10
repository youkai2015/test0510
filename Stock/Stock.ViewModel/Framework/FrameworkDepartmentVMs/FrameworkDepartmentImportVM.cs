using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.Framework.FrameworkDepartmentVMs
{
    public class FrameworkDepartmentTemplateVM : BaseTemplateVM
    {
        public ExcelPropety DepartmentCode = new ExcelPropety { ColumnName = Utils.GetResourceText("DepCode"), DataType = ColumnDataType.Text };
        public ExcelPropety DepartmentName = new ExcelPropety { ColumnName = Utils.GetResourceText("DepName"), DataType = ColumnDataType.Text, SubTableType = typeof(FrameworkDepartmentMLContent) };
        public ExcelPropety CompanyID = new ExcelPropety { ColumnName = "公司", DataType = ColumnDataType.ComboBox };

        public override void InitExcelData()
        {
            //CompanyID.ListItems = DC.Set<FrameworkCompany>().GetSelectListItemsForML(null, x => x, y => y.CompanyName, LoginUserInfo.DataPrivileges);
        }

    }


    public class FrameworkDepartmentImportVM : BaseImportVM<FrameworkDepartmentTemplateVM, FrameworkDepartment>
    {
        public override DuplicatedInfo<FrameworkDepartment> SetDuplicatedCheck()
        {
            var rv = this.CreateFieldsInfo(SimpleField(x => x.DepartmentCode));
            return rv;
        }
    }

}