using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        User? AddUser(User newUser);
        User? GetUserById(int userId);
        User? Login(string username, string password);
        User? UpdateUser(int id,  User p);

        User? DeleteUser(int userId);
    }
}
