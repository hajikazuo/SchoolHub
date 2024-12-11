using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Models.Usuarios
{
    public class Documento : IStatusModificado
    {
        public Guid DocumentoId { get; set; }

        [Display(Name = "Tipo de documento")]
        public TipoDocumento Tipo { get; set; } 

        [MaxLength(50)]
        [Display(Name = "Número do documento")]
        public string Numero { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de emissão")]
        public DateTime? DataEmissao { get; set; }

        public virtual Usuario? Usuario { get; set; }

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
