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

namespace dms_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public DepartmentController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetDepartments([FromQuery] DepartmentParameters departmentParameters)
        {
            try
            {
                var departments = _repository.Department.GetAllDepartments(departmentParameters);

                var metadata = new
                {
                    departments.TotalCount,
                    departments.PageSize,
                    departments.CurrentPage,
                    departments.TotalPages,
                    departments.HasNext,
                    departments.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {departments.Count} departments from database.");

                var shapedDepartments = departments.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedDepartments);
                }

                for (var index = 0; index < departments.Count; index++)
                {
                    var brandLinks = CreateLinksForDepartment(departments[index].Id, departmentParameters.Fields);
                    shapedDepartments[index].Add("Links", brandLinks);
                }

                var departmentsWrapper = new LinkCollectionWrapper<Entity>(shapedDepartments);

                return Ok(CreateLinksForDepartments(departmentsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllDepartments action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "DepartmentById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetDepartmentById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var department = _repository.Department.GetDepartmentById(id, fields);

                if (department.Id == Guid.Empty)
                {
                    _logger.LogError($"Department with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped department with id: {id}");
                    return Ok(department.Entity);
                }

                department.Entity.Add("Links", CreateLinksForDepartment(department.Id, fields));

                return Ok(department.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetDepartmentById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] Department department)
        {
            try
            {
                if (department.IsObjectNull())
                {
                    _logger.LogError("Department object sent from client is null.");
                    return BadRequest("Department object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid department object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Department.CreateDepartment(department);
                _repository.Save();

                return CreatedAtRoute("DepartmentById", new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateDepartment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(Guid id, [FromBody] Department department)
        {
            try
            {
                if (department.IsObjectNull())
                {
                    _logger.LogError("Department object sent from client is null.");
                    return BadRequest("Department object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid department object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbDepartment = _repository.Department.GetDepartmentById(id);
                if (dbDepartment.IsEmptyObject())
                {
                    _logger.LogError($"Department with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Department.UpdateDepartment(dbDepartment, department);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateDepartment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(Guid id)
        {
            try
            {
                var department = _repository.Department.GetDepartmentById(id);
                if (department.IsEmptyObject())
                {
                    _logger.LogError($"Department with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Department.DeleteDepartment(department);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteDepartment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForDepartment(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetDepartmentById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteDepartment), values: new {id}), "delete_department", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateDepartment), values: new {id}), "update_department", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForDepartments(LinkCollectionWrapper<Entity> departmentsWrapper)
        {
            departmentsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetDepartments), values: new { }), "self", "GET"));

            return departmentsWrapper;
        }
    }
}
