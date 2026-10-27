namespace FitTrack.Application.Common;

public sealed record PageResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalItems, int TotalPages)
{
    public static PageResult<T> Create(IReadOnlyList<T> items, int page, int pageSize, int totalItems) =>
        new(items, page, pageSize, totalItems, (int)Math.Ceiling(totalItems / (double)pageSize));
}
