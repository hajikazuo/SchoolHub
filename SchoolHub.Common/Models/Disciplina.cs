using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models
{
    public class Disciplina
    {
        public Guid DisciplinaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        public Guid TennantId { get; set; }
        public virtual Tennant? Tennant { get; set; }
        public virtual ICollection<Turma> Turmas { get; set; } = new List<Turma>();
    }

}
