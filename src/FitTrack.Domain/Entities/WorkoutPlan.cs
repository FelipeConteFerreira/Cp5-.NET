namespace FitTrack.Domain.Entities;

public sealed class WorkoutPlan
{
    private WorkoutPlan() { }
    public WorkoutPlan(string title, string description)
    {
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Título obrigatório.") : title.Trim();
        Description = description?.Trim() ?? "";
    }
    public int Id { get; private set; }
    public string Title { get; private set; } = "";
    public string Description { get; private set; } = "";
}
