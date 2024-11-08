using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Models.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(User user, IList<string> roles);
    }
}
