using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Extensions;
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

namespace DepartmentManagementSystem.Repository
{
    public class TaskRepository: RepositoryBase<Task>, ITaskRepository
    {
        private readonly ISortHelper<Task> _sortHelper;

        private readonly IDataShaper<Task> _dataShaper;

        public TaskRepository(RepositoryContext repositoryContext, ISortHelper<Task> sortHelper, IDataShaper<Task> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateTask(Task task)
        {
            task.TimeCreated = DateTime.Now;
            Create(task);
        }

        public void DeleteTask(Task task)
        {
            task.Deleted = true;
            task.TimeDeleted = DateTime.Now;
            Task dbTask = GetTaskById(task.Id);
            UpdateTask(dbTask, task);
        }

        public PagedList<ShapedEntity> GetAllTasks(TaskParameters taskParameters)
        {
            var tasks = FindByCondition(task => !task.Deleted);

            SearchByName(ref tasks, taskParameters.Name);

            var sortedTasks = _sortHelper.ApplySort(tasks, taskParameters.OrderBy);
            var shapedTasks = _dataShaper.ShapeData(sortedTasks, taskParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedTasks, taskParameters.PageNumber, taskParameters.PageSize);
        }

        public ShapedEntity GetTaskById(Guid taskId, string fields)
        {
            var task = FindByCondition(task => task.Id.Equals(taskId)).FirstOrDefault();

            if (task == null)
            {
                task = new Task();
            }

            return _dataShaper.ShapeData(task, fields);
        }

        public Task GetTaskById(Guid taskId)
        {
            return FindByCondition(i => i.Id.Equals(taskId)).FirstOrDefault();
        }

        public void UpdateTask(Task dbTask, Task task)
        {
            dbTask.Map(task);
            Update(dbTask);
        }

        private void SearchByName(ref IQueryable<Task> tasks, string taskName)
        {
            if (!tasks.Any() || string.IsNullOrWhiteSpace(taskName))
            {
                return;
            }

            tasks = tasks.Where(i => i.Name.ToLower().Contains(taskName.Trim().ToLower()));
        }
    }
}
