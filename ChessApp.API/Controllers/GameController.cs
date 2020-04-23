using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ChessApp.API.Resources;
using ChessApp.API.Validators;
using ChessApp.Domain.Models;
using ChessApp.Domain.Services;

namespace ChessApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GamesController(IGameService gameService, IMapper mapper)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameResource>> GetGameById(int id)
        {
            var game = await _gameService.GetGameById(id);
            var gameResource = _mapper.Map<Game, GameResource>(game);

            return Ok(gameResource);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameResource>>> GetAllGames()
        {
            var games = await _gameService.GetAllWithPlayer();
            var gameResources = _mapper.Map<IEnumerable<Game>, IEnumerable<GameResource>>(games);

            return Ok(gameResources);
        }

        [HttpPost]
        public async Task<ActionResult<GameResource>> CreateGame([FromBody] SaveGameResource saveGameResource)
        {
            var validator = new SaveGameResourceValidator();
            var validationResult = await validator.ValidateAsync(saveGameResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var gameToCreate = _mapper.Map<SaveGameResource, Game>(saveGameResource);
            var newGame = await _gameService.CreateGame(gameToCreate);
            var gameResource = _mapper.Map<Game, GameResource>(newGame);

            return Ok(gameResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameResource>> UpdateGame(int id, [FromBody] SaveGameResource saveGameResource)
        {
            var validator = new SaveGameResourceValidator();
            var validationResult = await validator.ValidateAsync(saveGameResource);

            var requestIsInvalid = id == 0 || !validationResult.IsValid;
            if (requestIsInvalid)
                return BadRequest(validationResult.Errors);

            var game = _mapper.Map<SaveGameResource, Game>(saveGameResource);

            await _gameService.UpdateGame(id, game);

            var updatedGame = await _gameService.GetGameById(id);
            var updatedGameResource = _mapper.Map<Game, GameResource>(updatedGame);

            return Ok(updatedGameResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (id == 0)
                return BadRequest();

            var game = await _gameService.GetGameById(id);
            if (game == null)
                return NotFound();

            await _gameService.DeleteGame(game);

            return NoContent();
        }
    }
}