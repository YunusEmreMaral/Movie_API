            using DataAccessLayer.Abstract;
            using DataAccessLayer.Concrete;
            using DataAccessLayer.Repository;
            using EntityLayer.Concrete;
            using Microsoft.EntityFrameworkCore;
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;

            namespace DataAccessLayer.EntityFramework
            {
                public class EfUserDal : IUserDal
                {
                    private readonly Context _context;
                    public EfUserDal(Context context)
                    {
                        _context = context;
                    }

                    public User? AddUser(User newUser)
                    {
                        if (newUser == null)
                        {
                            return null;
                        }
                        var existingUser = _context.Users.FirstOrDefault(u => u.UserName == newUser.UserName && u.UserPassword == newUser.UserPassword);
                        if (existingUser != null)
                        {
                            return null; // aynı username ve userpassword se null doner !
                        }
                        _context.Users.Add(newUser);
                        _context.SaveChanges();

                        return newUser;
                    }


                    public User? DeleteUser(int userId)
                    {
                        var user = _context.Users.Find(userId);
                        if (user != null)
                        {
                            _context.Users.Remove(user);
                            _context.SaveChanges();
                            return user;
                        }
                        else
                            return null;
                    }

                    public User? GetUserById(int userId)
                    {
                        User? user = _context.Users.FirstOrDefault(x => x.UserID == userId);
                        return user!;
                    }

                    public User? Login(string username, string password)
                    {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username && u.UserPassword == password);
            if (user?.UserName != null && user?.UserPassword != null)
            {
                return user;
            }
            else
            {
                return null;            }

        }

                    public User? UpdateUser(int id, User p)
                    {
                        var user = _context.Users.FirstOrDefault(x => x.UserID == id);

                        if (user == null)
                        {
                            return null;
                        }
                        var existingUser = _context.Users.FirstOrDefault(u => u.UserName == p.UserName && u.UserPassword == p.UserPassword && u.UserID != id);
                        if (existingUser != null)
                        {
                            return null; // aynı username vb password olmamasını saglamak 
                        }


                        user.UserName = p.UserName;
                        user.UserPassword = p.UserPassword;
                        user.UserFullName = p.UserFullName;

                        _context.SaveChanges();

                        return user;
                    }
                }
            }
