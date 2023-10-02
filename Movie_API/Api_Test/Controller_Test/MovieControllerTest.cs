using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core_Proje_Api;
using EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;

namespace Api_Test
{
    [TestCaseOrderer("Api_Test.PriorityOrderer", "Api_Test")]

    public class MovieControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public static int IdNewMovie;
        public MovieControllerTest(WebApplicationFactory<Startup> factory) 
        {
            _factory = factory;
        }
        

        private async Task<string> GetAccessTokenAsync(HttpClient client, string username, string password)
        { 
            

            var url = $"/api/users/login?username={username}&password={password}";

            var response = await client.PostAsync(url, null);



            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic tokenObject = JObject.Parse(responseContent);
            string accessToken = tokenObject.token; 
            return accessToken;
        }

       

        [Fact, TestPriority(1)]
        public async Task MovieAdd_ShouldReturnCreatedStatusWithValidData()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // kullanılacak filmi yaratma

            var newMovie = new Movie
            {
                MovieName = "Test Movie",
                MovieRelaseYear = 2023,
                MovieDirector = "Test Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1

            };
            // JSON formatına cevirme
            var jsonContent = JsonConvert.SerializeObject(newMovie);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Movie", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // kontrol etme 
            var content = await response.Content.ReadAsStringAsync();
            var movieResult = JsonConvert.DeserializeObject<Movie>(content);

            Assert.Equal(newMovie.MovieName, movieResult?.MovieName);
            Assert.Equal(newMovie.MovieRelaseYear, movieResult?.MovieRelaseYear);
            Assert.Equal(newMovie.MovieDirector, movieResult?.MovieDirector);
            Assert.Equal(newMovie.MovieDuration, movieResult?.MovieDuration);
            Assert.Equal(newMovie.IsWatched, movieResult?.IsWatched);
            Assert.Equal(newMovie.MovieGenreID, movieResult?.MovieGenreID);

            IdNewMovie = movieResult != null ? movieResult.MovieID : -1;
        }
        [Fact]
        public async Task MovieAdd_ShouldReturnBadRequestStatusForInvalidData()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Film oluşturuyoruz fakat validate kısmı için bir nesneyi hatalı veriyorum ben burada yılı yanlış verdim !
            var invalidMovie = new Movie
            {
                MovieName = "Invalid Movie",
                MovieRelaseYear = 100000,
                MovieDirector = "Invalid Director",
                MovieDuration = 120,
                IsWatched = false,
                MovieGenreID = 1
            };


            // JSON formatına çevirme
            var jsonContent = JsonConvert.SerializeObject(invalidMovie);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Movie", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(2)]

        
        public async Task MovieGetByID_ShouldReturnOkStatusForExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Önceki id ' yi alıyorum
            int existingMovieId = IdNewMovie;

            // Act
            var response = await client.GetAsync($"/api/Movie/{existingMovieId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        }
        [Fact]
        public async Task MovieGetByID_ShouldReturnNotFoundStatusForNonExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            int nonExistingMovieId = -1;

            // Act
            var response = await client.GetAsync($"/api/Movie/{nonExistingMovieId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact, TestPriority(3)]

        public async Task MovieWatched_ShouldReturnOkStatusForExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Önceki id ' yi alıyorum
            int existingMovieId = IdNewMovie;

            // Act
            var response = await client.PostAsync($"/api/Movie/{existingMovieId}", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task MovieWatched_ShouldReturnNotFoundStatusForNonExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Varolmayan bir film ID'si 
            int nonExistingMovieId = -1;

            // Act
            var response = await client.PostAsync($"/api/Movie/{nonExistingMovieId}", null);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(4)]

        
        public async Task MovieDelete_ShouldReturnOkStatusForExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Önceki id ' yi alıyorum
            int existingMovieId = IdNewMovie;

            // Act
            var response = await client.DeleteAsync($"/api/Movie/{existingMovieId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]

        public async Task MovieDelete_ShouldReturnNotFoundForExistingMovie()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // olmayan bir film idsi
            int existingMovieId = 9999;

            // Act
            var response = await client.DeleteAsync($"/api/Movie/{existingMovieId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task GetAllMovies_ShouldReturnListOfMovies()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            Assert.NotNull(accessToken);
            Assert.NotEmpty(accessToken);

            // Act
            var response = await client.GetAsync("/api/Movie");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

        }
    }
}