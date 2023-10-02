using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core_Proje_Api;
using EntityLayer.Concrete;
using Microsoft.OpenApi.Validations.Rules;

namespace Api_Test
{
    [TestCaseOrderer("Api_Test.PriorityOrderer", "Api_Test")]
    public class UserControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public static int IdNewUser;
        

        public UserControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string username, string password)
        {
            // token kısımları


            var url = $"/api/users/login?username={username}&password={password}";

            var response = await client.PostAsync(url, null);



            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic tokenObject = JObject.Parse(responseContent);
            string accessToken = tokenObject.token; // Değişiklik burada
            return accessToken;
        }

        [Fact, TestPriority(1)]// ekleme 
        public async Task UserController_ShouldAddNewUser()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Yeni bir kullanıcı verisi oluşturun

            var newUser = new User
            {
                
                UserName = "TestUser",
                UserPassword = "TestPassword",
                UserFullName = "Test User"
            };



            // JSON formatına çevirme
            var jsonContent = JsonConvert.SerializeObject(newUser);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/User", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var movieResult = JsonConvert.DeserializeObject<User>(content);
            IdNewUser = movieResult.UserID;


        }
        [Fact]// ekleme 
        public async Task UserController_AddNewUser_ReturnBadRequest()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // kullanıcı yaratıyorum fakat username boş bırakıyorum validate işlemine takılması için 
            var invalidUser = new User
            {
                UserName="q",
                UserPassword="12312312",
                UserFullName="deneme kullanıcı"
            };


            // JSON formatına cevirme
            var jsonContent = JsonConvert.SerializeObject(invalidUser);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/User", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(2)] // getirme
        public async Task UserController_ShouldReturnUserByID()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // önceki id'yi alıyorum
            int existingUserId = IdNewUser;

            // Act
            var response = await client.GetAsync($"/api/User/{existingUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var userResult = JsonConvert.DeserializeObject<User>(content);
            Assert.NotNull(userResult);
            Assert.Equal(existingUserId, userResult.UserID);
        }
        [Fact] // getirme
        public async Task UserController_ShouldReturnNotFoundForNonExistingUser()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Varolan olmayan bir kullanıcı ID'si
            int nonExistingUserId = -1;

            // Act
            var response = await client.GetAsync($"/api/User/{nonExistingUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
       
        [Fact, TestPriority(3)]// güncelleme
        public async Task UserController_ShouldUpdateUser()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // önceki id'yi alma
            int existingUserId = IdNewUser;

            // güncellemek için kullanıcı yaratma
            var updateUser = new User
            {
                UserID=existingUserId,
                UserName = "UpdatedUser",
                UserPassword = "UpdatePassword",
                UserFullName = "Update User"
            };
            

            var jsonContent = JsonConvert.SerializeObject(updateUser);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/api/User", requestContent);


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact, TestPriority(4)]// guncelleme ama eksik
        public async Task UserController_ShouldReturnBadRequestForInvalidUserUpdate()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Varolan bir kullanıcı ID'si belirleyin (örneğin, veritabanında mevcut bir kullanıcı ID'si)
            int existingUserId = IdNewUser;

            // gecersiz olucak bi kullanıcı yaratıyorum kullanıcı adı olmadı mesela 
            var invalidUser = new User
            {
                UserID = existingUserId,
                UserName = "q",
                UserPassword="NonExistPassword",
                UserFullName="Non Exist"

            };

            // JSON formatına cevirme
            var jsonContent = JsonConvert.SerializeObject(invalidUser);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/api/User", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
       
        [Fact, TestPriority(5)] // giriş
        public async Task UserController_ShouldReturnSuccessForLogin()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // önceki id şifreyi alıyoruz
            var Username = "UpdatedUser";
            var UserPassword = "UpdatePassword";



            var url = $"/api/users/login?username={Username}&password={UserPassword}";

            var response = await client.PostAsync(url, null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        }

            
        [Fact]
        public async Task UserController_ShouldReturnUnauthorizedForInvalidLogin() // giriş
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Geçersiz bir kullanıcı adı ve şifre belirleyin
            string invalidUsername = "invaliduser";
            string invalidPassword = "invalidpassword";

            // JSON formatına cevirme
            var loginData = new
            {
                username = invalidUsername,
                password = invalidPassword
            };
            var jsonContent = JsonConvert.SerializeObject(loginData);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/users/login", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact, TestPriority(6)] // silme
        public async Task UserController_ShouldDeleteUser()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Token kısımları 
            string accessToken = await GetAccessTokenAsync(client, "deneme", "deneme");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // önceki id'yi alma
            int existingUserId = IdNewUser;

            // Act
            var response = await client.DeleteAsync($"/api/User/{existingUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
        }



    }
}
