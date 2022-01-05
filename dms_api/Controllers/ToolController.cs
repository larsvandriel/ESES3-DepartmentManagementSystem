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
    public class ToolController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public ToolController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetTools([FromQuery] ToolParameters toolParameters)
        {
            try
            {
                var tools = _repository.Tool.GetAllTools(toolParameters);

                var metadata = new
                {
                    tools.TotalCount,
                    tools.PageSize,
                    tools.CurrentPage,
                    tools.TotalPages,
                    tools.HasNext,
                    tools.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {tools.Count} tools from database.");

                var shapedTools = tools.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedTools);
                }

                for (var index = 0; index < tools.Count; index++)
                {
                    var brandLinks = CreateLinksForTool(tools[index].Id, toolParameters.Fields);
                    shapedTools[index].Add("Links", brandLinks);
                }

                var toolsWrapper = new LinkCollectionWrapper<Entity>(shapedTools);

                return Ok(CreateLinksForTools(toolsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllTools action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "ToolById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetToolById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var tool = _repository.Tool.GetToolById(id, fields);

                if (tool.Id == Guid.Empty)
                {
                    _logger.LogError($"Tool with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped tool with id: {id}");
                    return Ok(tool.Entity);
                }

                tool.Entity.Add("Links", CreateLinksForTool(tool.Id, fields));

                return Ok(tool.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetToolById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateTool([FromBody] Tool tool)
        {
            try
            {
                if (tool.IsObjectNull())
                {
                    _logger.LogError("Tool object sent from client is null.");
                    return BadRequest("Tool object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid tool object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Tool.CreateTool(tool);
                _repository.Save();

                return CreatedAtRoute("ToolById", new { id = tool.Id }, tool);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateTool action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTool(Guid id, [FromBody] Tool tool)
        {
            try
            {
                if (tool.IsObjectNull())
                {
                    _logger.LogError("Tool object sent from client is null.");
                    return BadRequest("Tool object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid tool object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbTool = _repository.Tool.GetToolById(id);
                if (dbTool.IsEmptyObject())
                {
                    _logger.LogError($"Tool with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Tool.UpdateTool(dbTool, tool);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateTool action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTool(Guid id)
        {
            try
            {
                var tool = _repository.Tool.GetToolById(id);
                if (tool.IsEmptyObject())
                {
                    _logger.LogError($"Tool with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Tool.DeleteTool(tool);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteTool action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForTool(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetToolById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteTool), values: new {id}), "delete_tool", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateTool), values: new {id}), "update_tool", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForTools(LinkCollectionWrapper<Entity> toolsWrapper)
        {
            toolsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetTools), values: new { }), "self", "GET"));

            return toolsWrapper;
        }
    }
}
