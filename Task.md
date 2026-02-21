**Task Completed**: 
1. Create table structure to store assigned users (ProcedurePlanUser)
2. Create endpoints to interact with the new table
3. Hook up frontend to persist and manage user assignments
4. Implement remove single user and remove all users functionality

**Backend:**
NEW - Junction table model: Create a new DataModel file: Interview/RL.Data/DataModels/ProcedurePlanUser.cs
Add DbSet<ProcedurePlanUser> and entity config
Add the navigation property to the Plan class - access all assigned users for a plan through plan.ProcedurePlanUsers

Add migration and apply migration -- AddProcedurePlanUser

Create: Interview/RL.Backend/Services/ProcedurePlanUserService.cs           - Business logic for user assignments
  -- GetUsersForProcedureAsync: Loads persisted users for a procedure (solves the refresh bug)
  -- AssignUserToProcedureAsync: Adds user assignment, with duplicate check
  -- RemoveUserFromProcedureAsync: Removes a single user assignment
  -- RemoveAllUsersFromProcedureAsync: Removes all users from a procedure at once

Create: Interview/RL.Backend/Controllers/ProcedurePlanUserController.cs     - REST API endpoints
  -- GET /{planId}/{procedureId}: Retrieves assigned users (solves the refresh bug)
  -- POST: Adds a user to a procedure
  -- DELETE /{planId}/{procedureId}/{userId}: Removes one user
  -- DELETE /{planId}/{procedureId}/remove-all: Removes all users at once

Register service with DI container


**Frontend**

API Endpoints:
   Call GET /api/ProcedurePlanUser/{planId}/{procedureId}                 endpoint on component load
   Call POST /api/ProcedurePlanUser                                       when user selects a user from dropdown
   Call DELETE /api/ProcedurePlanUser/{planId}/{procedureId}/{userId}     for remove-one
   Call DELETE /api/ProcedurePlanUser/{planId}/{procedureId}/remove-all   for remove-all

New Component: PlanProcedureUserItem.jsx
   useEffect with [planId, procedureId] dependency loads assigned users when procedure changes
      handleAssignUserToProcedure - find the action
      handleAddUser - Add user to Procedure with planID
      handleRemoveUser(userId) - removes individual user via DELETE
      handleRemoveAll - removes all users via DELETE with /remove-all endpoint
   Component reloads data after each mutation to ensure UI stays in sync
