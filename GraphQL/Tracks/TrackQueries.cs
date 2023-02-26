using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Tracks
{
    [ExtendObjectType("Query")]
    public class TrackQueries
    {
        [UsePaging]
        public IQueryable<Track> GetTracks(
            ApplicationDbContext context)
        {
            return context.Tracks.OrderBy(t => t.Name);
        }

        public Task<Track> GetTrackByNameAsync(
            string name,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            return context.Tracks.FirstAsync(t => t.Name == name);
        }

        public async Task<IEnumerable<Track>> GetTrackByNamesAsync(
            string[] names,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            return await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync();
        }

        public Task<Track> GetTrackByIdAsync(
            [ID(nameof(Track))] int id,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            return trackById.LoadAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Track>> GetTracksByIdAsync(
            [ID(nameof(Track))] int[] ids,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            return await trackById.LoadAsync(ids, cancellationToken);
        }
    }
}