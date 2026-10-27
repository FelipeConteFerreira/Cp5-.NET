using FitTrack.Application.Abstractions;
using FitTrack.Application.Common;
using FitTrack.Application.Exercises;
using FitTrack.Domain.Entities;
using FitTrack.Domain.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[ApiController]
[Route("api/exercises")]
public sealed class ExercisesController(IExerciseRepository repository) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest(OperationResult<string>.Failure("page deve ser >= 1 e pageSize entre 1 e 100.", "invalid_pagination"));
        var result = await repository.GetPageAsync(page, pageSize, ct);
        var items = result.Items.Select(ToDto).ToArray();
        var links = new Dictionary<string, string>
        {
            ["self"] = Url.ActionLink(nameof(GetPage), values: new { page, pageSize })!,
            ["first"] = Url.ActionLink(nameof(GetPage), values: new { page = 1, pageSize })!
        };
        if (page < result.TotalPages) links["next"] = Url.ActionLink(nameof(GetPage), values: new { page = page + 1, pageSize })!;
        if (page > 1) links["previous"] = Url.ActionLink(nameof(GetPage), values: new { page = page - 1, pageSize })!;
        return Ok(new
        {
            data = new { items, result.Page, result.PageSize, result.TotalItems, result.TotalPages },
            links
        });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity is null) return NotFound(OperationResult<string>.Failure("Exercício não encontrado.", "not_found"));
        return Ok(new { data = ToDto(entity), links = new { self = Url.ActionLink(nameof(GetById), values: new { id }), collection = Url.ActionLink(nameof(GetPage)) } });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateExerciseRequest request, CancellationToken ct)
    {
        try
        {
            var entity = new Exercise(request.Name, request.MuscleGroup, request.DurationMinutes);
            await repository.AddAsync(entity, ct);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, OperationResult<ExerciseDto>.Success(ToDto(entity)));
        }
        catch (ArgumentException ex) { return BadRequest(OperationResult<string>.Failure(ex.Message, "validation_error")); }
        catch (RepositoryException ex) { return StatusCode(500, OperationResult<string>.Failure(ex.Message, "repository_error")); }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateExerciseRequest request, CancellationToken ct)
    {
        var entity = await repository.GetTrackedByIdAsync(id, ct);
        if (entity is null) return NotFound(OperationResult<string>.Failure("Exercício não encontrado.", "not_found"));
        try
        {
            entity.Update(request.Name, request.MuscleGroup, request.DurationMinutes);
            return Ok(OperationResult<ExerciseDto>.Success(ToDto(entity)));
        }
        catch (ArgumentException ex) { return BadRequest(OperationResult<string>.Failure(ex.Message, "validation_error")); }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await repository.GetTrackedByIdAsync(id, ct);
        if (entity is null) return NotFound(OperationResult<string>.Failure("Exercício não encontrado.", "not_found"));
        repository.Remove(entity);
        await repository.SaveChangesAsync(ct);
        return NoContent();
    }

    private static ExerciseDto ToDto(Exercise e) => new(e.Id, e.Name, e.MuscleGroup, e.DurationMinutes, e.CreatedAtUtc);
}
