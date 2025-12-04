using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Features.Agent.Queries.GetAllPropertyOfAgent;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Agent
{
    public class GetAllPropertyOfAgentQueryTests
    {
        private readonly DbContextOptions<RealStateAppContext> _dbOptions;

        public GetAllPropertyOfAgentQueryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
                .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
                .Options;
        }


        [Fact]
        public async Task Handle_Should_Return_All_Properties_Of_The_Agent()
        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);

            var saleType = new SaleType
            {
                Id = 1,
                Name = "Venta",
                Description = "Venta completa"
            };
            var propertyType = new Core.Domain.Entities.PropertyType
            {
                Id = 1,
                Name = "Casa",
                Description = "casa completa"
            };

            context.SaleTypes.Add(saleType);
            context.PropertyTypes.Add(propertyType);

            context.Properties.AddRange(
                new Core.Domain.Entities.Property
                {
                    Code = "000001",
                    PropertyTypeId = 1,
                    SaleTypeId = 1,
                    Price = 100,
                    SizeInMeters = 50,
                    Rooms = 2,
                    Bathrooms = 1,
                    AgentId = "agent",
                    CreatedAt = DateTime.UtcNow,
                    Id = 0,
                    Description = "casa bonita en punta cana"
                },
                new Core.Domain.Entities.Property
                {
                    Code = "000002",
                    PropertyTypeId = 1,
                    SaleTypeId = 1,
                    Price = 200,
                    SizeInMeters = 80,
                    Rooms = 3,
                    Bathrooms = 2,
                    AgentId = "agent",
                    CreatedAt = DateTime.UtcNow,
                    Id = 0,
                    Description = "casa linda en punta cana"
                }
            );
            await context.SaveChangesAsync();
            var propertyRepositoy = new PropertyRepository(context);
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent",
                    FirstName = "Pedro",
                    Email = "pedro@test.com",
                    LastName = "Pedro",
                    UserName = "Pedro Test",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    Role = nameof(Roles.Agent),
                    PhoneNumber = "802222333"
                });

            var handler = new GetAllPropertiesOfAgentQueryHandler(propertyRepositoy, accountServiceMock.Object);
            var query = new GetAllPropertyOfAgentQuery() { Id = "agent" };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_This_Agent_Doesnt_Have_Properties()
        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context);
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent",
                    FirstName = "Pedro",
                    Email = "pedro@test.com",
                    LastName = "Pedro",
                    UserName = "Pedro Test",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    Role = nameof(Roles.Agent),
                    PhoneNumber = "802222333"
                });

            var handler = new GetAllPropertiesOfAgentQueryHandler(propertyRepositoy, accountServiceMock.Object);
            var query = new GetAllPropertyOfAgentQuery() { Id = "agent" };
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

        }
    }
}