using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using Core_Proje_Api;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Api_Test
{
    public class WatchedControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public static int newWatchedId;

        public WatchedControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            
        }

        private async Task<string> GenerateToken(HttpClient client, string username, string password)
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
         // Bir filmi izlendi olarak kaydetme - Geçersiz film ID senaryosu
        public async Task WatchedController_ShouldAddWatchedMovieByUser_InvalidMovieId()
        {
            // Arrange
            using var client = _factory.CreateClient();



            // Token kısımları
            string accessToken = await GenerateToken(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Geçersiz bir film ID'si 
            int invalidMovieId = -1;

            // JSON formatına çevirme
            var movieData = new
            {
                movieId = invalidMovieId
            };
            var movieJsonContent = JsonConvert.SerializeObject(movieData);
            var movieRequestContent = new StringContent(movieJsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Watched", movieRequestContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact, TestPriority(2)]
        // Bir filmi izlendi olarak kaydetme - Başarılı senaryo
        public async Task WatchedController_ShouldAddWatchedMovieByUser_Success()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları
            string accessToken = await GenerateToken(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //eklenecek film id si
            int existingMovieId = 9;


            var movieData = new
            {
                movieId = existingMovieId
            };
            var movieJsonContent = JsonConvert.SerializeObject(movieData);
            var movieRequestContent = new StringContent(movieJsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"/api/Watched/{existingMovieId}", null);
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();

            var watched = JsonConvert.DeserializeObject<Watched>(responseContent);
            // watched id yi alma
            newWatchedId = watched.WatchedID;



        }
        [Fact, TestPriority(3)]
        // Bir filmi izlendi olarak kaydetme - Film zaten izlenmiş senaryosu
        public async Task WatchedController_ShouldAddWatchedMovieByUser_MovieAlreadyWatched()
        {
            // Arrange
            using var client = _factory.CreateClient();


            // Token kısımları
            string accessToken = await GenerateToken(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // üstteki filmi tekrar eklemeye calısıyorum 
            int existingMovieId = 9;

            // JSON formatına çevirme
            var movieData = new
            {
                movieId = existingMovieId
            };
            var movieJsonContent = JsonConvert.SerializeObject(movieData);
            var movieRequestContent = new StringContent(movieJsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Watched", movieRequestContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(4)] 
        // Kullanıcının izlediği filmleri getirme - Başarılı senaryo
        public async Task WatchedController_ShouldReturnWatchedMovieNamesByUser_Success()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları
            string accessToken = await GenerateToken(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.GetAsync("/api/Watched/watched/MovieNames");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            
            await client.DeleteAsync($"/api/Watched/watched/{newWatchedId}");


        }

        [Fact] // Kullanıcının izlediği filmleri getirme - Film izlemediği senaryo
        public async Task WatchedController_ShouldReturnWatchedMovieNamesByUser_NoMoviesWatched()
        {
            // Arrange
            using var client = _factory.CreateClient();


            // Token kısımları
            string accessToken = await GenerateToken(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await client.GetAsync("/api/Watched/watched/MovieNames");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        

        

        
    }
}