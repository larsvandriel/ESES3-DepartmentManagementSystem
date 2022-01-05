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
    public class EquipmentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public EquipmentController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetEquipments([FromQuery] EquipmentParameters equipmentParameters)
        {
            try
            {
                var equipments = _repository.Equipment.GetAllEquipments(equipmentParameters);

                var metadata = new
                {
                    equipments.TotalCount,
                    equipments.PageSize,
                    equipments.CurrentPage,
                    equipments.TotalPages,
                    equipments.HasNext,
                    equipments.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {equipments.Count} equipments from database.");

                var shapedEquipments = equipments.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedEquipments);
                }

                for (var index = 0; index < equipments.Count; index++)
                {
                    var brandLinks = CreateLinksForEquipment(equipments[index].Id, equipmentParameters.Fields);
                    shapedEquipments[index].Add("Links", brandLinks);
                }

                var equipmentsWrapper = new LinkCollectionWrapper<Entity>(shapedEquipments);

                return Ok(CreateLinksForEquipments(equipmentsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllEquipments action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "EquipmentById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetEquipmentById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var equipment = _repository.Equipment.GetEquipmentById(id, fields);

                if (equipment.Id == Guid.Empty)
                {
                    _logger.LogError($"Equipment with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped equipment with id: {id}");
                    return Ok(equipment.Entity);
                }

                equipment.Entity.Add("Links", CreateLinksForEquipment(equipment.Id, fields));

                return Ok(equipment.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetEquipmentById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateEquipment([FromBody] Equipment equipment)
        {
            try
            {
                if (equipment.IsObjectNull())
                {
                    _logger.LogError("Equipment object sent from client is null.");
                    return BadRequest("Equipment object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid equipment object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Equipment.CreateEquipment(equipment);
                _repository.Save();

                return CreatedAtRoute("EquipmentById", new { id = equipment.Id }, equipment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateEquipment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEquipment(Guid id, [FromBody] Equipment equipment)
        {
            try
            {
                if (equipment.IsObjectNull())
                {
                    _logger.LogError("Equipment object sent from client is null.");
                    return BadRequest("Equipment object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid equipment object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbEquipment = _repository.Equipment.GetEquipmentById(id);
                if (dbEquipment.IsEmptyObject())
                {
                    _logger.LogError($"Equipment with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Equipment.UpdateEquipment(dbEquipment, equipment);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateEquipment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEquipment(Guid id)
        {
            try
            {
                var equipment = _repository.Equipment.GetEquipmentById(id);
                if (equipment.IsEmptyObject())
                {
                    _logger.LogError($"Equipment with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Equipment.DeleteEquipment(equipment);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteEquipment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForEquipment(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetEquipmentById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteEquipment), values: new {id}), "delete_equipment", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateEquipment), values: new {id}), "update_equipment", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForEquipments(LinkCollectionWrapper<Entity> equipmentsWrapper)
        {
            equipmentsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetEquipments), values: new { }), "self", "GET"));

            return equipmentsWrapper;
        }
    }
}
