using System.Collections.Generic;
using System.Threading.Tasks;
using ChessApp.Domain.Models;

namespace ChessApp.Domain.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<Game> GetWithPlayerByIdAsync(int id); 
        Task<IEnumerable<Game>> GetAllWithPlayerAsync();
        Task<IEnumerable<Game>> GetAllWithPlayerByPlayerIdAsync(int playerId);
        Task<bool> IsExists(int id);
    }
}