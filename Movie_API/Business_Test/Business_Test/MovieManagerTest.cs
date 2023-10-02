using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Business_Test.Business_Test
{

    [TestCaseOrderer("Business_Test.PriorityOrderer", "Business_Test")]
    public class MovieManagerTest : TestBed<BusinessLayerStartupFixture>
    {
        private readonly IMovieService _movieService;
        public static int newMovieId;


        public MovieManagerTest(ITestOutputHelper testOutputHelper, BusinessLayerStartupFixture fixture) : base(testOutputHelper, fixture)
        {
            _movieService = fixture.GetService<IMovieService>(_testOutputHelper);

        }



        [Fact, TestPriority(1)]
        public void AddMovie_Returns_Movie()
        {
            // Arrange
            var newMovie = new Movie
            {
                MovieName = "Test Movie",
                MovieRelaseYear = 2023,
                MovieDirector = "Test Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1

            };

            // Act
            var result = _movieService.AddMovie(newMovie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.MovieName);
            Assert.Equal(2023, result.MovieRelaseYear);
            Assert.Equal("Test Director", result.MovieDirector);
            Assert.False(result.IsWatched);
            Assert.Equal(120, result.MovieDuration);
            Assert.Equal(1, result.MovieGenreID);

            newMovieId = result.MovieID;
        }
        [Fact]


        public void AddMovie_Returns_Null_WhenMovieIsNull()
        {
            // Arrange
            Movie? nullMovie = null;

            // Act
            var result = _movieService.AddMovie(nullMovie);

            // Assert
            Assert.Null(result);
        }

        //---------------------------------------------

        [Fact, TestPriority(2)]
        public void GetMovieByID_Returns_CorrectMovie()
        {
            // Arrange zaten veriyi üstte aldım 

            // Act
            var retrievedMovie = _movieService.GetMovieByID(newMovieId);

            // Assert
            Assert.NotNull(retrievedMovie);
            Assert.Equal("Test Movie", retrievedMovie.MovieName);
            Assert.Equal(2023, retrievedMovie.MovieRelaseYear);
            Assert.Equal("Test Director", retrievedMovie.MovieDirector);
            Assert.False(retrievedMovie.IsWatched);
            Assert.Equal(120, retrievedMovie.MovieDuration);
            Assert.Equal(1, retrievedMovie.MovieGenreID);
        }
        [Fact, TestPriority(3)]
        public void WatchedMovie_Marks_Movie_As_Watched()
        {
            // Arrange üstteki filmi kullanıyoruz gene 


            // Act
            var watchedMovie = _movieService.WatchedMovie(newMovieId);

            // Assert
            Assert.NotNull(watchedMovie);
            Assert.True(watchedMovie.IsWatched);

        }
        [Fact, TestPriority(4)]
        public void DeleteMovie_Returns_DeletedMovie()
        {
            // Arrange



            // Act
            var deletedMovie = _movieService.DeleteMovieByID(newMovieId);

            // Assert
            Assert.NotNull(deletedMovie);
            Assert.Equal("Test Movie", deletedMovie.MovieName);
            Assert.Equal(2023, deletedMovie.MovieRelaseYear);
            Assert.Equal("Test Director", deletedMovie.MovieDirector);
            Assert.True(deletedMovie.IsWatched);
            Assert.Equal(1, deletedMovie.MovieGenreID);
        }
        [Fact]
        public void GetMovieByID_Returns_MovieNotFound()
        {
            // Arrange  
            int nonExistingMovieID = 999;

            // Act
            var result = _movieService.GetMovieByID(nonExistingMovieID);

            // Assert
            Assert.Null(result);
        }
        //--------------------------------------------

        [Fact]
        public void GetAllMovie_Returns_NotNull()
        {
            // Arrange degiştirilebilir çünkü sabit bir veri tabanı yok suanlık deneme amaclı 

            // Act
            var result = _movieService.GetAllFilms();

            // Assert
            Assert.NotNull(result);
        }
        ////--------------------------------------------

        
        [Fact]
        public void WatchedMovie_Marks_ReturnNull()
        {
            // Arrange 

            // Act
            var watchedMovie = _movieService.WatchedMovie(9999);

            // Assert
            Assert.Null(watchedMovie);

        }
        //-------------------------------------------------
       


        [Fact]
        public void DeleteMovie_Returns_Null_WhenMovieNotFound()
        {
            // Arrange
            int nonExistingMovieID = 999;

            // Act
            var result = _movieService.DeleteMovieByID(nonExistingMovieID);

            // Assert
            Assert.Null(result);
        }

    }


}


