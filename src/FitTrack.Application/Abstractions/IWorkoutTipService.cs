namespace FitTrack.Application.Abstractions;
public interface IWorkoutTipService { Task<string> GetTipAsync(CancellationToken ct); }
