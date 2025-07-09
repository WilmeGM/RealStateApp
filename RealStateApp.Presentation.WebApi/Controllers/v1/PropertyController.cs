using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyByCode;
using RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PropertyController : BaseApiController
    {
        /// <summary>
        /// Get all properties.
        /// </summary>
        /// <returns>List of all properties in the system.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var properties = await Mediator.Send(new GetAllPropertiesQuery());
                if (!properties.Any()) return NoContent();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a property by its ID.
        /// </summary>
        /// <param name="id">The ID of the property.</param>
        /// <returns>Details of the property.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var property = await Mediator.Send(new GetPropertyByIdQuery { Id = id });
                if (property == null) return NotFound();

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a property by its unique code.
        /// </summary>
        /// <param name="code">The unique code of the property.</param>
        /// <returns>Details of the property.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("by-code/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var property = await Mediator.Send(new GetPropertyByCodeQuery { Code = code });
                if (property == null) return NotFound();

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
