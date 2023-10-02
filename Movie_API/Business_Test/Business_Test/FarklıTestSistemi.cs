using Business_Test.Business_Test;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Business_Test.Business_Test
{
    public class FarklıTestSistemi : TestBed<BusinessLayerStartupFixture>
    {
        private readonly IMovieService _movieService;
        private int movieId;
        private readonly IUserService _userService;
        private int userId;
        private readonly IWatchedService _watchedService;
        private int watchedId;

        public FarklıTestSistemi(ITestOutputHelper testOutputHelper, BusinessLayerStartupFixture fixture) : base(testOutputHelper, fixture)
        {
            _movieService = fixture.GetService<IMovieService>(_testOutputHelper);
            _userService = fixture.GetService<IUserService>(_testOutputHelper);
            _watchedService = fixture.GetService<IWatchedService>(_testOutputHelper);
        }
        [Fact]
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

            _movieService.DeleteMovieByID(newMovie.MovieID);
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

        [Fact]
        public void GetMovieByID_Returns_CorrectMovie()
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

            var result = _movieService.AddMovie(newMovie);

            // Act
            var retrievedMovie = _movieService.GetMovieByID(result.MovieID);

            // Assert
            Assert.NotNull(retrievedMovie);
            Assert.Equal("Test Movie", retrievedMovie.MovieName);
            Assert.Equal(2023, retrievedMovie.MovieRelaseYear);
            Assert.Equal("Test Director", retrievedMovie.MovieDirector);
            Assert.False(retrievedMovie.IsWatched);
            Assert.Equal(120, retrievedMovie.MovieDuration);
            Assert.Equal(1, retrievedMovie.MovieGenreID);
            _movieService.DeleteMovieByID(result.MovieID);
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
        //--------------------------------------------

        [Fact]
        public void WatchedMovie_Marks_Movie_As_Watched()
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

            var result = _movieService.AddMovie(newMovie);


            // Act
            var watchedMovie = _movieService.WatchedMovie(result.MovieID);

            // Assert
            Assert.NotNull(watchedMovie);
            Assert.True(watchedMovie.IsWatched);

            _movieService.DeleteMovieByID(result.MovieID);

        }
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
        public void DeleteMovie_Returns_DeletedMovie()
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

            var result = _movieService.AddMovie(newMovie);

            // Act
            var deletedMovie = _movieService.DeleteMovieByID(result.MovieID);

            // Assert
            Assert.NotNull(deletedMovie);
            Assert.Equal("Test Movie", deletedMovie.MovieName);
            Assert.Equal(2023, deletedMovie.MovieRelaseYear);
            Assert.Equal("Test Director", deletedMovie.MovieDirector);
            Assert.False(deletedMovie.IsWatched);
            Assert.Equal(1, deletedMovie.MovieGenreID);
            _movieService.DeleteMovieByID(result.MovieID);
        }


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
        [Fact]
        public void AddUser_Returns_User()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            // Act
            var result = _userService.AddUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("deneme1", result.UserName);
            Assert.Equal("password123", result.UserPassword);
            Assert.Equal("Test User", result.UserFullName);

            _userService.DeleteUser(result.UserID);

        }
        [Fact]
        public void AddUser_Returns_Null_WhenUserAlreadyExists()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };
            _userService.AddUser(newUser);

            // Act
            var newUser2 = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };
            var result = _userService.AddUser(newUser2);

            // Assert
            Assert.Null(result);
            _userService.DeleteUser(newUser.UserID);

        }
        [Fact]
        public void AddUser_Returns_Null()
        {
            // Arrange
            User? nullUser = null;

            if (nullUser== null) {
            // Act
            var result = _userService.AddUser(nullUser);
            
            
            // Assert
            Assert.Null(result);
            }
    
        }
        //-------------------------------------------------------
        [Fact]
        public void GetUserById_Returns_CorrectUser()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            _userService.AddUser(newUser);


            // Act
            var result = _userService.GetUserById(newUser.UserID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("deneme1", result.UserName);
            Assert.Equal("password123", result.UserPassword);
            Assert.Equal("Test User", result.UserFullName);
            _userService.DeleteUser(newUser.UserID);

        }
        public void GetUserById_Returns_NonExistUser()
        {
            // Arrange

            int nonExistingUserId = 999;
            // Act
            var result = _userService.GetUserById(nonExistingUserId);

            // Assert
            Assert.Null(result);

        }
        //-------------------------------------------------------


        [Fact]
        public void Login_Returns_User_WhenCredentialsAreCorrect()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };


            var result = _userService.AddUser(newUser);
            // Act
            var login = _userService.Login("deneme1", "password123");

            // Assert
            Assert.NotNull(login);
            Assert.Equal("deneme1", login.UserName);
            Assert.Equal("password123", login.UserPassword);

            _userService.DeleteUser(newUser.UserID);
        }

        [Fact]
        public void Login_Returns_Null_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };


            var result = _userService.AddUser(newUser);
            // Act
            var login = _userService.Login("testuser", "wrongpassword");

            // Assert
            Assert.Null(login);

            _userService.DeleteUser(newUser.UserID);
        }
        //-------------------------------------------------------
        [Fact]
        public void UpdateUser_Returns_Null_WhenUserNotFound()
        {
            int nonExistingUserId = 999;

            // Arrange & Act
            var updatedUser = new User
            {
                UserName = "updateduser",
                UserPassword = "updatedpassword",
                UserFullName = "Updated User"
            };
            var result = _userService.UpdateUser(nonExistingUserId, updatedUser);

            // Assert
            Assert.Null(result);
        }

        [Fact]
            public void UpdateUser_Returns_UpdatedUser_WhenUserFoundCorrect()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme2",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            // Add the user once
            _userService.AddUser(newUser);

            // Update the user
            var updatedUser = new User
            {
                UserName = "updateduser",
                UserPassword = "updatedpassword",
                UserFullName = "Updated User"
            };

            // Act
            var updatedUserResult = _userService.UpdateUser(newUser.UserID, updatedUser);

            // Assert
            Assert.NotNull(updatedUserResult);
            Assert.Equal("updateduser", updatedUserResult.UserName);
            Assert.Equal("updatedpassword", updatedUserResult.UserPassword);
            Assert.Equal("Updated User", updatedUserResult.UserFullName);

            _userService.DeleteUser(newUser.UserID);
        }
        //_---------------------------------------------------------------
        [Fact]
        public void DeleteUser_Returns_DeletedUser()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };
            
            _userService.AddUser(newUser);

            // Act
            var deletedUser = _userService.DeleteUser(newUser.UserID);

            // Assert
            Assert.NotNull(deletedUser);
            Assert.Equal("deneme1", deletedUser.UserName);
            Assert.Equal("password123", deletedUser.UserPassword);
            Assert.Equal("Test User", deletedUser.UserFullName);
        }
        [Fact]
        public void DeleteUser_Returns_Null()
        {
            // Arrange EN SON SİLME İŞLEMİ
            int nonExistingUserId = 999;
            // Act
            var deletedUser = _userService.DeleteUser(nonExistingUserId);

            // Assert
            Assert.Null(deletedUser);

        }
        [Fact]//3.test veya sırası önemli degil 
        public void AddWatchedMovieById_Returns_1_When_Movie_Not_Found()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            _userService.AddUser(newUser);
            
            int nonExistMovieId = 9999;
            // Act
            var result = _watchedService.AddWatchedMovieById(newUser.UserID, nonExistMovieId);

            // Assert
            Assert.Equal((null, 1), result);

            _userService.DeleteUser(newUser.UserID);
        }

        [Fact]//2. test
        public void AddWatchedMovieById_Returns_2_When_Movie_Already_Watched_By_User()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            _userService.AddUser(newUser);

            var newMovie = new Movie
            {
                MovieName = "Test Movie",
                MovieRelaseYear = 2023,
                MovieDirector = "Test Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1

            };

            _movieService.AddMovie(newMovie);

            _watchedService.AddWatchedMovieById(newUser.UserID, newMovie.MovieID);

            // Act
            var result = _watchedService.AddWatchedMovieById(newUser.UserID, newMovie.MovieID);
           
            // Assert
            Assert.Equal((null, 2), result);
            _userService.DeleteUser(newUser.UserID);
            _movieService.DeleteMovieByID(newMovie.MovieID);
            

        }

        [Fact] //1. test bu olucak 
        public void AddWatchedMovieById_Returns_3_When_Movie_Added_To_Watched_Successfully()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            _userService.AddUser(newUser);

            var newMovie = new Movie
            {
                MovieName = "Test Movie",
                MovieRelaseYear = 2023,
                MovieDirector = "Test Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1

            };
            _movieService.AddMovie(newMovie);

            // Act
            var result = _watchedService.AddWatchedMovieById(newUser.UserID, newMovie.MovieID);
            // Assert
            Assert.Equal((result.Item1, 3), result);

            _userService.DeleteUser(newUser.UserID);
            _movieService.DeleteMovieByID(newMovie.MovieID);
            _watchedService.DeleteWatched(result.Item1.WatchedID);

        }

        [Fact] // son test bulucak en son yazdıgırdı testi yapıp sonrada silicez 
        public void GetWatchedMovieNamesByUser_Returns_Movie_Names_Successfully()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            _userService.AddUser(newUser);

            var newMovie = new Movie
            {
                MovieName = "Test Movie",
                MovieRelaseYear = 2023,
                MovieDirector = "Test Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1

            };
            _movieService.AddMovie(newMovie);
            var result = _watchedService.AddWatchedMovieById(newUser.UserID, newMovie.MovieID);
            var expectedMovieNames = new List<string> { "Test Movie" };

            // Act
            var MovieNames = _watchedService.GetWatchedMovieNamesByUser(newUser.UserID);

            // Assert
            Assert.Equal(expectedMovieNames, MovieNames);

            _movieService.DeleteMovieByID(newMovie.MovieID);
            _userService.DeleteUser(newUser.UserID);
            _watchedService.DeleteWatched(result.Item1.WatchedID);

        }



    }
}
