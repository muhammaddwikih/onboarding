using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using onboarding.bll.Kafka;
using onboarding.bll.Test.Common;
using onboarding.dal.Model;
using onboarding.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace onboarding.bll.Test
{
    public class MovieServiceTest
    {
        private IEnumerable<MovieModel> movie;
        private Mock<IkafkaSender> kafka;
        private Mock<IUnitOfWork> uow;
        private IEnumerable<National> national;

        public MovieServiceTest()
        {
            movie = CommonHelper.LoadDataFromFile<IEnumerable<MovieModel>>(@"MockData\Movie.json");
            national = CommonHelper.LoadDataFromFile<IEnumerable<National>>(@"MockData\National.json");
            uow = MockUnitOfWork();
            kafka = MockKafka();
            
        }

        /*private Mock<IConfiguration> MockConfig()
        {
            throw new NotImplementedException();
        }*/

        private MovieService CreateMovieService()
        {
            return new MovieService(uow.Object, kafka.Object);
        }

        private Mock<IkafkaSender> MockKafka()
        {
            var mockKafka = new Mock<IkafkaSender>();

            mockKafka.Setup(x => x.SendAsync(It.IsAny<String>(), It.IsAny<Object>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return mockKafka;
        }

        private Mock<IUnitOfWork> MockUnitOfWork()
        {
            var moviesQueryable = movie.AsQueryable().BuildMock().Object;

            var nationQueryable = national.AsQueryable().BuildMock().Object;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.MovieRepository.GetAll())
                .Returns(moviesQueryable);

            mockUnitOfWork
                .Setup(x => x.NationalRepository.GetAll())
                .Returns(nationQueryable);

            mockUnitOfWork
                .Setup(u => u.MovieRepository.IsExist(It.IsAny<Expression<Func<MovieModel, bool>>>()))
                .Returns((Expression<Func<MovieModel, bool>> condition) => moviesQueryable.Any(condition));

            mockUnitOfWork
               .Setup(u => u.MovieRepository.GetSingleAsync(It.IsAny<Expression<Func<MovieModel, bool>>>()))
               .ReturnsAsync((Expression<Func<MovieModel, bool>> condition) => moviesQueryable.FirstOrDefault(condition));

            mockUnitOfWork
               .Setup(u => u.MovieRepository.AddAsync(It.IsAny<MovieModel>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((MovieModel author, CancellationToken token) =>
               {
                   author.Id = Guid.NewGuid();
                   return author;
               });

            mockUnitOfWork
                .Setup(u => u.MovieRepository.Delete(It.IsAny<Expression<Func<MovieModel, bool>>>()))
                .Verifiable();


            mockUnitOfWork
                .Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return mockUnitOfWork;
        }

        [Fact]
        public void GetAll()
        {
            var expected = movie;

            var svc = CreateMovieService();

            // act
            var actual = svc.GetAll();

            // assert      
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(3)]
        public void GetMovieByIdTest(int imdbid)
        {
            var expected = movie.First(x => x.ImdbId == imdbid);

            var svc = CreateMovieService();

            var actual = svc.GetMovieById(imdbid);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateMovieTest()
        {
            var expected = new MovieModel
            {
                Genre = "genre",
                ImdbId = 6,
                Title = "title",
                NationalId = Guid.Parse("007957c3-1b7a-11ec-840c-d45d646e1fa9")
            };

            var svc = CreateMovieService();

            Func<Task> act = async () => { await svc.CreateMovie(expected); };

            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("49CB0862-E990-4209-0667-08D9893984D7", "TitleEdit", "GenreEdit")]
        public void EditMovieTest(string id, string title, string genre)
        {
            var expected = new MovieModel
            {
                Id = Guid.Parse(id),
                Title = title,
                Genre = genre,
                NationalId = Guid.Parse("007957c3-1b7a-11ec-840c-d45d646e1fa9")
            };

            var svc = CreateMovieService();

            var actual = svc.EditMovie(Guid.Parse(id), expected);

            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Theory]
        [InlineData("stringEdit")]
        public async Task DeleteMovieTest(string title)
        {
            var svc = CreateMovieService();

            Func<Task> act = async () => { await svc.DeleteMovie(title); };

            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
