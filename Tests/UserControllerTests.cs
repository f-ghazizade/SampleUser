using DataAccessLayer.Models;
using DataAccessLayer.Models.MyInMemoryApi.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleUser.Controllers;
using ServicesLayer.Contracts;
using System.ComponentModel.DataAnnotations;
using Xunit;


namespace Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();

            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task SignUp_ReturnsOkResult_WhenUserIsCreated()
        {
            // Arrange
            var newUserViewModel = new UserViewModel { UserName = "testuser", Password = "Password123!" };
            var expectedResult = new ResultModel<User> { Success = true, Message = "کاربر با موفقیت ثبت شد.", Data = new User { UserName = "testuser", Password = "" } };
            _mockUserService.Setup(service => service.SignUpAsync(It.IsAny<UserViewModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SignUp(newUserViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResultModel<User>>(okResult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal(expectedResult.Message, returnValue.Message);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginUserViewModel = new UserViewModel { UserName = "testuser", Password = "wrongpassword" };
            var expectedResult = new ResultModel<User> { Success = false, Message = "نام کاربری یا رمز عبور نادرست است." };
            _mockUserService.Setup(service => service.LoginAsync(It.IsAny<UserViewModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(loginUserViewModel);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResultModel<User>>(unauthorizedResult.Value);
            Assert.False(returnValue.Success);
            Assert.Equal(expectedResult.Message, returnValue.Message);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidViewModel()
        {
            // Arrange
            var loginUserViewModel = new UserViewModel { UserName = "user", Password = "pass" };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(loginUserViewModel);
            Validator.TryValidateObject(loginUserViewModel, validationContext, validationResults);

            var expectedResult = new ResultModel<User> { Success = false, Message = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage)) };

            _mockUserService.Setup(service => service.LoginAsync(It.IsAny<UserViewModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(loginUserViewModel);

            // Assert
            var badRequestResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResultModel<User>>(badRequestResult.Value);
            var expectedErrorMessage = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            Assert.False(returnValue.Success);
            Assert.Equal(expectedErrorMessage, returnValue.Message);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var loginUserViewModel = new UserViewModel { UserName = "nonexistentuser", Password = "Password123!" };
            var expectedResult = new ResultModel<User> { Success = false, Message = "نام کاربری یافت نشد." };
            _mockUserService.Setup(service => service.LoginAsync(It.IsAny<UserViewModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(loginUserViewModel);

            // Assert
            var badRequestResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResultModel<User>>(badRequestResult.Value);
            Assert.Equal(expectedResult.Message, returnValue.Message);
        }

    }
}