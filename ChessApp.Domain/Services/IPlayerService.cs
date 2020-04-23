using System.Collections.Generic;
using System.Threading.Tasks;
using ChessApp.Domain.Models;

namespace ChessApp.Domain.Services
{
    public interface IPlayerService
    {
        Task<Player> CreatePlayer(Player newPlayer);
        Task<Player> GetPlayerById(int id);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task UpdatePlayer(int id, Player player);
        Task DeletePlayer(Player player);
    }
}