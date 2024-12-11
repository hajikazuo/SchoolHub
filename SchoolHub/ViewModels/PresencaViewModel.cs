using SchoolHub.Common.Models.Enums;

namespace SchoolHub.Mvc.ViewModels
{
    public class PresencaViewModel
    {
        public DateTime DataAula { get; set; }
        public List<PresencaAlunoViewModel> Alunos { get; set; } = new List<PresencaAlunoViewModel>();
    }

    public class PresencaAlunoViewModel
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public PresencaStatus Status { get; set; }
        public string Observacoes { get; set; }
    }
}
