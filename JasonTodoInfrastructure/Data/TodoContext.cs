using JasonTodoInfrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JasonTodoInfrastructure.Data;

public class TodoContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    //public TodoContext(DbContextOptions<TodoContext> options)
    //   : base(options)
    //{
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "blogging.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        // optionsBuilder.Options.UseSqlite($"Data Source={dbPath}");
        // options.UseSqlite($"Data Source={dbPath}");

        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        //var dbPath = System.IO.Path.Join(path, "blogging.db");
        //options.UseSqlite(dbPath);
    }
}
