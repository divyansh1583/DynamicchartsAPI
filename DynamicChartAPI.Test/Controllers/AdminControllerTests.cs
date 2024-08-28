using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Controllers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamicChartAPI.Test.Controllers
{
    public class AdminControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IAdminService> mockAdminService;

        public AdminControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockAdminService = this.mockRepository.Create<IAdminService>();
        }

        private AdminController CreateAdminController()
        {
            return new AdminController(
                this.mockAdminService.Object);
        }

        [Fact]
        public async Task AddUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var adminController = this.CreateAdminController();
            AdminRegisterDto adminRegisterDto = null;

            // Act
            var result = await adminController.AddUser(
                adminRegisterDto);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Login_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var adminController = this.CreateAdminController();
            AdminLoginDto adminLoginDto = null;

            // Act
            var result = await adminController.Login(
                adminLoginDto);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
