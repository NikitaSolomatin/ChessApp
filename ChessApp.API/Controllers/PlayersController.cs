using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ChessApp.API.Resources;
using ChessApp.Domain.Models;
using ChessApp.Domain.Services;

namespace ChessApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<SavePlayerResource> _validator;

        public PlayersController(IPlayerService playerService, IMapper mapper, AbstractValidator<SavePlayerResource> validator)
        {
            _mapper = mapper;
            _playerService = playerService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerResource>>> GetAllPlayers()
        {
            var players = await _playerService.GetAllPlayers();
            var playerResources = _mapper.Map<IEnumerable<Player>, IEnumerable<PlayerResource>>(players);

            return Ok(playerResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerResource>> GetPlayerById(int id)
        {
            var player = await _playerService.GetPlayerById(id);
            var playerResource = _mapper.Map<Player, PlayerResource>(player);

            return Ok(playerResource);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerResource>> CreatePlayer([FromBody] SavePlayerResource savePlayerResource)
        {
            var validationResult = await _validator.ValidateAsync(savePlayerResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var playerToCreate = _mapper.Map<SavePlayerResource, Player>(savePlayerResource);
            var newPlayer = await _playerService.CreatePlayer(playerToCreate);
            var playerResource = _mapper.Map<Player, PlayerResource>(newPlayer);

            return Ok(playerResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerResource>> UpdatePlayer(int id, [FromBody] SavePlayerResource savePlayerResource)
        {
            var validationResult = await _validator.ValidateAsync(savePlayerResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var player = _mapper.Map<SavePlayerResource, Player>(savePlayerResource);

            await _playerService.UpdatePlayer(id, player);

            var updatedPlayer = await _playerService.GetPlayerById(id);
            var updatedPlayerResource = _mapper.Map<Player, PlayerResource>(updatedPlayer);

            return Ok(updatedPlayerResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _playerService.GetPlayerById(id);

            await _playerService.DeletePlayer(player);

            return NoContent();
        }
    }
}