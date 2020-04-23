using System.Threading.Tasks;
using ChessApp.Domain;
using ChessApp.Domain.Repositories;
using ChessApp.DAL.Repositories;

namespace ChessApp.DAL
{
    public class UOW : IUOW
    {
        private readonly ChessAppDbContext _context;
        private GameRepository _gameRepository;
        private PlayerRepository _playerRepository;

        public UOW(ChessAppDbContext context)
        {
            _context = context;
        }

        public IGameRepository Games => _gameRepository ??= new GameRepository(_context);

        public IPlayerRepository Players => _playerRepository ??= new PlayerRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}