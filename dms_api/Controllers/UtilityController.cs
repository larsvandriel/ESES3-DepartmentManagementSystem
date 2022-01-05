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
    public class UtilityController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public UtilityController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetUtilities([FromQuery] UtilityParameters utilityParameters)
        {
            try
            {
                var utilities = _repository.Utility.GetAllUtilities(utilityParameters);

                var metadata = new
                {
                    utilities.TotalCount,
                    utilities.PageSize,
                    utilities.CurrentPage,
                    utilities.TotalPages,
                    utilities.HasNext,
                    utilities.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {utilities.Count} utilities from database.");

                var shapedUtilities = utilities.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedUtilities);
                }

                for (var index = 0; index < utilities.Count; index++)
                {
                    var brandLinks = CreateLinksForUtility(utilities[index].Id, utilityParameters.Fields);
                    shapedUtilities[index].Add("Links", brandLinks);
                }

                var utilitiesWrapper = new LinkCollectionWrapper<Entity>(shapedUtilities);

                return Ok(CreateLinksForUtilities(utilitiesWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUtilities action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "UtilityById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetUtilityById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var utility = _repository.Utility.GetUtilityById(id, fields);

                if (utility.Id == Guid.Empty)
                {
                    _logger.LogError($"Utility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped utility with id: {id}");
                    return Ok(utility.Entity);
                }

                utility.Entity.Add("Links", CreateLinksForUtility(utility.Id, fields));

                return Ok(utility.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetUtilityById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateUtility([FromBody] Utility utility)
        {
            try
            {
                if (utility.IsObjectNull())
                {
                    _logger.LogError("Utility object sent from client is null.");
                    return BadRequest("Utility object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid utility object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Utility.CreateUtility(utility);
                _repository.Save();

                return CreatedAtRoute("UtilityById", new { id = utility.Id }, utility);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUtility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUtility(Guid id, [FromBody] Utility utility)
        {
            try
            {
                if (utility.IsObjectNull())
                {
                    _logger.LogError("Utility object sent from client is null.");
                    return BadRequest("Utility object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid utility object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbUtility = _repository.Utility.GetUtilityById(id);
                if (dbUtility.IsEmptyObject())
                {
                    _logger.LogError($"Utility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Utility.UpdateUtility(dbUtility, utility);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUtility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUtility(Guid id)
        {
            try
            {
                var utility = _repository.Utility.GetUtilityById(id);
                if (utility.IsEmptyObject())
                {
                    _logger.LogError($"Utility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Utility.DeleteUtility(utility);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUtility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForUtility(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUtilityById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteUtility), values: new {id}), "delete_utility", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateUtility), values: new {id}), "update_utility", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForUtilities(LinkCollectionWrapper<Entity> utilitiesWrapper)
        {
            utilitiesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUtilities), values: new { }), "self", "GET"));

            return utilitiesWrapper;
        }
    }
}
