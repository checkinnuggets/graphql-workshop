using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Types;

public class AttendeeType : ObjectType<Attendee>
{
    protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode(async (ctx, id)                                // Adding async-await here gets rid of the warning.
                => await ctx.DataLoader<AttendeeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted)   // Need to investigate this further.
            );

        descriptor
            .Field(t => t.SessionsAttendees)
            .ResolveWith<AttendeeResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .Name("sessions");
    }

    private class AttendeeResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            [Parent] Attendee attendee,
            ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            var speakerIds = await dbContext.Attendees
                .Where(a => a.Id == attendee.Id)
                .Include(a => a.SessionsAttendees)
                .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
                .ToArrayAsync(cancellationToken);

            return await sessionById.LoadAsync(speakerIds, cancellationToken);
        }
    }
}