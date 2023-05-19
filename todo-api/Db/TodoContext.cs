using Microsoft.EntityFrameworkCore;
using todo_api.Db.Models;

namespace todo_api.Db;

public class TodoContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options)
       : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //     var folder = Environment.SpecialFolder.LocalApplicationData;
    //    var path = Environment.GetFolderPath(folder);
    //    var dbPath = System.IO.Path.Join(path, "blogging.db");
    //    options.UseSqlite(dbPath);
    //}
}
