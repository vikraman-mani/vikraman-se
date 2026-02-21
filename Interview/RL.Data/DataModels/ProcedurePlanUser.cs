using RL.Data.DataModels.Common;

namespace RL.Data.DataModels;

public class ProcedurePlanUser : IChangeTrackable
{
    public int ProcedurePlanUserId { get; set; }

    // Foreign Keys
    public int PlanId { get; set; }
    public int ProcedureId { get; set; }
    public int UserId { get; set; }

    // Navigation Properties
    public Plan Plan { get; set; }
    public Procedure Procedure { get; set; }
    public User User { get; set; }

    // Tracking
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}