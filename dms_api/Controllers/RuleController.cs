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
    public class RuleController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public RuleController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetRules([FromQuery] RuleParameters ruleParameters)
        {
            try
            {
                var rules = _repository.Rule.GetAllRules(ruleParameters);

                var metadata = new
                {
                    rules.TotalCount,
                    rules.PageSize,
                    rules.CurrentPage,
                    rules.TotalPages,
                    rules.HasNext,
                    rules.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {rules.Count} rules from database.");

                var shapedRules = rules.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedRules);
                }

                for (var index = 0; index < rules.Count; index++)
                {
                    var brandLinks = CreateLinksForRule(rules[index].Id, ruleParameters.Fields);
                    shapedRules[index].Add("Links", brandLinks);
                }

                var rulesWrapper = new LinkCollectionWrapper<Entity>(shapedRules);

                return Ok(CreateLinksForRules(rulesWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllRules action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "RuleById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetRuleById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var rule = _repository.Rule.GetRuleById(id, fields);

                if (rule.Id == Guid.Empty)
                {
                    _logger.LogError($"Rule with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped rule with id: {id}");
                    return Ok(rule.Entity);
                }

                rule.Entity.Add("Links", CreateLinksForRule(rule.Id, fields));

                return Ok(rule.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetRuleById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateRule([FromBody] Rule rule)
        {
            try
            {
                if (rule.IsObjectNull())
                {
                    _logger.LogError("Rule object sent from client is null.");
                    return BadRequest("Rule object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid rule object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Rule.CreateRule(rule);
                _repository.Save();

                return CreatedAtRoute("RuleById", new { id = rule.Id }, rule);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateRule action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRule(Guid id, [FromBody] Rule rule)
        {
            try
            {
                if (rule.IsObjectNull())
                {
                    _logger.LogError("Rule object sent from client is null.");
                    return BadRequest("Rule object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid rule object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbRule = _repository.Rule.GetRuleById(id);
                if (dbRule.IsEmptyObject())
                {
                    _logger.LogError($"Rule with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Rule.UpdateRule(dbRule, rule);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateRule action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRule(Guid id)
        {
            try
            {
                var rule = _repository.Rule.GetRuleById(id);
                if (rule.IsEmptyObject())
                {
                    _logger.LogError($"Rule with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Rule.DeleteRule(rule);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteRule action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForRule(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetRuleById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteRule), values: new {id}), "delete_rule", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateRule), values: new {id}), "update_rule", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForRules(LinkCollectionWrapper<Entity> rulesWrapper)
        {
            rulesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetRules), values: new { }), "self", "GET"));

            return rulesWrapper;
        }
    }
}
