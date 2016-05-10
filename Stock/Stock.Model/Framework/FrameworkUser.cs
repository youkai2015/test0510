using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Abstraction;
using WalkingTec.Mvvm.Core;
using Stock.Resource;

namespace Stock.Model
{
    public enum UserTypeEnum
    {
        [Display(Name = "a")]
        Inside = 0,
        [Display(Name = "b")]
        OutSide = 1
    }

    [Serializable]
    public class FrameworkUser : FrameworkUserBase
    {
        [Unicode(false)]
        [Display(Name = "Password", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false)]
        [StringLength(32)]
        public string Password { get; set; }

        [Unicode(false)]
        [Display(Name = "WorkPhone", ResourceType = typeof(Language))]
        [StringLength(30)]
        public string WorkPhone { get; set; }

        [Unicode(false)]
        [Display(Name = "Cellphone", ResourceType = typeof(Language))]
        [StringLength(30)]
        public string CellPhone { get; set; }

        [Unicode(false)]
        [Display(Name = "HomePhone", ResourceType = typeof(Language))]
        [StringLength(30)]
        public string HomePhone { get; set; }

        [Display(Name = "Fax", ResourceType = typeof(Language))]
        [StringLength(30)]
        [Unicode(false)]
        public string Fax { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Language))]
        [StringLength(200)]
        public string Address { get; set; }

        [Display(Name = "Zip", ResourceType = typeof(Language))]
        [StringLength(6)]
        [Unicode(false)]
        public string ZipCode { get; set; }

        [Display(Name = "EnrollDate", ResourceType = typeof(Language))]
        public DateTime? StartWorkDate { get; set; }

        [Display(Name = "IsValid", ResourceType = typeof(Language))]
        public bool IsValid { get; set; }

        [Display(Name = "Photo", ResourceType = typeof(Language))]
        public Guid? PhotoID { get; set; }

        [Display(Name = "Photo", ResourceType = typeof(Language))]
        public FileAttachment Photo { get; set; }

        [Display(Name = "Department", ResourceType = typeof(Language))]
        public Guid? DepartmentID { get; set; }

        [Display(Name = "Department", ResourceType = typeof(Language))]
        public FrameworkDepartment Department { get; set; }

        public UserTypeEnum? UserType { get; set; }
    }
}
