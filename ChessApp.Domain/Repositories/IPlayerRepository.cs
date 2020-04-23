using System.Collections.Generic;
using System.Threading.Tasks;
using ChessApp.Domain.Models;

namespace ChessApp.Domain.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<Player> GetWithGamesByIdAsync(int id);


        Task<IEnumerable<Player>> GetAllWithGamesAsync();


        Task<bool> IsExists(int id);
    }
}