using EntityLayer.Concrete;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IWatchedDal 
    {
        OneOf<Watched, int> AddWatchedMovieById(int userId, int movieId);
        Watched? DeleteWatched(int watchedId);
        List<string> GetWatchedMovieNamesByUser(int userId);
       
    }
}
