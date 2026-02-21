using Microsoft.AspNetCore.Mvc;
using RL.Backend.Services;
using RL.Data.DataModels;

namespace RL.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcedurePlanUserController : ControllerBase
{
    private readonly IProcedurePlanUserService _procedurePlanUserService;

    public ProcedurePlanUserController(IProcedurePlanUserService procedurePlanUserService)
    {
        _procedurePlanUserService = procedurePlanUserService;
    }

    /// <summary>
    /// Get all users assigned to a procedure
    /// GET: api/procedureplanusr/{planId}/{procedureId}
    /// </summary>
    [HttpGet("{planId}/{procedureId}")]
    public async Task<ActionResult<List<User>>> GetUsersForProcedure(int planId, int procedureId)
    {
        var users = await _procedurePlanUserService.GetUsersForProcedureAsync(planId, procedureId);
        return Ok(users);
    }

    /// <summary>
    /// Assign a user to a procedure
    /// POST: api/procedureplanusr
    /// Body: { "planId": 1, "procedureId": 1, "userId": 1 }
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProcedurePlanUser>> AssignUserToProcedure(
        [FromBody] AssignUserRequest request)
    {
        var result = await _procedurePlanUserService.AssignUserToProcedureAsync(
            request.PlanId, request.ProcedureId, request.UserId);
        return CreatedAtAction(nameof(GetUsersForProcedure),
            new { planId = result.PlanId, procedureId = result.ProcedureId }, result);
    }

    /// <summary>
    /// Remove a user from a procedure
    /// DELETE: api/procedureplanusr/{planId}/{procedureId}/{userId}
    /// </summary>
    [HttpDelete("{planId}/{procedureId}/{userId}")]
    public async Task<IActionResult> RemoveUserFromProcedure(int planId, int procedureId, int userId)
    {
        await _procedurePlanUserService.RemoveUserFromProcedureAsync(planId, procedureId, userId);
        return NoContent();
    }

    /// <summary>
    /// Remove all users from a procedure
    /// DELETE: api/procedureplanusr/{planId}/{procedureId}/remove-all
    /// </summary>
    [HttpDelete("{planId}/{procedureId}/remove-all")]
    public async Task<IActionResult> RemoveAllUsersFromProcedure(int planId, int procedureId)
    {
        await _procedurePlanUserService.RemoveAllUsersFromProcedureAsync(planId, procedureId);
        return NoContent();
    }
}

public class AssignUserRequest
{
    public int PlanId { get; set; }
    public int ProcedureId { get; set; }
    public int UserId { get; set; }
}