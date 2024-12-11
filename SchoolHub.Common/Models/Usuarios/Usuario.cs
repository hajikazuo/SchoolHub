using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Usuarios
{
    public class Usuario : IdentityUser<Guid>, IStatusModificado
    {
        [MaxLength(100)]
        [Required]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; }

        [MaxLength(20)]
        public string? Celular { get; set; }

        [MaxLength(100)]
        public string? Imagem { get; set; }

        public Guid? TennantId { get; set; }
        public Guid? TurmaId { get; set; }


        public virtual Tennant? Tennant { get; set; }
        public virtual Turma? Turma { get; set; }
        
        public virtual ICollection<Presenca>? Presencas { get; set; }

        public List<Documento> Documentos { get; set; } = new List<Documento>();

        public void AddDocumento(Documento documento)
        {
            Documentos.Add(documento);
        }

        #region Interface
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DataModificado { get; set; }

        #endregion
    }
}
