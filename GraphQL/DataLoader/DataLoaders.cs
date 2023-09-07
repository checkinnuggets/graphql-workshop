using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.DataLoader;

public static class DataLoaders
{
    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Attendee>> AttendeeByIdDataLoader(
        IReadOnlyList<int> keys,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Attendees
            .Where(a => keys.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id, cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Session>> SessionByIdDataLoader(
        IReadOnlyList<int> keys,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Sessions
            .Where(a => keys.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id, cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Speaker>> SpeakerByIdDataLoader(
        IReadOnlyList<int> keys,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Speakers
            .Where(a => keys.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id, cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Track>> TrackByIdDataLoader(
        IReadOnlyList<int> keys,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tracks
            .Where(a => keys.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id, cancellationToken);
    }
}