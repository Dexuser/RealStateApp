using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

public class DashBoardServiceTests
{
    [Fact]
    public async Task GetAdminDashBoard_ReturnsCorrectCounts()
    {
        // Arrange
        var mockAccountService = new Mock<IAccountServiceForWebApp>();
        var mockPropertyRepo = new Mock<IPropertyRepository>();

        // EF InMemory
        var options = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new RealStateAppContext(options);

        context.Properties.AddRange(
            new Property
            {
                IsAvailable = true,
                Code = "000001",
                PropertyTypeId = 1,
                SaleTypeId = 1,
                Price = 500,
                SizeInMeters = 25,
                Rooms = 2,
                Bathrooms = 1,
                Description = "Apartamento bonito",
                CreatedAt = DateTime.Now,
                AgentId = "agent1",
                Id = 0
            },
            new Property
            {
                IsAvailable = false,
                Code = "000002",
                PropertyTypeId = 1,
                SaleTypeId = 1,
                Price = 500,
                SizeInMeters = 25,
                Rooms = 2,
                Bathrooms = 1,
                Description = "Apartamento bonito 2",
                CreatedAt = DateTime.Now,
                AgentId = "agent2",
                Id = 0
            },
            new Property
            {
                IsAvailable = true,
                Code = "000003",
                PropertyTypeId = 2,
                SaleTypeId = 2,
                Price = 500,
                SizeInMeters = 27,
                Rooms = 2,
                Bathrooms = 2,
                Description = "Apartamento bonito 3",
                CreatedAt = DateTime.Now,
                AgentId = "agent3",
                Id = 0
            }
        );

        await context.SaveChangesAsync();

        mockPropertyRepo
            .Setup(r => r.GetAllQueryable())
            .Returns(context.Properties);

        mockAccountService.Setup(x => x.CountUsers(Roles.Client, true)).ReturnsAsync(10);
        mockAccountService.Setup(x => x.CountUsers(Roles.Client, false)).ReturnsAsync(2);
        mockAccountService.Setup(x => x.CountUsers(Roles.Agent, true)).ReturnsAsync(5);
        mockAccountService.Setup(x => x.CountUsers(Roles.Agent, false)).ReturnsAsync(1);
        mockAccountService.Setup(x => x.CountUsers(Roles.Developer, true)).ReturnsAsync(3);
        mockAccountService.Setup(x => x.CountUsers(Roles.Developer, false)).ReturnsAsync(0);

        var service = new DashBoardService(
            mockAccountService.Object,
            mockPropertyRepo.Object);

        // Act
        var result = await service.GetAdminDashBoard();

        // Assert
        result.AvailablePropertiesCount.Should().Be(2);
        result.SoldPropertiesCount.Should().Be(1);
        result.ActiveClientsCount.Should().Be(10);
        result.InactiveClientsCount.Should().Be(2);
        result.ActiveAgentsCount.Should().Be(5);
        result.InactiveAgentsCount.Should().Be(1);
        result.ActiveDevelopersCount.Should().Be(3);
        result.InActiveDevelopersCount.Should().Be(0);
    }
}
