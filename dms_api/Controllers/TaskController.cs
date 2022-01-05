using DepartmentManagementSystem.API.Filters;
using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities.Extensions;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace dms_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public TaskController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetTasks([FromQuery] TaskParameters taskParameters)
        {
            try
            {
                var tasks = _repository.Task.GetAllTasks(taskParameters);

                var metadata = new
                {
                    tasks.TotalCount,
                    tasks.PageSize,
                    tasks.CurrentPage,
                    tasks.TotalPages,
                    tasks.HasNext,
                    tasks.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {tasks.Count} tasks from database.");

                var shapedTasks = tasks.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedTasks);
                }

                for (var index = 0; index < tasks.Count; index++)
                {
                    var brandLinks = CreateLinksForTask(tasks[index].Id, taskParameters.Fields);
                    shapedTasks[index].Add("Links", brandLinks);
                }

                var tasksWrapper = new LinkCollectionWrapper<Entity>(shapedTasks);

                return Ok(CreateLinksForTasks(tasksWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllTasks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "TaskById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetTaskById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var task = _repository.Task.GetTaskById(id, fields);

                if (task.Id == Guid.Empty)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped task with id: {id}");
                    return Ok(task.Entity);
                }

                task.Entity.Add("Links", CreateLinksForTask(task.Id, fields));

                return Ok(task.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetTaskById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] Task task)
        {
            try
            {
                if (task.IsObjectNull())
                {
                    _logger.LogError("Task object sent from client is null.");
                    return BadRequest("Task object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid task object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Task.CreateTask(task);
                _repository.Save();

                return CreatedAtRoute("TaskById", new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] Task task)
        {
            try
            {
                if (task.IsObjectNull())
                {
                    _logger.LogError("Task object sent from client is null.");
                    return BadRequest("Task object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid task object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbTask = _repository.Task.GetTaskById(id);
                if (dbTask.IsEmptyObject())
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Task.UpdateTask(dbTask, task);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                var task = _repository.Task.GetTaskById(id);
                if (task.IsEmptyObject())
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Task.DeleteTask(task);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTask action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForTask(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetTaskById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteTask), values: new {id}), "delete_task", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateTask), values: new {id}), "update_task", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForTasks(LinkCollectionWrapper<Entity> tasksWrapper)
        {
            tasksWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetTasks), values: new { }), "self", "GET"));

            return tasksWrapper;
        }
    }
}
