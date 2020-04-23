using System.Collections.Generic;
using System.Threading.Tasks;
using ChessApp.Domain.Models;

namespace ChessApp.Domain.Services
{
    public interface IGameService
    {
        Task<Game> CreateGame(Game newGame);
        Task<Game> GetGameById(int id);



        Task<IEnumerable<Game>> GetAllWithPlayer();
        Task<IEnumerable<Game>> GetGamesByPlayerId(int playerId);





        Task UpdateGame(int id, Game game);
        Task DeleteGame(Game game);
    }
}