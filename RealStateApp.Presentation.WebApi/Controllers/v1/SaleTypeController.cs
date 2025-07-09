using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SaleTypeController : BaseApiController
    {
        /// <summary>
        /// Create a new Sale Type
        /// </summary>
        /// <param name="command">Data for the Sale Type</param>
        /// <returns>201 Created if successful</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var result = await Mediator.Send(command);
                return result ? StatusCode(StatusCodes.Status201Created) : BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Update an existing Sale Type
        /// </summary>
        /// <param name="id">Sale Type ID</param>
        /// <param name="command">Updated data for the Sale Type</param>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid || id != command.Id)
                    return BadRequest();

                var updatedSaleType = await Mediator.Send(command);
                return Ok(updatedSaleType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Get all Sale Types
        /// </summary>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var saleTypes = await Mediator.Send(new GetAllSaleTypesQuery());
                if (!saleTypes.Any())
                    return NoContent();

                return Ok(saleTypes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Get a Sale Type by ID
        /// </summary>
        /// <param name="id">Sale Type ID</param>
        /// <returns>200 OK if successful</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var saleType = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });
                if (saleType == null)
                    return NotFound();

                return Ok(saleType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Delete a Sale Type by ID
        /// </summary>
        /// <param name="id">Sale Type ID</param>
        /// <returns>204 No Content if successful</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteSaleTypeCommand { Id = id });
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
