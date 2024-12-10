﻿using SchoolHub.Common.Models.Enums;
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
    public class Presenca : IStatusModificado
    {
        public Guid PresencaId { get; set; }

        [Required]
        public Guid TurmaId { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public DateTime DataAula { get; set; }

        public PresencaStatus Status { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }

        public virtual Usuario? Usuario { get; set; }
        public virtual Turma? Turma { get; set; }

        #region Interface
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DataModificado { get; set; }

        #endregion
    }
}
