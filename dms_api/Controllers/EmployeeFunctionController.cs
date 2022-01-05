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
    public class EmployeeFunctionController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public EmployeeFunctionController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetEmployeeFunctions([FromQuery] EmployeeFunctionParameters employeeFunctionParameters)
        {
            try
            {
                var employeeFunctions = _repository.EmployeeFunction.GetAllEmployeeFunctions(employeeFunctionParameters);

                var metadata = new
                {
                    employeeFunctions.TotalCount,
                    employeeFunctions.PageSize,
                    employeeFunctions.CurrentPage,
                    employeeFunctions.TotalPages,
                    employeeFunctions.HasNext,
                    employeeFunctions.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {employeeFunctions.Count} employeeFunctions from database.");

                var shapedEmployeeFunctions = employeeFunctions.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedEmployeeFunctions);
                }

                for (var index = 0; index < employeeFunctions.Count; index++)
                {
                    var brandLinks = CreateLinksForEmployeeFunction(employeeFunctions[index].Id, employeeFunctionParameters.Fields);
                    shapedEmployeeFunctions[index].Add("Links", brandLinks);
                }

                var employeeFunctionsWrapper = new LinkCollectionWrapper<Entity>(shapedEmployeeFunctions);

                return Ok(CreateLinksForEmployeeFunctions(employeeFunctionsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllEmployeeFunctions action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "EmployeeFunctionById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetEmployeeFunctionById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var employeeFunction = _repository.EmployeeFunction.GetEmployeeFunctionById(id, fields);

                if (employeeFunction.Id == Guid.Empty)
                {
                    _logger.LogError($"EmployeeFunction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped employeeFunction with id: {id}");
                    return Ok(employeeFunction.Entity);
                }

                employeeFunction.Entity.Add("Links", CreateLinksForEmployeeFunction(employeeFunction.Id, fields));

                return Ok(employeeFunction.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetEmployeeFunctionById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateEmployeeFunction([FromBody] EmployeeFunction employeeFunction)
        {
            try
            {
                if (employeeFunction.IsObjectNull())
                {
                    _logger.LogError("EmployeeFunction object sent from client is null.");
                    return BadRequest("EmployeeFunction object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid employeeFunction object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.EmployeeFunction.CreateEmployeeFunction(employeeFunction);
                _repository.Save();

                return CreatedAtRoute("EmployeeFunctionById", new { id = employeeFunction.Id }, employeeFunction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateEmployeeFunction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeFunction(Guid id, [FromBody] EmployeeFunction employeeFunction)
        {
            try
            {
                if (employeeFunction.IsObjectNull())
                {
                    _logger.LogError("EmployeeFunction object sent from client is null.");
                    return BadRequest("EmployeeFunction object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid employeeFunction object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbEmployeeFunction = _repository.EmployeeFunction.GetEmployeeFunctionById(id);
                if (dbEmployeeFunction.IsEmptyObject())
                {
                    _logger.LogError($"EmployeeFunction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.EmployeeFunction.UpdateEmployeeFunction(dbEmployeeFunction, employeeFunction);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateEmployeeFunction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeFunction(Guid id)
        {
            try
            {
                var employeeFunction = _repository.EmployeeFunction.GetEmployeeFunctionById(id);
                if (employeeFunction.IsEmptyObject())
                {
                    _logger.LogError($"EmployeeFunction with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.EmployeeFunction.DeleteEmployeeFunction(employeeFunction);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteEmployeeFunction action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForEmployeeFunction(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetEmployeeFunctionById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteEmployeeFunction), values: new {id}), "delete_employee_function", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateEmployeeFunction), values: new {id}), "update_employee_function", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForEmployeeFunctions(LinkCollectionWrapper<Entity> employeeFunctionsWrapper)
        {
            employeeFunctionsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetEmployeeFunctions), values: new { }), "self", "GET"));

            return employeeFunctionsWrapper;
        }
    }
}
