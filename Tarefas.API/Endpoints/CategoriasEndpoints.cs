using Microsoft.EntityFrameworkCore;
using Tarefas.API.Context;
using Tarefas.API.Model;

namespace Tarefas.API.Endpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            app.MapGet("/Categorias", async (TarefasDbContext db) => await db.Categorias.ToListAsync());

            app.MapGet("/Categorias/{id}", async (int id, TarefasDbContext db) =>
                        await db.Categorias.FindAsync(id) is Categoria categoria
                        ? Results.Ok(categoria)
                        : Results.NotFound());

            app.MapPost("/Categorias", async (Categoria categoria, TarefasDbContext db) =>
            {
                if (categoria == null) return Results.BadRequest("Requisição inválida!");

                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();

                return Results.Created($"/Categorias/{categoria.CategoriaId}", categoria);
            });

            app.MapPut("/Categorias/{id}", async (int id, Categoria categoriaAtualizada, TarefasDbContext db) =>
            {
                var categoria = await db.Categorias.FindAsync(id);

                if (categoria is null)
                {
                    return Results.NotFound();
                }

                categoria.Nome = categoriaAtualizada.Nome;

                await db.SaveChangesAsync();

                return Results.Ok("Categoria atualizada com sucesso!");
            });

            app.MapDelete("/Categorias/{id}", async (int id, TarefasDbContext db) =>
            {
                var categoria = await db.Categorias.FindAsync(id);

                if (categoria is null)
                {
                    return Results.NotFound();
                }

                db.Categorias.Remove(categoria);

                await db.SaveChangesAsync();

                return Results.Ok("Categoria Deletada com sucesso!");
            });
        }
    }
}
