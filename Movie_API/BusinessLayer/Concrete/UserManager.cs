using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public User? AddUser(User newUser)
        {
            if (newUser != null)
            {
                var user = _userDal.AddUser(newUser);
                return user;
            }
            else
                return null;

        }

        public User? DeleteUser(int userId)
        {
            return _userDal.DeleteUser(userId);
        }

        public User? GetUserById(int userId)
        {
            var user = _userDal.GetUserById(userId);
            return user;
        }

        public User? Login(string username, string password)
        {

            return _userDal.Login(username, password);

        }

        public User? UpdateUser(int id,User p)
        {
            var user = _userDal.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            user.UserName = p.UserName;
            user.UserPassword = p.UserPassword;
            user.UserFullName = p.UserFullName;

            return _userDal.UpdateUser(id, user);
        }

            
    }
}
