using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Mvc.ViewModels;

namespace SchoolHub.Mvc.Extensions
{
    public class DropDownFunc
    {
        public static async Task<List<SelectListaGenericaViewModel>> DisciplinaAsync(AppDbContext context, Guid tennantid)
        {
            return await context.Disciplinas
                .Where(w => w.TennantId == tennantid)
                .OrderBy(o => o.Nome)
                .Select(s => new SelectListaGenericaViewModel
                {
                    Id = s.DisciplinaId,
                    Nome = s.Nome,
                })
                .ToListAsync();
        }
    }
}
