using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfMovieDal : IMovieDal

    {
        private readonly Context _context;
        public EfMovieDal(Context context)
        {
            _context = context;
        }
        public Movie? AddMovie(Movie newMovie)
        {
            if (newMovie != null)
            {
                _context.Movies.Add(newMovie);
                _context.SaveChanges();

                return newMovie;
            }

            return null;
        }

        public Movie? DeleteMovieByID(int id)
        {
            var movie = _context.Movies.Find(id);  
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                return movie;
            }
            else
                return null;
        }

        public List<Movie> GetAllFilms()
        {
            return _context.Movies?.ToList() ?? new List<Movie>();
        }

        public Movie? GetMovieByID(int movieID)
        {
            return _context.Movies?.FirstOrDefault(x => x.MovieID == movieID);
        }

        public Movie? WatchedMovie(int id)
        {
            var movie = _context.Movies?.FirstOrDefault(x => x.MovieID == id);
            if (movie != null)
            {
                movie.IsWatched = true;
                _context.SaveChanges();
                return movie;
            }
            return null;
        }

    }
}

