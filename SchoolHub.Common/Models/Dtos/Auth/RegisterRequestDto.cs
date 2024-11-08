using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Dtos.Auth
{
    public class RegisterRequestDto
    {
        public Guid? TennantId { get; set; }
        public Guid? ClassGroupId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        
    }
}
