using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ChessApp.Domain;
using ChessApp.Domain.Models;
using ChessApp.Domain.Repositories;
using NUnit.Framework;

namespace ChessApp.BLL.Tests
{
    [TestFixture]
    public class DeletePlayerTests
    {
        private static (Mock<IUOW> uow, Mock<IPlayerRepository> playerRepo, Dictionary<int, Player> dbCollection) GetMocks()
        {
            var uow = new Mock<IUOW>(MockBehavior.Strict);
            var playerRepo = new Mock<IPlayerRepository>(MockBehavior.Strict);
            var dbCollection = new Dictionary<int, Player>
            {
                [26] = new Player
                {
                    Id = 26,
                    FullName = "Delete Name"
                },
                [27] = new Player
                {
                    Id = 27,
                    FullName = "Name"
                }
            };

            uow.SetupGet(e => e.Players).Returns(playerRepo.Object);
            uow.Setup(e => e.CommitAsync()).ReturnsAsync(0);

            playerRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection.ContainsKey(id));
            playerRepo.Setup(e => e.Remove(It.IsAny<Player>()))
                      .Callback((Player newPlayer) => { dbCollection.Remove(newPlayer.Id); });

            return (uow, playerRepo, dbCollection);
        }

        [Test]
        public async Task DeletePlayer_TargetItem_Success()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player
            {
                Id = 26,
                FullName = "Delete Name"
            };

            // Act
            await service.DeletePlayer(player);

            // Assert
            Assert.IsFalse(dbCollection.ContainsKey(26));
        }

        [Test]
        public void DeletePlayer_ItemDoesNotExists_NullReferenceException()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player
            {
                Id = 0,
                FullName = "Delete Name"
            };

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeletePlayer(player));
        }
    }
}