using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IMovieService 
    {
        Movie? AddMovie(Movie newMovie);
        Movie? DeleteMovieByID(int id);
        List<Movie> GetAllFilms();
        Movie? GetMovieByID(int movieID);
        Movie? WatchedMovie(int id);
    }
}
