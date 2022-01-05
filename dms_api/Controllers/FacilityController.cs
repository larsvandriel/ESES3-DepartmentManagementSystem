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
    public class FacilityController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public FacilityController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetFacilities([FromQuery] FacilityParameters facilityParameters)
        {
            try
            {
                var facilities = _repository.Facility.GetAllFacilities(facilityParameters);

                var metadata = new
                {
                    facilities.TotalCount,
                    facilities.PageSize,
                    facilities.CurrentPage,
                    facilities.TotalPages,
                    facilities.HasNext,
                    facilities.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {facilities.Count} facilities from database.");

                var shapedFacilities = facilities.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedFacilities);
                }

                for (var index = 0; index < facilities.Count; index++)
                {
                    var brandLinks = CreateLinksForFacility(facilities[index].Id, facilityParameters.Fields);
                    shapedFacilities[index].Add("Links", brandLinks);
                }

                var facilitiesWrapper = new LinkCollectionWrapper<Entity>(shapedFacilities);

                return Ok(CreateLinksForFacilities(facilitiesWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllFacilities action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "FacilityById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetFacilityById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var facility = _repository.Facility.GetFacilityById(id, fields);

                if (facility.Id == Guid.Empty)
                {
                    _logger.LogError($"Facility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped facility with id: {id}");
                    return Ok(facility.Entity);
                }

                facility.Entity.Add("Links", CreateLinksForFacility(facility.Id, fields));

                return Ok(facility.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetFacilityById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateFacility([FromBody] Facility facility)
        {
            try
            {
                if (facility.IsObjectNull())
                {
                    _logger.LogError("Facility object sent from client is null.");
                    return BadRequest("Facility object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid facility object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Facility.CreateFacility(facility);
                _repository.Save();

                return CreatedAtRoute("FacilityById", new { id = facility.Id }, facility);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateFacility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFacility(Guid id, [FromBody] Facility facility)
        {
            try
            {
                if (facility.IsObjectNull())
                {
                    _logger.LogError("Facility object sent from client is null.");
                    return BadRequest("Facility object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid facility object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbFacility = _repository.Facility.GetFacilityById(id);
                if (dbFacility.IsEmptyObject())
                {
                    _logger.LogError($"Facility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Facility.UpdateFacility(dbFacility, facility);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateFacility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFacility(Guid id)
        {
            try
            {
                var facility = _repository.Facility.GetFacilityById(id);
                if (facility.IsEmptyObject())
                {
                    _logger.LogError($"Facility with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Facility.DeleteFacility(facility);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteFacility action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForFacility(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetFacilityById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteFacility), values: new {id}), "delete_facility", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateFacility), values: new {id}), "update_facility", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForFacilities(LinkCollectionWrapper<Entity> facilitiesWrapper)
        {
            facilitiesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetFacilities), values: new { }), "self", "GET"));

            return facilitiesWrapper;
        }
    }
}
