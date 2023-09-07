using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

static void DbContextOptions (DbContextOptionsBuilder options)
{
    options.UseSqlite("Data Source=conferences.db");
    options.EnableSensitiveDataLogging();
}

builder.Services.AddDbContextPool<ApplicationDbContext>(DbContextOptions);

builder.Services
    .AddGraphQLServer()
    .SetupApplicationGraphQLServer();

var app = builder.Build();

app.MapGet("/", () => "A GraphQL API");

app.UseWebSockets();
app.UseRouting();

app.MapGraphQL();

// Apply migrations to initialise database
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();