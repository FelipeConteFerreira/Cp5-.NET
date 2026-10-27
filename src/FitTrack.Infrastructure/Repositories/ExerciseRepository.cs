using FitTrack.Application.Abstractions;
using FitTrack.Application.Common;
using FitTrack.Domain.Entities;
using FitTrack.Domain.Errors;
using FitTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Infrastructure.Repositories;

public sealed class ExerciseRepository(FitTrackDbContext db) : IExerciseRepository
{
    public async Task<Exercise?> GetByIdAsync(int id, CancellationToken ct) =>
        await db.Exercises.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<Exercise?> GetTrackedByIdAsync(int id, CancellationToken ct) =>
        await db.Exercises.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<PageResult<Exercise>> GetPageAsync(int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);
        var query = db.Exercises.AsNoTracking().OrderBy(x => x.Id);
        var total = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return PageResult<Exercise>.Create(items, page, pageSize, total);
    }

    public async Task<Exercise> AddAsync(Exercise exercise, CancellationToken ct)
    {
        try { db.Exercises.Add(exercise); await db.SaveChangesAsync(ct); return exercise; }
        catch (DbUpdateException ex) { throw new RepositoryException("Não foi possível salvar o exercício.", ex); }
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct)
    {
        try { return await db.SaveChangesAsync(ct) > 0; }
        catch (DbUpdateException ex) { throw new RepositoryException("Falha ao persistir alterações.", ex); }
    }

    public void Remove(Exercise exercise) => db.Exercises.Remove(exercise);
}
