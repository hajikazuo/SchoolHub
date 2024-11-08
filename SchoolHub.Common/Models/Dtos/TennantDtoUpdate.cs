using SchoolHub.Common.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Dtos
{
    public class TennantDtoUpdate
    {
        public string Name { get; set; }
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public TennantStatus Status { get; set; }
    }
}
