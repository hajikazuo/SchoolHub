using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Models.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Common.Models.Entities.Users
{
    public class User : IdentityUser<Guid>, IStatusModified
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(14)]
        public string? Cpf { get; set; }

        public Guid? TennantId { get; set; }
        public Guid? ClassGroupId { get; set; }


        public virtual Tennant? Tennant { get; set; }
        public virtual ClassGroup? ClassGroup { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }

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
