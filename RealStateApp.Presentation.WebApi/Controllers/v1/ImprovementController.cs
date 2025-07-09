using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Improvemets.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.Improvemets.Commands.DeleteImprovementById;
using RealStateApp.Core.Application.Features.Improvemets.Commands.UpdateImprovement;
using RealStateApp.Core.Application.Features.Improvemets.Queries.GetAllImprovements;
using RealStateApp.Core.Application.Features.Improvemets.Queries.GetImprovementById;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ImprovementController : BaseApiController
    {
        /// <summary>
        /// Create a new improvement.
        /// </summary>
        /// <param name="command">The improvement data.</param>
        /// <returns>A boolean indicating success.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateImprovementCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await Mediator.Send(command);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing improvement.
        /// </summary>
        /// <param name="id">The improvement ID.</param>
        /// <param name="command">The updated improvement data.</param>
        /// <returns>The updated improvement data.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateImprovementCommand command)
        {
            try
            {
                if (!ModelState.IsValid || id != command.Id)
                {
                    return BadRequest();
                }

                return Ok(await Mediator.Send(command));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all improvements.
        /// </summary>
        /// <returns>A list of improvements.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllImprovementQuery());
                if (!result.Any())
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get an improvement by ID.
        /// </summary>
        /// <param name="id">The improvement ID.</param>
        /// <returns>The improvement data.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetImprovementByIdQuery { Id = id }));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an improvement by ID.
        /// </summary>
        /// <param name="id">The improvement ID.</param>
        /// <returns>A boolean indicating success.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });
                return NoContent();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
