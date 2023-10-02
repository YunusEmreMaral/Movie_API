using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IUserDal
    {
        User? AddUser(User newUser);
        User? DeleteUser(int userId);
        User? GetUserById(int userId);
        User? Login(string username, string password);
        User? UpdateUser(int id,  User p);
    }
}
