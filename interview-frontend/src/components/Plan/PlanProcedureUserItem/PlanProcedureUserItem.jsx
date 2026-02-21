import ReactSelect from "react-select";
import { useEffect, useState } from "react";
 import { loadAssignedUsers, addUser, removeUser, removeAllUser } from "../../../api/api";

export const PlanProcedureUserItem = ({ planId, procedureId, procedure ,users}) => {
  const [assignedUsers, setAssignedUsers] = useState([]);

  // Load assigned users on mount
  useEffect(() => {
    loadAssignedUsers(planId, procedureId).then((users) => {
      setAssignedUsers(users);
    });
  }, [planId, procedureId]);

  const handleAddUser = async (selectedUser) => {
    await addUser(planId, procedureId, selectedUser.value);
    await loadAssignedUsers(planId, procedureId).then((users) => {
      setAssignedUsers(users);
    });
  };

  const handleRemoveUser = async (userId) => {
    await removeUser(planId, procedureId, userId);
    await loadAssignedUsers(planId, procedureId).then((users) => {
       setAssignedUsers(users);
     }); 
  };

  const handleRemoveAll = async () => {
    await removeAllUser(planId, procedureId);
    await loadAssignedUsers(planId, procedureId).then((users) => {
       setAssignedUsers(users);
     }); 
  };

  const selectedOptions = assignedUsers.map((u) => ({
    value: u.userId,
    label: u.name,
  }));

  const handleAssignUserToProcedure = (selected,actionMeta) => {
      if(actionMeta.action === "select-option"){
        handleAddUser(actionMeta.option);
      }
        else if(actionMeta.action === "remove-value"){
            handleRemoveUser(actionMeta.removedValue.value);
        }
        else if(actionMeta.action === "clear"){
            handleRemoveAll();
        }
    };

  return (
    <div className="py-2">
      <div>{procedure.procedureTitle}</div>

      {/* Select dropdown for adding users */}
      <ReactSelect
        options={users}
        onChange={handleAssignUserToProcedure}
        isMulti={true}
        value={selectedOptions}
        placeholder="Select User to Assign"
        isClearable
      />
    </div>
  );
};
