namespace FitTrack.Application.Exercises;

public sealed record ExerciseDto(int Id, string Name, string MuscleGroup, int DurationMinutes, DateTime CreatedAtUtc);
public sealed record CreateExerciseRequest(string Name, string MuscleGroup, int DurationMinutes);
public sealed record UpdateExerciseRequest(string Name, string MuscleGroup, int DurationMinutes);
