using Microsoft.EntityFrameworkCore;
using Tarefas.API.Model;

namespace Tarefas.API.Context
{
    public class TarefasDbContext(DbContextOptions<TarefasDbContext> options) : DbContext(options)
    {
        // Método Construtor comum
        //public TarefasDbContext(DbContextOptions<TarefasDbContext> options) : base(options)
        //{
        //}

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
