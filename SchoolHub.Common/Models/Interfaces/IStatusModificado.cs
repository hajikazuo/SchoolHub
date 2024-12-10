using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Interfaces
{
    public interface IStatusModificado
    {
        DateTime DataCadastro { get; set; }

        DateTime? DataModificado { get; set; }
    }
}
