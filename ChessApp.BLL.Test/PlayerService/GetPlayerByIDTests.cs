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
    public class GetPlayerByIdTests
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

            return (uow, playerRepo, dbCollection);
        }

        [Test]
        public async Task GetPlayerById_ItemExists_Success()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);

            // Act
            var player = await service.GetPlayerById(27);

            // Assert
            Assert.AreEqual(player, dbCollection[27]);
        }

        [Test]
        public void GetPlayerById_ItemDoesNotExists_KeyNotFoundException()
        {
            // Arrange
            var (uow, playerRepo, dbCollection) = GetMocks();
            var service = new PlayerService(uow.Object);

            // Act + Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetPlayerById(0));
        }
    }
}