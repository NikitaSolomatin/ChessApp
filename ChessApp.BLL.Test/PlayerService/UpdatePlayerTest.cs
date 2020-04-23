using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using ChessApp.Domain;
using ChessApp.Domain.Models;
using ChessApp.Domain.Repositories;
using NUnit.Framework;

namespace ChessApp.BLL.Tests
{
    [TestFixture]
    public class UpdatePlayerTests
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

            playerRepo.Setup(e => e.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection[id]);
            playerRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection.ContainsKey(id));

            return (uow, playerRepo, dbCollection);
        }

        [Test]
        public async Task UpdatePlayer_FullInfo_Success()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player
            {
                FullName = "New Name"
            };

            // Act
            await service.UpdatePlayer(27, player);

            // Assert
            Assert.AreEqual((await uow.Object.Players.GetByIdAsync(27)).FullName, player.FullName);
        }

        [Test]
        public void UpdatePlayer_EmptyFullName_InvalidDataException()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player()
            {
                FullName = ""
            };

            // Act + Assert
            Assert.ThrowsAsync<InvalidDataException>(async () => await service.UpdatePlayer(27, player));
        }

        [Test]
        public void UpdatePlayer_NoItemForUpdate_NullReferenceException()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);
            var player = new Player()
            {
                FullName = "Update Name"
            };

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdatePlayer(0, player));
        }
    }
}