using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


static void DbContextOptions (DbContextOptionsBuilder options)
{
    options.UseSqlite("Data Source=conferences.db");
    options.EnableSensitiveDataLogging();
}

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(DbContextOptions);
builder.Services.AddDbContext<ApplicationDbContext>(DbContextOptions);
builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddGraphQLTypes()                          // Auto-generation from HotChocolate.Analyzers package.  Name will typically be 'Add{ProjectName}Types()'.
    .AddGlobalObjectIdentification()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.MapGet("/", () => "A GraphQL API");

app.UseWebSockets();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

// Apply migrations to initialise database
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
