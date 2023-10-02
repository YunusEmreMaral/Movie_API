using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace DataAccessLayer.EntityFramework
{
    public class EfWatchedDal : IWatchedDal
    {
        private readonly Context _context;
        public EfWatchedDal(Context context)
        {
            _context = context;
        }

        public OneOf<Watched, int> AddWatchedMovieById(int userId, int movieId)
        {
            var user = _context.Users.Find(userId);
            var movie = _context.Movies.Find(movieId);

            if (user == null || movie == null)
            {
                return 1;
            }
            var isMovieAlreadyWatched = _context.Watcheds.Any(w => w.UserID == userId && w.MovieID == movieId);
            if (isMovieAlreadyWatched)
            {
                return 2;
            }
            else
            {
                var watched = new Watched
                {
                    UserID = userId,
                    MovieID = movieId
                };
                _context.Watcheds.Add(watched);
                _context.SaveChanges();
                return watched;
            }
        }

        public Watched? DeleteWatched(int watchedId)
        {
            var watched = _context.Watcheds.Find(watchedId);
            if (watched != null)
            {
                _context.Watcheds.Remove(watched);
                _context.SaveChanges();
                return watched;
            }
            else
                return null;

        }

        public List<string?> GetWatchedMovieNamesByUser(int userId)
        {

            var movieNames = _context.Watcheds
    .Where(w => w.UserID == userId)
    .Select(w => w.Movie != null ? w.Movie.MovieName : null) 
    .ToList();

            return movieNames;
        }

        
    }
}
