using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MusicApp.Core;
using MusicApp.Core.Models;
using MusicApp.Core.Repositories;
using NUnit.Framework;

namespace MusicApp.BLL.Tests
{
    [TestFixture]
    public class CreateMusicTests
    {
        private static (Mock<IUnitOfWork> unitOfWork, Mock<IMusicRepository> musicRepo, Dictionary<int, Music> dbCollection) GetMocks()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var musicRepo = new Mock<IMusicRepository>(MockBehavior.Strict);
            var dbCollection = new Dictionary<int, Music>
            {
                [26] = new Music
                {
                    Id = 26,
                    ArtistId = 26,
                    Name = "Delete Track"
                },
                [27] = new Music
                {
                    Id = 27,
                    ArtistId = 27,
                    Name = "Track"
                }
            };

            unitOfWork.SetupGet(e => e.Musics).Returns(musicRepo.Object);
            unitOfWork.Setup(e => e.CommitAsync()).ReturnsAsync(0);

            musicRepo.Setup(e => e.AddAsync(It.IsAny<Music>()))
                     .Callback((Music newMusic) => { dbCollection.Add(newMusic.Id, newMusic); })
                     .Returns((Music _) => Task.CompletedTask);

            return (unitOfWork, musicRepo, dbCollection);
        }

        [Test]
        public async Task CreateMusic_FullInfo_Success()
        {
            // Arrange
            var (unitOfWork, musicRepo, dbCollection) = GetMocks();
            var service = new MusicService(unitOfWork.Object);
            var music = new Music
            {
                Id = 28,
                Name = "New Track"
            };

            // Act
            await service.CreateMusic(music);

            // Assert
            Assert.IsTrue(dbCollection.ContainsKey(music.Id));
        }

        [Test]
        public void CreateMusic_NullObject_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, musicRepo, dbCollection) = GetMocks();
            var service = new MusicService(unitOfWork.Object);

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreateMusic(null));
        }
    }
}