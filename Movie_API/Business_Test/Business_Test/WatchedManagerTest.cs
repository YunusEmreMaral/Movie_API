using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.JSInterop;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Business_Test.Business_Test
{
    [TestCaseOrderer("Business_Test.PriorityOrderer", "Business_Test")]
    public class WatchedManagerTest: TestBed<BusinessLayerStartupFixture>
    {

        private readonly IWatchedService _watchedService;
        public static int watchedId;

        public WatchedManagerTest(ITestOutputHelper testOutputHelper, BusinessLayerStartupFixture fixture) : base(testOutputHelper, fixture)
        {
            _watchedService = fixture.GetService<IWatchedService>(_testOutputHelper);
        }
        [Fact, TestPriority(1)]
        public void AddWatchedMovieById_Returns_1_When_Movie_Not_Found()
        {
            // Arrange
             int  nonExistMovieId = 9999;
            // Act
            var result = _watchedService.AddWatchedMovieById(1, nonExistMovieId);

            // Assert
            Assert.Equal((null, 1), result);
        }

        [Fact, TestPriority(3)]
        public void AddWatchedMovieById_Returns_2_When_Movie_Already_Watched_By_User()
        {
            // Arrange
            

            // Act
            var result = _watchedService.AddWatchedMovieById(1, 10);

            // Assert
            Assert.Equal((null, 2), result);
        }

        [Fact, TestPriority(2)]
        public void AddWatchedMovieById_Returns_3_When_Movie_Added_To_Watched_Successfully()
        {
            // Arrange


            // Act
            var result = _watchedService.AddWatchedMovieById(1, 10);
            
            // Assert
            Assert.Equal((result.Item1, 3), result);

            watchedId = result.Item1.WatchedID;

            
        }

        [Fact, TestPriority(4)]
        public void GetWatchedMovieNamesByUser_Returns_Movie_Names_Successfully()
        {
            // Arrange
            var userId = 1;
            var expectedMovieNames = new List<string> { "Seven", "Schindler's List", "1917" };

            // Act
            var result = _watchedService.GetWatchedMovieNamesByUser(userId);

            // Assert
            Assert.Equal(expectedMovieNames, result);

            _watchedService.DeleteWatched(watchedId);// en son siliyorum veri tabanında kalmasın diye  sıra düzgün ilerler ise olur testler
        }
    }
}
