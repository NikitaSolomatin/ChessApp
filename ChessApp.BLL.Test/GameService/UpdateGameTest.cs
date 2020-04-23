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
    public class UpdateGameTests
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
            gameRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionGame.ContainsKey(id));

            playerRepo.Setup(e => e.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionPlayers[id]);
            playerRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionPlayers.ContainsKey(id));

            return (uow, gameRepo, dbCollectionGame);
        }

        [Test]
        public async Task UpdateGame_FullInfo_Success()
        {
            // Arrange
            var (uow, gameRepo, dbCollectionGame) = GetMocks();
            var service = new GameService(uow.Object);
            var game = new Game
            {
                PlayerId = 27,
                Result = "New Result"
            };

            // Act
            await service.UpdateGame(27, game);

            // Assert
            Assert.AreEqual((await uow.Object.Games.GetWithPlayerByIdAsync(27)).Result, game.Result);
        }

        [Test]
        public void UpdateGame_EmptyFullName_InvalidDataException()
        {
            // Arrange
            var (uow, gameRepo, dbCollectionGame) = GetMocks();
            var service = new GameService(uow.Object);
            var game = new Game()
            {
                Result = ""
            };

            // Act + Assert
            Assert.ThrowsAsync<InvalidDataException>(async () => await service.UpdateGame(27, game));
        }

        [Test]
        public void UpdateGame_NoItemForUpdate_NullReferenceException()
        {
            // Arrange
            var (uow, gameRepo, dbCollectionGame) = GetMocks();
            var service = new GameService(uow.Object);
            var game = new Game()
            {
                Result = "Update Track"
            };

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateGame(0, game));
        }
    }
}