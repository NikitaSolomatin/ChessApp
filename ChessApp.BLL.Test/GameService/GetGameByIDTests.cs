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
    public class GetGameByIdTests
    {
        private static (Mock<IUOW> uow, Mock<IGameRepository> gameRepo, Dictionary<int, Game> dbCollectionGame) GetMocks()
        {
            var uow = new Mock<IUOW>(MockBehavior.Strict);
            var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
            var playerRepo = new Mock<IPlayerRepository>(MockBehavior.Strict);
            var dbCollectionGame = new Dictionary<int, Game>
            {
                [26] = new Game
                {
                    Id = 26,
                    PlayerId = 26,
                    Result = "Delete Result"
                },
                [27] = new Game
                {
                    Id = 27,
                    PlayerId = 27,
                    Result = "Result"
                }
            };

            var dbCollectionPlayers = new Dictionary<int, Player>
            {
                [26] = new Player
                {
                    Id = 26,
                    FullName = "Name"
                },
                [27] = new Player
                {
                    Id = 27,
                    FullName = "Other Name"
                }
            };

            uow.SetupGet(e => e.Games).Returns(gameRepo.Object);
            uow.SetupGet(e => e.Players).Returns(playerRepo.Object);
            uow.Setup(e => e.CommitAsync()).ReturnsAsync(0);

            gameRepo.Setup(e => e.GetWithPlayerByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((int id) => dbCollectionGame[id]);

            playerRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionPlayers.ContainsKey(id));

            return (uow, gameRepo, dbCollectionGame);
        }

        [Test]
        public async Task GetGameById_ItemExists_Success()
        {
            // Arrange
            var (uow, gameRepo, dbCollectionGame) = GetMocks();
            var service = new GameService(uow.Object);

            // Act
            var game = await service.GetGameById(27);

            // Assert
            Assert.AreEqual(game, dbCollectionGame[27]);
        }

        [Test]
        public void GetGameById_ItemDoesNotExists_KeyNotFoundException()
        {
            // Arrange
            var (uow, gameRepo, dbCollectionGame) = GetMocks();
            var service = new GameService(uow.Object);

            // Act + Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetGameById(0));
        }
    }
}