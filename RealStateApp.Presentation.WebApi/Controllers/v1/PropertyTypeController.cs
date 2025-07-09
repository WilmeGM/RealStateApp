using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PropertyTypeController : BaseApiController
    {
        /// <summary>
        /// Create a new Property Type
        /// </summary>
        /// <param name="command">Property Type Data</param>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var result = await Mediator.Send(command);
                return result ? StatusCode(StatusCodes.Status201Created) : BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Update an existing Property Type
        /// </summary>
        /// <param name="id">Property Type ID</param>
        /// <param name="command">Updated data for the Property Type</param>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
            try
            {
                if (id != command.Id) return BadRequest();

                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Get all Property Types
        /// </summary>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin, Developer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllPropertyTypesQuery());
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Get a Property Type by ID
        /// </summary>
        /// <param name="id">Property Type ID</param>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id });
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Delete a Property Type
        /// </summary>
        /// <param name="id">Property Type ID</param>
        /// <returns>204 No Content if successful</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeletePropertyTypeCommand { Id = id });
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
