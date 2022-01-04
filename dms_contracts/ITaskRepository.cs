using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace DepartmentManagementSystem.Contracts
{
    public interface ITaskRepository: IRepositoryBase<Task>
    {
        PagedList<ShapedEntity> GetAllTasks(TaskParameters taskParameters);
        ShapedEntity GetTaskById(Guid taskId, string fields);
        Task GetTaskById(Guid taskId);
        void CreateTask(Task task);
        void UpdateTask(Task dbTask, Task task);
        void DeleteTask(Task task);
    }
}
