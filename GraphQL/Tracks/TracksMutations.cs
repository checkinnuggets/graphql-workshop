using ConferencePlanner.GraphQL.Common;
using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Tracks;

[MutationType]
public class TrackMutations
{
    public async Task<AddTrackPayload> AddTrackAsync(
        AddTrackInput input,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var track = new Track { Name = input.Name };
        context.Tracks.Add(track);

        await context.SaveChangesAsync(cancellationToken);

        return new AddTrackPayload(track);
    }

    public async Task<RenameTrackPayload> RenameTrackAsync(
        RenameTrackInput input,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var track = await context.Tracks.FindAsync(input.Id);

        if (track == null)
        {
            return new RenameTrackPayload(
                new UserError("Track not found.", "TRACK_NOT_FOUND"));
        }

        track.Name = input.Name;

        await context.SaveChangesAsync(cancellationToken);

        return new RenameTrackPayload(track);
    }
}