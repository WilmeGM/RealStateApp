using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Agents.Commands.ChangeAgentStatus;
using RealStateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealStateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using RealStateApp.Core.Application.Features.Agents.Queries.GetAgentProperties;
using Microsoft.AspNetCore.Authorization;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AgentController : BaseApiController
    {
        /// <summary>
        /// Get a list of all agents.
        /// </summary>
        /// <returns>List of agents.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var agents = await Mediator.Send(new GetAllAgentsQuery());
                if (!agents.Any()) return NoContent();

                return Ok(agents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get details of an agent by ID.
        /// </summary>
        /// <param name="id">Agent ID.</param>
        /// <returns>Details of the agent.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var agent = await Mediator.Send(new GetAgentByIdQuery { Id = id });
                if (agent == null) return NotFound();

                return Ok(agent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get properties of an agent by agent ID.
        /// </summary>
        /// <param name="id">Agent ID.</param>
        /// <returns>List of properties managed by the agent.</returns>
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}/properties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentProperties(string id)
        {
            try
            {
                var properties = await Mediator.Send(new GetAgentPropertiesQuery { AgentId = id });
                if (properties.Count == 0) return NoContent();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Change the status (active/inactive) of an agent.
        /// </summary>
        /// <param name="id">Agent ID.</param>
        /// <param name="isActive">New status (true for active, false for inactive).</param>
        /// <returns>No content if successful.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/change-status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] bool isActive)
        {
            try
            {
                var result = await Mediator.Send(new ChangeAgentStatusCommand { Id = id, IsActive = isActive });
                if (!result) return BadRequest("Unable to change status.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
