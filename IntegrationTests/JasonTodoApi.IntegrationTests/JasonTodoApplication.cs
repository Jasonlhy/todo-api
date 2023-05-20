using JasonTodoInfrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Data.Common;

namespace JasonTodoApi.IntegrationTests;

internal class JasonTodoApplication : WebApplicationFactory<Program>
{
    private DbConnection _connection;
    private DbContextOptions<TodoContext> _contextOptions;

    public new void Dispose() {
        base.Dispose();
        _connection.Dispose();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        //var root = new InMemoryDatabaseRoot();
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(TodoContext));

            _contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlite(_connection)
                .Options;

            //services.AddDbContext<TodoContext>(optionsBuilder =>
            //{
            //    var folder = Environment.SpecialFolder.LocalApplicationData;
            //    var path = Environment.GetFolderPath(folder);
            //    var dbPath = System.IO.Path.Join(path, "blogging.db");
            //    optionsBuilder.UseSqlite($"Data Source={dbPath}");
            //});

            services.RemoveAll(typeof(DbContextOptions<TodoContext>));
            services.AddDbContext<TodoContext>(options =>
            {
                options.UseSqlite(_connection);

                //_contextOptions = new DbContextOptionsBuilder<TodoContext>()
                //    .UseSqlite(_connection)
                //.Options;
            });
        });

        return base.CreateHost(builder);
    }
}
