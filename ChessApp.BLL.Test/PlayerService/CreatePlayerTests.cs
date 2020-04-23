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
    public class CreatePlayerTests
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

            playerRepo.Setup(e => e.AddAsync(It.IsAny<Player>()))
                      .Callback((Player newPlayer) => { dbCollection.Add(newPlayer.Id, newPlayer); })
                      .Returns((Player _) => Task.CompletedTask);

            return (uow, playerRepo, dbCollection);
        }

        [Test]
        public async Task CreatePlayer_FullInfo_Success()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player
            {
                Id = 28,
                FullName = "New Name"
            };

            // Act
            await service.CreatePlayer(player);

            // Assert
            Assert.IsTrue(dbCollection.ContainsKey(player.Id));
        }

        [Test]
        public void CreatePlayer_NullObject_NullReferenceException()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreatePlayer(null));
        }
    }
}