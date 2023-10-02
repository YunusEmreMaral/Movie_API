using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IMovieDal 
    {
        Movie? AddMovie(Movie newMovie);
        Movie? DeleteMovieByID(int id);
        List<Movie> GetAllFilms();
        Movie? GetMovieByID(int movieID);
        Movie? WatchedMovie(int id);

    }
}
