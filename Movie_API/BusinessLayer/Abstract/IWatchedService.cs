using EntityLayer.Concrete;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IWatchedService 
    {
        OneOf<Watched, int> AddWatchedMovieById(int userId, int movieId);
        List<string> GetWatchedMovieNamesByUser(int userId);

        Watched? DeleteWatched(int watchedId);         
    }
}
