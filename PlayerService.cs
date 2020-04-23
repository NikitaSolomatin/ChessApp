using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ChessApp.Domain;
using ChessApp.Domain.Models;
using ChessApp.Domain.Services;

namespace ChessApp.BLL
{
    public class PlayerService : IPlayerService
    {
        private readonly IUOW _uow;

        public PlayerService(IUOW uow)
        {
            _uow = uow;
        }

        public async Task<Player> CreatePlayer(Player newPlayer)
        {
            if (newPlayer is null)
                throw new NullReferenceException();

            await _uow.Players.AddAsync(newPlayer);
            await _uow.CommitAsync();

            return newPlayer;
        }

        public async Task<Player> GetPlayerById(int id)
        {
            return await _uow.Players.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _uow.Players.GetAllAsync();
        }

        public async Task UpdatePlayer(int id, Player player)
        {
            if (!await _uow.Players.IsExists(id))
                throw new NullReferenceException();

            if (player.FullName.Length == 0 || player.FullName.Length > 50)
                throw new InvalidDataException();

            var playerToBeUpdated = await GetPlayerById(id);
            playerToBeUpdated.FullName = player.FullName;

            await _uow.CommitAsync();
        }

        public async Task DeletePlayer(Player player)
        {
            if (!await _uow.Players.IsExists(player.Id))
                throw new NullReferenceException();

            _uow.Players.Remove(player);

            await _uow.CommitAsync();
        }
    }
}