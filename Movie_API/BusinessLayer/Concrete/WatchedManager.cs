using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class WatchedManager : IWatchedService
    {
        IWatchedDal _watchedDal;
        public WatchedManager(IWatchedDal watchedDal)
        {
            _watchedDal = watchedDal;

        }
        public OneOf<Watched, int> AddWatchedMovieById(int userId, int movieId)
        {
            return _watchedDal.AddWatchedMovieById(userId, movieId);

        }   
        public Watched? DeleteWatched(int watchedId)
        {
            return _watchedDal.DeleteWatched(watchedId);
        }
        public List<string> GetWatchedMovieNamesByUser(int userId)
        {
            return _watchedDal.GetWatchedMovieNamesByUser(userId);
        }
        

    }
}
