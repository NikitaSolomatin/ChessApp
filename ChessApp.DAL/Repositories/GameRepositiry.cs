using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChessApp.Domain.Models;
using ChessApp.Domain.Repositories;

namespace ChessApp.DAL.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(ChessAppDbContext context) : base(context) { }

        public async Task<Game> GetWithPlayerByIdAsync(int id)
        {
            return await MyGameDbContext.Games.Include(m => m.Player)
                                                .SingleOrDefaultAsync(m => m.Id == id); ;
        }

        public async Task<IEnumerable<Game>> GetAllWithPlayerAsync()
        {
            return await MyGameDbContext.Games.Include(m => m.Player)
                                                .ToListAsync();
        }


        public async Task<IEnumerable<Game>> GetAllWithPlayerByPlayerIdAsync(int playerId)
        {
            return await MyGameDbContext.Games.Include(m => m.Player)
                                                .Where(m => m.PlayerId == playerId)
                                                .ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            return await GetByIdAsync(id) is { };
        }

        private ChessAppDbContext MyGameDbContext => Context as ChessAppDbContext;
    }
}