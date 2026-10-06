using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GuiaConsole;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public decimal Preco { get; set; }
    public int? CategoriaId { get; set; }
}

public class LojaDb(DbContextOptions<LojaDb> opcoes)
    : DbContext(opcoes)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
}

public static class DadosDemo
{
    public static async Task ExecutarAsync()
    {
        using var conexao = new SqliteConnection(
            "Data Source=:memory:");
        conexao.Open();

        var opcoes = new DbContextOptionsBuilder<LojaDb>()
            .UseSqlite(conexao).Options;
        await using var db = new LojaDb(opcoes);
        await db.Database.EnsureCreatedAsync();

        db.Categorias.Add(new Categoria { Nome = "Bebidas" });
        db.Produtos.AddRange(
            new Produto
            {
                Nome = "Suco", Preco = 8, CategoriaId = 1,
            },
            new Produto { Nome = "Mesa", Preco = 300 });
        await db.SaveChangesAsync();

        // EF Core 10: LeftJoin como operador LINQ
        var lista = await db.Produtos
            .LeftJoin(db.Categorias,
                p => p.CategoriaId, c => c.Id,
                (p, c) => new { p.Nome, Categoria = c!.Nome })
            .ToListAsync();
        foreach (var l in lista)
            Console.WriteLine($"  {l.Nome} -> {l.Categoria}");

        // Atualizacao em lote, sem carregar entidades
        var n = await db.Produtos
            .Where(p => p.Preco > 100)
            .ExecuteUpdateAsync(s => s.SetProperty(
                p => p.Preco, p => p.Preco * 0.9m));
        Console.WriteLine($"  {n} produto(s) reajustado(s)");

        // Dapper: SQL direto, mesma conexao
        var caros = await conexao.QueryAsync<Produto>(
            "select * from Produtos where Preco > @min",
            new { min = 100 });
        Console.WriteLine($"  Dapper: {caros.Count()} caros");
    }
}
