using FitTrack.Domain.Entities;
using FitTrack.Application.Common;

namespace FitTrack.Application.Abstractions;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(int id, CancellationToken ct);
    Task<Exercise?> GetTrackedByIdAsync(int id, CancellationToken ct);
    Task<PageResult<Exercise>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<Exercise> AddAsync(Exercise exercise, CancellationToken ct);
    Task<bool> SaveChangesAsync(CancellationToken ct);
    void Remove(Exercise exercise);
}
