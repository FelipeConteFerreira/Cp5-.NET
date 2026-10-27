namespace FitTrack.Domain.Entities;

public sealed class Exercise
{
    private Exercise() { }
    public Exercise(string name, string muscleGroup, int durationMinutes)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Nome obrigatório.") : name.Trim();
        MuscleGroup = string.IsNullOrWhiteSpace(muscleGroup) ? throw new ArgumentException("Grupo muscular obrigatório.") : muscleGroup.Trim();
        DurationMinutes = durationMinutes > 0 ? durationMinutes : throw new ArgumentOutOfRangeException(nameof(durationMinutes));
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string MuscleGroup { get; private set; } = "";
    public int DurationMinutes { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public void Update(string name, string muscleGroup, int durationMinutes)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome obrigatório.");
        if (string.IsNullOrWhiteSpace(muscleGroup)) throw new ArgumentException("Grupo muscular obrigatório.");
        if (durationMinutes <= 0) throw new ArgumentOutOfRangeException(nameof(durationMinutes));
        Name = name.Trim(); MuscleGroup = muscleGroup.Trim(); DurationMinutes = durationMinutes;
    }
}
