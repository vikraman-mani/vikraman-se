using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Services;

public interface IProcedurePlanUserService
{
    Task<List<User>> GetUsersForProcedureAsync(int planId, int procedureId);
    Task<ProcedurePlanUser> AssignUserToProcedureAsync(int planId, int procedureId, int userId);
    Task RemoveUserFromProcedureAsync(int planId, int procedureId, int userId);
    Task RemoveAllUsersFromProcedureAsync(int planId, int procedureId);
}

public class ProcedurePlanUserService : IProcedurePlanUserService
{
    private readonly RLContext _context;

    public ProcedurePlanUserService(RLContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users assigned to a specific procedure within a plan
    /// </summary>
    public async Task<List<User>> GetUsersForProcedureAsync(int planId, int procedureId)
    {
        return await _context.ProcedurePlanUsers
            .Where(ppu => ppu.PlanId == planId && ppu.ProcedureId == procedureId)
            .Select(ppu => ppu.User)
            .ToListAsync();
    }

    /// <summary>
    /// Assigns a user to a procedure within a plan
    /// </summary>
    public async Task<ProcedurePlanUser> AssignUserToProcedureAsync(int planId, int procedureId, int userId)
    {
        // Check if assignment already exists
        var existingAssignment = await _context.ProcedurePlanUsers
            .FirstOrDefaultAsync(ppu => ppu.PlanId == planId &&
                                        ppu.ProcedureId == procedureId &&
                                        ppu.UserId == userId);

        if (existingAssignment != null)
            return existingAssignment;

        var procedurePlanUser = new ProcedurePlanUser
        {
            PlanId = planId,
            ProcedureId = procedureId,
            UserId = userId
        };

        _context.ProcedurePlanUsers.Add(procedurePlanUser);
        await _context.SaveChangesAsync();

        return procedurePlanUser;
    }

    /// <summary>
    /// Removes a specific user from a procedure within a plan
    /// </summary>
    public async Task RemoveUserFromProcedureAsync(int planId, int procedureId, int userId)
    {
        var assignment = await _context.ProcedurePlanUsers
            .FirstOrDefaultAsync(ppu => ppu.PlanId == planId &&
                                        ppu.ProcedureId == procedureId &&
                                        ppu.UserId == userId);

        if (assignment != null)
        {
            _context.ProcedurePlanUsers.Remove(assignment);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Removes all users from a procedure within a plan
    /// </summary>
    public async Task RemoveAllUsersFromProcedureAsync(int planId, int procedureId)
    {
        var assignments = await _context.ProcedurePlanUsers
            .Where(ppu => ppu.PlanId == planId && ppu.ProcedureId == procedureId)
            .ToListAsync();

        if (assignments.Any())
        {
            _context.ProcedurePlanUsers.RemoveRange(assignments);
            await _context.SaveChangesAsync();
        }
    }
}