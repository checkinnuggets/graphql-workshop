using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Attendees;

public class SessionAttendeeCheckIn
{
    public SessionAttendeeCheckIn(int attendeeId, int sessionId)
    {
        AttendeeId = attendeeId;
        SessionId = sessionId;
    }

    [ID(nameof(Attendee))]
    public int AttendeeId { get; }

    [ID(nameof(Session))]
    public int SessionId { get; }

    public async Task<int> CheckInCountAsync(
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Sessions
            .Where(session => session.Id == SessionId)
            .SelectMany(session => session.SessionAttendees)
            .CountAsync(cancellationToken);
    }

    public Task<Attendee> GetAttendeeAsync(
        AttendeeByIdDataLoader attendeeById,
        CancellationToken cancellationToken)
    {
        return attendeeById.LoadAsync(AttendeeId, cancellationToken);
    }

    public Task<Session> GetSessionAsync(
        SessionByIdDataLoader sessionById,
        CancellationToken cancellationToken)
    {
        return sessionById.LoadAsync(SessionId, cancellationToken);
    }
}