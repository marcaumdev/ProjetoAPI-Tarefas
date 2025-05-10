using Microsoft.EntityFrameworkCore;
using Tarefas.API.Context;
using Tarefas.API.DTO;
using Tarefas.API.Model;

namespace Tarefas.API.Endpoints
{
    public static class TarefasEndPoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/Tarefas", async (TarefasDbContext db) =>
            {
                var Tarefas = await db.Tarefas.ToListAsync();

                if(Tarefas.Count > 0)
                {
                    List<TarefaDTOGetAll> TarefasDTO = new List<TarefaDTOGetAll>();

                    foreach (var Tarefa in Tarefas)
                    {
                        var tarefaDTO = new TarefaDTOGetAll()
                        {
                            Id = Tarefa.Id,
                            Nome = Tarefa.Nome,
                            Concluida = Tarefa.Concluida ? "Sim" : "Não"
                        };

                        TarefasDTO.Add(tarefaDTO);
                    }

                    return Results.Ok(TarefasDTO);
                }
                else
                {
                    return Results.NotFound("Não Há Tarefas");
                }
            });

            app.MapGet("/Tarefas/{id}", async (Guid id, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.FindAsync(id);

                if(tarefa != null)
                {
                    var categoria = await db.Categorias.FindAsync(tarefa.CategoriaId);

                    TarefaDTOGet tarefaGet = new TarefaDTOGet()
                    {
                        Id = tarefa.Id,
                        Nome = tarefa.Nome,
                        Detalhe = tarefa.Detalhes,
                        Concluida = tarefa.Concluida ? "Sim" : "Não",
                        DataCadastro = tarefa.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        DataConclusao = tarefa.DataConclusao?.ToString("dd/MM/yyyy HH:mm:ss") ?? "A concluir",
                        Categoria = categoria
                    };

                    return Results.Ok(tarefaGet);
                }
                else
                {
                    return Results.NotFound("Tarefa não encontrada!");
                }

                
            });

            app.MapPost("/Tarefas", async (TarefaDTOPost tarefaDTO, TarefasDbContext db) =>
            {
                if (tarefaDTO == null) return Results.BadRequest("Requisição inválida!");

                Tarefa tarefa = new Tarefa()
                {
                    Nome = tarefaDTO.Nome,
                    Detalhes = tarefaDTO.Detalhes,
                    CategoriaId = tarefaDTO.CategoriaId,
                };

                db.Tarefas.Add(tarefa);

                await db.SaveChangesAsync();

                return Results.Created($"/Tarefa/{tarefa.Id}", tarefa);
            });

            app.MapPut("/Tarefas/{id}", async (Guid id, Tarefa tarefaAtualizada, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.FindAsync(id);

                if (tarefa is null)
                {
                    return Results.NotFound();
                }

                tarefa.Nome = tarefaAtualizada.Nome != null ? tarefaAtualizada.Nome : tarefa.Nome;
                tarefa.Detalhes = tarefaAtualizada.Nome != null ? tarefaAtualizada.Detalhes : tarefa.Detalhes;
                tarefa.CategoriaId = tarefaAtualizada.CategoriaId != 0 ? tarefaAtualizada.CategoriaId : tarefa.CategoriaId;
                if (tarefaAtualizada.Concluida)
                {
                    tarefa.ConcluirTarefa();
                }

                await db.SaveChangesAsync();

                return Results.Ok("Tarefa atualizada com sucesso!");
            });

            app.MapPut("/Tarefas/{id}/Concluir", async (Guid id, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.FindAsync(id);

                if (tarefa is null)
                {
                    return Results.NotFound();
                }

                tarefa.Concluida = true;
                tarefa.ConcluirTarefa();

                await db.SaveChangesAsync();

                return Results.Ok("Tarefa concluida com sucesso!");
            });

            app.MapDelete("/Tarefas/{id}", async (Guid id, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.FindAsync(id);

                if (tarefa is null)
                {
                    return Results.NotFound();
                }

                db.Tarefas.Remove(tarefa);

                await db.SaveChangesAsync();

                return Results.Ok("Tarefa Deletada com sucesso!");
            });
        }
    }
}
