using BusinessLayer.Abstract;
using BusinessLayer.Validator;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Movie_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedController : ControllerBase
    {
        private readonly IWatchedService _watchedService;
        public WatchedController(IWatchedService watchedService)
        {
            _watchedService = watchedService;
        }
        [HttpGet("watched/MovieNames")] 
        public IActionResult GetWatchedMovieNamesByUser()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            if (userId == null)
            {
                return Unauthorized("Geçersiz kullanıcı!");
            }

            var movieNames = _watchedService.GetWatchedMovieNamesByUser(Convert.ToInt32(userId));
            if (movieNames.Count >= 1)
            {
                return Ok(movieNames);
            }
            else
            {
                return NotFound("Bu kullanıcının izlediği film bulunmamaktadır");
            }
        }

        [HttpPost("{movieId}")]
        public IActionResult AddWatchedMovieByUser(int movieId)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Geçersiz kullanıcı!");
            }

            var result = _watchedService.AddWatchedMovieById(int.Parse(userId), movieId);
            return result.Match<IActionResult>(
                watched => Ok(watched),
                errorCode =>
                {
                    if (errorCode == 1)
                        return NotFound("Geçersiz movie id !");
                    else if (errorCode == 2)
                        return NotFound("Film daha önceden izlendi olarak kaydedildi!");
                    else
                        return StatusCode(500, "Beklenmeyen bir hata oluştu!");
                }
            );
        }
        [HttpDelete("watched/{watchedId}")] 
        public IActionResult DeleteWatched(int watchedId)
        {
            var userId = HttpContext.Items["UserId"]?.ToString(); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Geçersiz kullanıcı!");
            }

            var deletedWatched = _watchedService.DeleteWatched(watchedId);

            if (deletedWatched == null)
            {
                return NotFound("İzlenen film bulunamadı!");
            }

            return Ok(deletedWatched);
        }



    }
}
