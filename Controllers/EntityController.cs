using TestProj.Data.Services;
using TestProj.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TestProj.Exceptions;
using TestProj.Data.Models;

namespace TestProj.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly EntityService _entityService;

        public EntityController(EntityService entityService)
        {
            _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
        }

        private IActionResult HandleException(Exception ex)
        {
            if (ex is GuidNotFoundException)
            {
                return BadRequest($"{ex.Message}");
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entity>>> GetAllEntitiesAsync()
        {
            var entities = await _entityService.GetAllEntitiesAsync();
            return Ok(entities);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetEntityByIdAsync(Guid guid)
        {
            try
            {
                var entity = await _entityService.GetEntityByIdAsync(guid);
                return entity != null ? Ok(entity) : NotFound();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEntityAsync([FromBody] EntityVM entityVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _entityService.AddEntityAsync(entityVM);
                return CreatedAtAction(nameof(GetEntityByIdAsync), new { guid = Guid.NewGuid() }, entityVM);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateEntityByIdAsync(Guid guid, [FromBody] EntityVM entityVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedEntity = await _entityService.UpdateEntityByIdAsync(guid, entityVM);
                return updatedEntity != null ? Ok(updatedEntity) : NotFound();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteEntityByIdAsync(Guid guid)
        {
            try
            {
                await _entityService.DeleteEntityByIdAsync(guid);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}