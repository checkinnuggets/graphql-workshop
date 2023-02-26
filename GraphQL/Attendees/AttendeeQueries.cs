using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Language;

namespace ConferencePlanner.GraphQL.Attendees
{
    [ExtendObjectType(OperationType.Query)]
    public class AttendeeQueries
    {
        [UsePaging]
        public IQueryable<Attendee> GetAttendees(
            ApplicationDbContext context)
        {
            return context.Attendees;
        }

        public Task<Attendee> GetAttendeeByIdAsync(
            [ID(nameof(Attendee))] int id,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken)
        {
            return attendeeById.LoadAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesByIdAsync(
            [ID(nameof(Attendee))] int[] ids,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken)
        {
            return await attendeeById.LoadAsync(ids, cancellationToken);
        }
    }
}