using SchoolHub.Common.Models.Entities.Interfaces;
using SchoolHub.Common.Models.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Entities
{
    public class ClassGroup : IStatusModified
    {
        public Guid ClassGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public Guid TennantId { get; set; }


        public virtual Tennant? Tennant { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

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
