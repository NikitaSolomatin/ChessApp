using System;
using System.Threading.Tasks;
using ChessApp.Domain.Repositories;

namespace ChessApp.Domain
{
    public interface IUOW : IDisposable
    {
        IGameRepository Games { get; }
        IPlayerRepository Players { get; }
        Task<int> CommitAsync();
    }
}