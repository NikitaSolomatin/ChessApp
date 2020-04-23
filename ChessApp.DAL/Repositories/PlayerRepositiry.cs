using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChessApp.Domain.Models;
using ChessApp.Domain.Repositories;

namespace ChessApp.DAL.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(ChessAppDbContext context) : base(context) { }

        public async Task<Player> GetWithGamesByIdAsync(int id)
        {
            return await MyGameDbContext.Players.Include(a => a.Games)
                                                 .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Player>> GetAllWithGamesAsync()
        {
            return await MyGameDbContext.Players.Include(a => a.Games)
                                                 .ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await GetByIdAsync(id) is { };
        }

        private ChessAppDbContext MyGameDbContext => Context as ChessAppDbContext;
    }
}