using SchoolHub.Common.Models.Enums;
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
    public class Tennant : IStatusModificado
    {
        public Guid TennantId { get; set; }
        public TennantStatus Status { get; set; }

        [MaxLength(50)]
        [Required]
        public string Nome { get; set; }

        [MaxLength(100)]
        public string? Logo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(150)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(20)]
        public string? Whatsapp { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
        public virtual ICollection<Turma>? Turmas { get; set; }
        public virtual ICollection<Disciplina>? Disciplinas { get; set; } = new List<Disciplina>();


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
