using System;
using WalkingTec.Mvvm.Core;

namespace Stock.Model
{
    public class CompanyPhoto : BasePoco
    {
        public Guid? PhotoID { get; set; }
        public FileAttachment Photo { get; set; }

        public string Remark { get; set; }

        public Guid CompanyID { get; set; }

        public FrameworkCompany Company { get; set; }
    }
}
