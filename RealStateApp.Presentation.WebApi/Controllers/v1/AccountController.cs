using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces.Services;
using System.Net.Mime;

namespace RealStateApp.Presentation.WebApi.Controllers.v1
{
    /// <summary>
    /// API Controller for managing accounts.
    /// </summary>
    [ApiVersion("1.0")]
    public class AccountController(IAccountServiceForWebApi accountServiceForWebApi) : BaseApiController
    {
        private readonly IAccountServiceForWebApi _accountServiceForWebApi = accountServiceForWebApi;

        /// <summary>
        /// Login to the system and get a JWT token.
        /// </summary>
        /// <param name="request">The login request data.</param>
        /// <returns>JWT token and user information.</returns>
        /// <response code="200">Returns the JWT token and user information.</response>
        /// <response code="400">If the login request is invalid.</response>
        /// <response code="403">If the user is already authenticated.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (User.Identity.IsAuthenticated)
                return StatusCode(StatusCodes.Status403Forbidden, "You are already authenticated.");

            var response = await _accountServiceForWebApi.AuthenticateAsync(request);

            if (response.HasError)
            {
                if (response.ErrorMessage == "No puedes acceder como agente a la api")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You are not available for this resource.");
                }

                if (response.ErrorMessage == "No puedes acceder como cliente a la api")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You are not available for this resource.");
                }

                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }

        /// <summary>
        /// Register a new developer user.
        /// </summary>
        /// <param name="request">The developer user registration data.</param>
        /// <returns>Information about the newly created developer user.</returns>
        /// <response code="200">Returns the information of the newly created developer user.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("RegisterDeveloper")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateDevelopersUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterDeveloper([FromBody] CreateDevelopersUserRequest request)
        {
            var response = await _accountServiceForWebApi.RegisterDeveloperUserAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }

        /// <summary>
        /// Register a new administrator user.
        /// </summary>
        /// <param name="request">The administrator user registration data.</param>
        /// <returns>Information about the newly created administrator user.</returns>
        /// <response code="200">Returns the information of the newly created administrator user.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is unauthorized to perform this action.</response>
        /// <response code="403">If the user does not have the required role to perform this action.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterAdmin")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateAdminUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] CreateAdminUserRequest request)
        {
            var response = await _accountServiceForWebApi.RegisterAdminUserAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }
    }
}
