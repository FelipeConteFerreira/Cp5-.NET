using FitTrack.Application.Abstractions;
using FitTrack.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[ApiController]
[Route("api/external")]
public sealed class ExternalController(IWorkoutTipService tips) : ControllerBase
{
    [HttpGet("workout-tip")]
    public async Task<IActionResult> GetTip(CancellationToken ct)
    {
        try { return Ok(OperationResult<string>.Success(await tips.GetTipAsync(ct))); }
        catch (HttpRequestException) { return StatusCode(502, OperationResult<string>.Failure("Serviço externo indisponível.", "external_service_error")); }
        catch (TaskCanceledException) { return StatusCode(504, OperationResult<string>.Failure("Tempo limite do serviço externo excedido.", "external_timeout")); }
    }
}
