using SchoolHub.Common.Models.Interfaces;
using SchoolHub.Common.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models
{
    public class Turma : IStatusModificado
    {
        public Guid TurmaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }
        public Guid TennantId { get; set; }

        [Display(Name = "Escola")]
        public virtual Tennant? Tennant { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
        public virtual ICollection<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();

        #region Interface
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de cadastro")]
        public DateTime DataCadastro { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Data últ. modificação")]
        public DateTime? DataModificado { get; set; }

        #endregion
    }
}
