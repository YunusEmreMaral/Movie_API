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
        public class MovieController : ControllerBase
        {
            private readonly IMovieService _movieService;
            public MovieController(IMovieService movieService)
            {
                _movieService = movieService;
            }
            [HttpDelete("{id}")]
            public IActionResult DeleteMovie(int id)
            {
                var movie = _movieService.DeleteMovieByID(id);
                return movie != null ? Ok("Film Başarıyla Silindi") : NotFound("Film Bulunamadı.");
            }
            [HttpGet]
            public IActionResult GetAllMovies()
            {
                var movies = _movieService.GetAllFilms();
                return movies != null ? Ok(movies) : NotFound("Film Bulunmamakta.");
            }
            [HttpGet("{id}")]
            public IActionResult MovieGetByID(int id) 
            {
                var movie = _movieService.GetMovieByID(id);
                return movie != null ? Ok(movie) : NotFound("Bu ID'ye sahip film bulunmamaktadır.");
            }
            [HttpPost] 
            public IActionResult MovieAdd(Movie newMovie)
            {
                var validationResult = new MovieValidator().Validate(newMovie);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
                    return BadRequest(errors);
                }
                var movie = _movieService.AddMovie(newMovie);
                return Created("", newMovie);
            
            }
            [HttpPost("{id}")] 
            public IActionResult MovieWatched(int id)
            {
                var movie = _movieService.WatchedMovie(id);
                return movie != null ? Ok("Film başarıyla izlendi olarak işaretlendi") : NotFound("Bu ID'ye sahip film bulunmamaktadır.");
            
            }

        }
        


    }
