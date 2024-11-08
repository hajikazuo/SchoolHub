using SchoolHub.Common.Models.Entities.Enum;
using SchoolHub.Common.Models.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Entities
{
    public class Attendance
    {
        public Guid AttendanceId { get; set; }

        [Required]
        public Guid ClassGroupId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public AttendanceStatus Status { get; set; }

        [MaxLength(200)]
        public string? Notes { get; set; }

        public virtual User User { get; set; }
        public virtual ClassGroup ClassGroup { get; set; }
    }
}
