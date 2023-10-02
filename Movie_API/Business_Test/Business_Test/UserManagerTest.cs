using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Business_Test.Business_Test
{
    [TestCaseOrderer("Business_Test.PriorityOrderer", "Business_Test")]
    public class UserManagerTest : TestBed<BusinessLayerStartupFixture>
    {
        private readonly IUserService _userService;
        public static int userId;

        public UserManagerTest(ITestOutputHelper testOutputHelper, BusinessLayerStartupFixture fixture) : base(testOutputHelper, fixture)
        {
            _userService = fixture.GetService<IUserService>(_testOutputHelper);
        }
        [Fact, TestPriority(1)]
        public void AddUser_Returns_User()
        {
            // Arrange
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };

            // Act
            var result = _userService.AddUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("deneme1", result.UserName);
            Assert.Equal("password123", result.UserPassword);
            Assert.Equal("Test User", result.UserFullName);


            userId = newUser.UserID;
        }
        [Fact, TestPriority(2)]

        public void AddUser_Returns_Null_WhenUserAlreadyExists()
        {
            // Arrange sıralı olacagı için önceki eklenin oldugunu kabul ediyorum 
            
            // Act
            var newUser = new User
            {
                UserName = "deneme1",
                UserPassword = "password123",
                UserFullName = "Test User"
            };
            var result = _userService.AddUser(newUser);

            // Assert
            Assert.Null(result);
   
        }
        [Fact]
        public void AddUser_Returns_Null()
        {
            // Arrange
            User? nullUser = null;
            

            // Act
            var result = _userService.AddUser(nullUser);

            // Assert
            Assert.Null(result);
        
        }
        //-------------------------------------------------------
          [Fact, TestPriority(3)]

        public void GetUserById_Returns_CorrectUser()
        {
            // Arrange
            

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("deneme1", result.UserName);
            Assert.Equal("password123", result.UserPassword);
            Assert.Equal("Test User", result.UserFullName);
        }
        public void GetUserById_Returns_NonExistUser()
        {
            // Arrange

            int nonExistingUserId = 999;
            // Act
            var result = _userService.GetUserById(nonExistingUserId);

            // Assert
            Assert.Null(result);

        }
        //-------------------------------------------------------


        [Fact, TestPriority(4)]

        public void Login_Returns_User_WhenCredentialsAreCorrect()
        {
            // Arrange
           

            // Act
            var result = _userService.Login("deneme1", "password123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("deneme1", result.UserName);
            Assert.Equal("password123", result.UserPassword);
            
        }

        [Fact]
        public void Login_Returns_Null_WhenCredentialsAreIncorrect()
        {
            // Arrange
            
            // Act
            var result = _userService.Login("testuser", "wrongpassword");

            // Assert
            Assert.Null(result);
        }
        //-------------------------------------------------------
        [Fact]
        public void UpdateUser_Returns_Null_WhenUserNotFound()
        {
            int nonExistingUserId = 999;

            // Arrange & Act
            var updatedUser = new User
            {
                UserName = "deneme",
                UserPassword = "updatedpassword",
                UserFullName = "Updated User"
            };
            var result = _userService.UpdateUser(nonExistingUserId, updatedUser);

            // Assert
            Assert.Null(result);
        }

        [Fact, TestPriority(5)]
        public void UpdateUser_Returns_Correct_WhenUserFoundCorrect()
        {
            // Arrange 
            var updatedUser = new User
            {
                UserName = "updateduser",
                UserPassword = "updatedpassword",
                UserFullName = "Updated User"
            };
            // Act
            var updated = _userService.UpdateUser(userId, updatedUser); 
            
            // Assert
            Assert.NotNull(updated); 
            Assert.Equal("updateduser", updated.UserName);
            Assert.Equal("updatedpassword", updated.UserPassword);
            Assert.Equal("Updated User", updated.UserFullName);
        }
        //_---------------------------------------------------------------
        [Fact, TestPriority(6)]
        public void DeleteUser_Returns_DeletedUser()
        {
            // Arrange EN SON SİLME İŞLEMİ

            // Act
            var deletedUser = _userService.DeleteUser(userId);

            // Assert
            Assert.NotNull(deletedUser);
            Assert.Equal("updateduser", deletedUser.UserName);
            Assert.Equal("updatedpassword", deletedUser.UserPassword);
            Assert.Equal("Updated User", deletedUser.UserFullName);
        }
        [Fact]
        public void DeleteUser_Returns_Null()
        {
            // Arrange EN SON SİLME İŞLEMİ
            int nonExistingUserId = 999;
            // Act
            var deletedUser = _userService.DeleteUser(nonExistingUserId);

            // Assert
            Assert.Null(deletedUser);
            
        }

    }
}

