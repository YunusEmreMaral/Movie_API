using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Validator;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;

        }
        [Authorize]
        [HttpGet("{id}")] // BELLİ BİR KULLANICIN PROFİLİNİ
        public IActionResult UserGetByID(int id)
        { 
            var user = _userService.GetUserById(id);
            return user != null ? Ok(user) : NotFound("Böyle bir id ' ye sahip kullanıcı bulunmamaktadır");
        }
        [Authorize]
        [HttpPut]  // BELLİ BİR KULLANICININ PROFİLİNİ GÜNCELLEME
        public IActionResult UpdateUser( [FromBody] User updateUser) 

        {
            int id = updateUser.UserID;
            var validationResult = new UserValidator().Validate(updateUser);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
                return BadRequest(errors);
            }
            var user = _userService.UpdateUser(id, updateUser);
            return user != null ? Ok(user) : NotFound("Geçersiz kullanıcı girdiniz !");

        }
        [Authorize]
        [HttpPost] // yeni bir kullanıcı ekleme  
        public IActionResult UserAdd(User newUser)
        {
            var validationResult = new UserValidator().Validate(newUser);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var user = _userService.AddUser(newUser);

            if (user != null)
                return Created("", newUser);
            else
                return NotFound("Kullanıcı adi veya şifre değiştirin.");
        }
        [Authorize]

        [HttpDelete("{id}")]// foreign keyleri denemek için koydum ve calısıyor !
        public IActionResult DeleteUser(int id)
        {

            var movie = _userService.DeleteUser(id);
            return movie != null ? Ok("Kullanıcı Başarıyla Silindi") : NotFound("Kullanıcı Bulunamadı.");
        }


        [HttpPost("/api/users/login")]// login hata mesajlı 
        public IActionResult Login( string username,  string password)
        {
            var user = _userService.Login(username, password);
            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış !");
            }
           
            var token = GenerateToken(user);    

            return Ok(new { Token = token });


        }

        private string GenerateToken(User user)
        {
            
            var tokenSettings = _configuration.GetSection("TokenSettings");
            var secretKey = tokenSettings.GetValue<string>("SecretKey");
            var validIssuer = tokenSettings.GetValue<string>("ValidIssuer");
            var validAudience = tokenSettings.GetValue<string>("ValidAudience");

            // Token oluşturma
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        new Claim("UserId", user.UserID.ToString())
    };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = validIssuer,
                Audience = validAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}