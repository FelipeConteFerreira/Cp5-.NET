namespace FitTrack.Domain.Errors;

public sealed class EntityNotFoundException(string message) : Exception(message);
public sealed class RepositoryException(string message, Exception? inner = null) : Exception(message, inner);
