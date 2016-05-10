using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Resource;

namespace Stock.Model
{
    [Serializable]
    [Workflow(Description = "合同审批流程", ResourceType = typeof(Language))]
    public class ContractApprove : BasePoco
    {
        [WorkflowField]
        public string TitleTitleTitleTitleTitleTitleTitle { get; set; }
        [WorkflowField]
        public string Content { get; set; }
        [WorkflowField]
        public string Approver { get; set; }

        [Required]
        [Display(Name = "Company", ResourceType = typeof(Language))]
        public Guid? CompanyID { get; set; }
        [WorkflowField]
        [Display(Name = "Company", ResourceType = typeof(Language))]
        public FrameworkCompany Company { get; set; }

        [Display(Name = "Department", ResourceType = typeof(Language))]
        public Guid? DepartmentID { get; set; }
        [WorkflowField]
        [Display(Name = "Department", ResourceType = typeof(Language))]
        public FrameworkDepartment Department { get; set; }

    }
}
