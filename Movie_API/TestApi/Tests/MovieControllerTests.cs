using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Movie_API.Controllers;

using Moq;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Abstract;

namespace TestApi.Tests
{
    public class MovieControllerTests { 
    [Fact]
    public void GetAllMovies_ReturnsOkResult()
        {
            // Arrange
            var movieServiceMock = new Mock<IMovieService>();
            var controller = new MovieController(movieServiceMock.Object);

            // Act
            var result = controller.GetAllMovies();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
