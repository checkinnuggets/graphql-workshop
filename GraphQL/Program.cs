using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// A Guide to Entity Framework with Hot Chocolate 13
// https://www.youtube.com/watch?v=BcTPIGLYB0I
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=conferences.db");
    options.EnableSensitiveDataLogging();
});

builder.Services
    .AddGraphQLServer()
    .ConfigureApplicationGraphQLServer();

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