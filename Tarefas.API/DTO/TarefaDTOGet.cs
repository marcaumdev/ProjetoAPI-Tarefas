using Tarefas.API.Model;

namespace Tarefas.API.DTO
{
    public class TarefaDTOGet
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Detalhe { get; set; }
        public string Concluida { get; set; }
        public string DataCadastro { get; set; }
        public string? DataConclusao { get; set; }
        public Categoria Categoria { get; set; }
    }
}
