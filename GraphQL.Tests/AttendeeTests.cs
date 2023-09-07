using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;

namespace GraphQL.Tests;

public class AttendeeTests
{
    [Fact]
    public async Task Attendee_Schema_Changed()
    {
        // arrange
        // act
        var schema = await new ServiceCollection()
            .AddPooledDbContextFactory<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("Data Source=conferences.db")
            )
            .AddGraphQL()
            .ConfigureApplicationGraphQLServer()
            .BuildSchemaAsync();

        // assert
        schema.Print().MatchSnapshot();
    }

    [Fact]
    public async Task RegisterAttendee()
    {
        // arrange
        var executor = await new ServiceCollection()
            .AddPooledDbContextFactory<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("Data Source=conferences.db")
            )
            .AddGraphQL()
            .ConfigureApplicationGraphQLServer()
            .BuildRequestExecutorAsync();

        // act
        var result = await executor.ExecuteAsync(@"
        mutation RegisterAttendee {
            registerAttendee(
                input: {
                    emailAddress: ""michael@chillicream.com""
                        firstName: ""michael""
                        lastName: ""staib""
                        userName: ""michael3""
                    })
            {
                attendee {
                    id
                }
            }
        }");

        // assert
        result.ToJson().MatchSnapshot();
    }
}