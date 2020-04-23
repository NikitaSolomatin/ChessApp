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
    public class CreateGameTests
    {
        private static (Mock<IUOW> uow, Mock<IGameRepository> gameRepo,
            Dictionary<int, Game> dbCollection) GetMocks()
        {
            var uow = new Mock<IUOW>(MockBehavior.Strict);
            var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
            var dbCollection = new Dictionary<int, Game>
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

            uow.SetupGet(e => e.Games).Returns(gameRepo.Object);
            uow.Setup(e => e.CommitAsync()).ReturnsAsync(0);

            gameRepo.Setup(e => e.AddAsync(It.IsAny<Game>()))
                     .Callback((Game newGame) => { dbCollection.Add(newGame.Id, newGame); })
                     .Returns((Game _) => Task.CompletedTask);

            return (uow, gameRepo, dbCollection);
        }

        [Test]
        public async Task CreateGame_FullInfo_Success()
        {
            // Arrange
            var (uow, gameRepo, dbCollection) = GetMocks();
            var service = new GameService(uow.Object);
            var game = new Game
            {
                Id = 28,
                Result = "New Result",
                Memo = "New Memo"
            };

            // Act
            await service.CreateGame(game);

            // Assert
            Assert.IsTrue(dbCollection.ContainsKey(game.Id));
        }

        [Test]
        public void CreateGame_NullObject_NullReferenceException()
        {
            // Arrange
            var (uow, gameRepo, dbCollection) = GetMocks();
            var service = new GameService(uow.Object);

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreateGame(null));
        }
    }
}
