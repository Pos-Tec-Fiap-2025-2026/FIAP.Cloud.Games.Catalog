using Catalog.Application;
using Catalog.Core;
using Catalog.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerAbstractBase
    {
        private readonly IGameServices _gameServices;
        private const string Administrador = nameof(Roles.Administrador);
        private const string AllRolesAccess = $"{nameof(Roles.Usuario)},{nameof(Roles.Administrador)}";

        public CatalogController(IGameServices gameServices) => _gameServices = gameServices;

        [HttpGet("games")]
        public async Task<IActionResult> GetGames()
        {
            var result = await _gameServices.GetGamesAsync();

            return IsNullOrEmpty(result) ? NoContent() : Ok(result);
        }

        [HttpGet("games/{id}")]
        public async Task<IActionResult> GetGamesById(int id)
        {
            var result = await _gameServices.GetGamesByIdAsync(id);

            return IsNullOrEmpty(result) ? NoContent() : Ok(result);
        }

        [HttpPost("games/create")]
        [Authorize(Roles = Administrador)]
        public async Task<IActionResult> CreateGame([FromBody] GameInputDto gameInputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _gameServices.CreateAsync(gameInputDto);

            if (!result.IsSuccess)
            {
                if (IsNullOrEmpty(result))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Created(string.Empty, result.Value);
        }

        [HttpPatch("games/update/{id}")]
        [Authorize(Roles = Administrador)]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] GameUpdateDto gameUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _gameServices.UpdateAsync(id, gameUpdateDto);

            if (!result.IsSuccess)
            {
                if (IsNullOrEmpty(result))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Value);
        }

        [HttpPost("games/buy")]
        [Authorize(Roles = AllRolesAccess)]
        public async Task<IActionResult> BuyGame([FromBody] BuyRequest request)
        {
            var result = await _gameServices.BuyGameAsync(request);

            if (IsNullOrEmpty(result))
                return NotFound(result.Message);

            return Accepted(new { result.Value, result.Message });
        }

        [HttpDelete("games/delete/{id}")]
        [Authorize(Roles = Administrador)]
        public async Task<IActionResult> DisableGame(int id)
        {
            var result = await _gameServices.DisableGameAsync(id);

            if (IsNullOrEmpty(result))
                return NotFound(result.Message);

            return Ok(result.Value);
        }
    }
}