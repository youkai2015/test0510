using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Model;

namespace Stock.ViewModel.Framework.FrameworkUserVMs
{
    public class FrameworkUserTemplateVM : BaseTemplateVM
    {
        public string Status { get; set; }


        public ExcelPropety ITCode = new ExcelPropety { ColumnName = Utils.GetResourceText("ITCode"), DataType = ColumnDataType.Text };
        public ExcelPropety Name = new ExcelPropety { ColumnName = Utils.GetResourceText("ChineseName"), DataType = ColumnDataType.Text };
        // public ExcelPropety Password = new ExcelPropety { ColumnName = Utils.GetResourceText("Password"), DataType = ColumnDataType.Text };
        public ExcelPropety CellPhone = new ExcelPropety { ColumnName = Utils.GetResourceText("CellPhone"), DataType = ColumnDataType.Text };
        public ExcelPropety Email = new ExcelPropety { ColumnName = Utils.GetResourceText("Email"), DataType = ColumnDataType.Text };
        public ExcelPropety UserType = new ExcelPropety { ColumnName = Utils.GetResourceText("UserType"), DataType = ColumnDataType.Enum, EnumType = typeof(UserTypeEnum) };
        public ExcelPropety IsValid = new ExcelPropety { ColumnName = Utils.GetResourceText("IsValid"), DataType = ColumnDataType.ComboBox, ListItems = Utils.GetBoolCombo(BoolComboTypes.ValidInvalid) };
        public string Password { get; set; }
        public string RowIndex { get; set; }
    }

    public class FrameworkUserImportVM : BaseImportVM<FrameworkUserTemplateVM, FrameworkUser>
    {
        public override bool BatchSaveData()
        {
            SetEntityList();
            foreach (var item in this.EntityList)
            {
                item.Password = "000000";
            }

            return base.BatchSaveData();
        }
    }

}