using BusinessLayer.Abstract;
using BusinessLayer.Validator;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MovieManager : IMovieService
    {
        
        private readonly IMovieDal _movieDal; // readonly yap 
        private readonly MovieValidator _movieValidator;



        public MovieManager(IMovieDal movieDal, MovieValidator movieValidator)
        {
            _movieDal = movieDal;
            _movieValidator = movieValidator;


        }

        public Movie? AddMovie(Movie newMovie)
        {
            if (newMovie != null)
            {
                var movie = _movieDal.AddMovie(newMovie);
                return movie;
            }
            else
                return null;

    }

        public Movie? DeleteMovieByID(int id)
        {
            var movie = _movieDal.DeleteMovieByID(id);
            return movie;
        }

        public List<Movie> GetAllFilms()
        {
                return _movieDal.GetAllFilms();
        }

        public Movie? GetMovieByID(int movieID)
        {
            var movie = _movieDal.GetMovieByID(movieID);
            return movie;
        }

        public Movie? WatchedMovie(int id)
        {
            var movie = _movieDal.WatchedMovie(id);
            return movie;
        }
        
    }
}
