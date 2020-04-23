using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ChessApp.Domain;
using ChessApp.Domain.Models;
using ChessApp.Domain.Services;

namespace ChessApp.BLL
{
    public class GameService : IGameService
    {
        private readonly IUOW _uow;

        public GameService(IUOW uow)
        {
            _uow = uow;
        }

        public async Task<Game> CreateGame(Game newGame)
        {
            if (newGame is null)
                throw new NullReferenceException();

            await _uow.Games.AddAsync(newGame);
            await _uow.CommitAsync();







            return newGame;
        }

        public async Task<Game> GetGameById(int id)
        {
            return await _uow.Games.GetWithPlayerByIdAsync(id);
        }

        public async Task<IEnumerable<Game>> GetGamesByPlayerId(int artistId)
        {
            return await _uow.Games.GetAllWithPlayerByPlayerIdAsync(artistId);
        }

        public async Task<IEnumerable<Game>> GetAllWithPlayer()
        {
            return await _uow.Games.GetAllWithPlayerAsync();
        }

        public async Task UpdateGame(int id, Game game)
        {
            if (!await _uow.Games.IsExists(id))
                throw new NullReferenceException();

            if (game.Result.Length <= 0 || game.Result.Length > 50 
                || game.Memo.Length <= 0 || game.Memo.Length >= 100
                || game.PlayerId <= 0)
                throw new InvalidDataException();

            var gameToBeUpdated = await GetGameById(id);
            gameToBeUpdated.Result = game.Result;
            gameToBeUpdated.Memo = game.Memo;
            gameToBeUpdated.PlayerId = game.PlayerId;

            await _uow.CommitAsync();
        }

        public async Task DeleteGame(Game game)
        {
            if (!(await _uow.Games.IsExists(game.Id)))
                throw new NullReferenceException();

            _uow.Games.Remove(game);

            await _uow.CommitAsync();
        }
    }
}