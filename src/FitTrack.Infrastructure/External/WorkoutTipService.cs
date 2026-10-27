using FitTrack.Application.Abstractions;
using System.Net.Http.Json;

namespace FitTrack.Infrastructure.External;

public sealed class WorkoutTipService(HttpClient http) : IWorkoutTipService
{
    public async Task<string> GetTipAsync(CancellationToken ct)
    {
        using var response = await http.GetAsync("https://uselessfacts.jsph.pl/api/v2/facts/today?language=en", ct);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<FactResponse>(cancellationToken: ct);
        return payload?.Text ?? "Mantenha a consistência e respeite seus limites.";
    }
    private sealed record FactResponse(string Text);
}
