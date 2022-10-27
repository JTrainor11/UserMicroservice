using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Controllers;
using UserMicroservice.Models;
using UserMicroservice.Repository;

namespace UserMicroserviceTests.Tests
{
    public class UserMicroserviceTests
    {
        private readonly UserController _controller;
        private readonly IUserRepository _mockUserRepository;

        public UserMicroserviceTests()
        {
            _mockUserRepository = new MockUserRepository();
            _controller = new UserController(_mockUserRepository);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOKResult()
        {
            var okResult = _controller.Get();

            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllUsers()
        {
            var okResult = _controller.Get() as OkObjectResult;

            var items = Assert.IsAssignableFrom<IEnumerable<User>>(okResult?.Value);
            Assert.Equal(3, items.Count());
        }

        [Fact]
        public void Put_WhenCalledWithExistingUserName_Returns409Conflict()
        {
            var conflictResult = _controller.Put(3, "User Two") as ObjectResult;

            Assert.IsType<ProblemDetails>(conflictResult?.Value as ProblemDetails);

            ProblemDetails? problemDetails = conflictResult?.Value as ProblemDetails;

            Assert.Equal(409, (problemDetails?.Status));
        }

        [Fact]
        public void Put_WhenCalledWithNonExistentUser_Returns404NotFound()
        {
            var conflictResult = _controller.Put(4, "User Four") as ObjectResult;

            Assert.IsType<ProblemDetails>(conflictResult?.Value as ProblemDetails);

            ProblemDetails? problemDetails = conflictResult?.Value as ProblemDetails;

            Assert.Equal(404, (problemDetails?.Status));
        }

        [Fact]
        public void Put_WhenCalledWithNewUserNameForExistingUser_ReturnsOKResult()
        {
            var updatedOkResult = _controller.Put(3, "User Four");
            Assert.IsType<OkResult>(updatedOkResult as OkResult);
        }

        [Fact]
        public void Post_WhenCalledWithExistingUserName_Returns409Conflict()
        {
            var conflictResult = _controller.Post("User Two") as ObjectResult;

            Assert.IsType<ProblemDetails>(conflictResult?.Value as ProblemDetails);

            ProblemDetails? problemDetails = conflictResult?.Value as ProblemDetails;

            Assert.Equal(409, (problemDetails?.Status));
        }

        [Fact]
        public void Post_WhenCalledWithNewUserName_ReturnsOKResult()
        {
            var insertedOkResult = _controller.Post("User Five");
            Assert.IsType<OkResult>(insertedOkResult as OkResult);
        }

        [Fact]
        public void Delete_WhenCalledWithNonExistentUser_Returns404NotFound()
        {
            var conflictResult = _controller.Delete(6) as ObjectResult;

            Assert.IsType<ProblemDetails>(conflictResult?.Value as ProblemDetails);

            ProblemDetails? problemDetails = conflictResult?.Value as ProblemDetails;

            Assert.Equal(404, (problemDetails?.Status));
        }

        [Fact]
        public void Delete_WhenCalledWithExistingUser_ReturnsOKResult()
        {
            var deletedOkResult = _controller.Delete(3);
            Assert.IsType<OkResult>(deletedOkResult as OkResult);
        }

        [Fact]
        public void Get_WhenCalledWithNoUsers_Returns404NotFound()
        {
            _controller.Delete(1);
            _controller.Delete(2);
            _controller.Delete(3);

            var notFoundResult = _controller.Get() as ObjectResult;

            Assert.IsType<ProblemDetails>(notFoundResult?.Value as ProblemDetails);

            ProblemDetails? problemDetails = notFoundResult?.Value as ProblemDetails;

            Assert.Equal(404, (problemDetails?.Status));
        }
    }
}