using SchoolHub.Common.Models.Entities.Enum;
using SchoolHub.Common.Models.Entities.Interfaces;
using SchoolHub.Common.Models.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Common.Models.Entities
{
    public class Tennant : IStatusModified
    {
        public Guid TennantId { get; set; }
        public TennantStatus Status { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Logo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(150)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? Telephone { get; set; }

        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<ClassGroup>? ClassGroups { get; set; }

        #region Interface
        [ScaffoldColumn(false)]
        public bool Deleted { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateDeleted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DateRegistration { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? LastModificationDate { get; set; }

        #endregion
    }
}
