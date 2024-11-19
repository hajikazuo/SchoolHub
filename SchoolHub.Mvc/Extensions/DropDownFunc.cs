using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities.Enum;
using SchoolHub.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Mvc.Extensions
{
    public class DropDownFunc
    {
        public static async Task<List<SelectGenericList>> ReturnTennants(AppDbContext context)
        {
            return await context.Tennants
                .Where(t => t.Status == TennantStatus.Active)
                .OrderBy(o => o.Name)
                .Select(s => new SelectGenericList
                {
                    Id = s.TennantId,
                    Name = s.Name,
                })
                .ToListAsync();
        }
    }
}
