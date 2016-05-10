using System.Data.Entity;
using Stock.Model;
using Stock.Model.Material;

namespace Stock.DataAccess
{
    public partial class DataContext
    {
        public DbSet<FrameworkUser> FrameworkUsers { get; set; }
        public DbSet<FrameworkDepartment> FrameworkDepartments { get; set; }
        public DbSet<FrameworkCompany> FrameworkCompanies { get; set; }
        public DbSet<FrameworkFactory> FrameworkFactory { get; set; }
        public DbSet<ContractApprove> ContractApproves { get; set; }
        public DbSet<Material> Material { get; set; }

        public DbSet<MaterialDetail> MaterialDetail { get; set; }
        //public DbSet<StockLog> StockLog { get; set; }
    }
}
