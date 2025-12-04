using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Property.Queries.GetById;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Property
{
    public class GetPropertyByIdQueryTests
    {
        private readonly DbContextOptions<RealStateAppContext> _dbOptions;

        public GetPropertyByIdQueryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
                .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
                .Options;
        }


        [Fact]
        public async Task Handle_Should_Return_Property_When_Exists()
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

            var property = new Core.Domain.Entities.Property
            {
                Code = "000001",
                PropertyTypeId = propertyType.Id,
                SaleTypeId = saleType.Id,
                Price = 100,
                SizeInMeters = 50,
                Rooms = 2,
                Bathrooms = 1,
                AgentId = "agent1",
                CreatedAt = DateTime.UtcNow,
                Id = 0,
                Description = "casa bonita en punta cana"
            };
            context.Properties.Add(property);
            
            var accountServiceMock = new Mock<IBaseAccountService>();
            accountServiceMock
                .Setup(x => x.GetUserById("agent1"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent1",
                    FirstName = "Pedro",
                    Email = "pedro@test.com",
                    LastName = "Pedro",
                    UserName = "Pedro Test",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    Role = nameof(Roles.Agent),
                    PhoneNumber = "802222333"
                });
            
            await context.SaveChangesAsync();
            var propertyRepositoy = new PropertyRepository(context);

            var query = new GetPropertyByIdQuery { Id = 1 };
            var handler = new GetPropertyByIdHandler(propertyRepositoy, accountServiceMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Code.Should().Be("000001");
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_No_Agents_Exists()
        {
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context);

            var accountServiceMock = new Mock<IBaseAccountService>();
            accountServiceMock
                .Setup(x => x.GetUserById("agent1"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent1",
                    FirstName = "Pedro",
                    Email = "pedro@test.com",
                    LastName = "Pedro",
                    UserName = "Pedro Test",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    Role = nameof(Roles.Agent),
                    PhoneNumber = "802222333"
                });

            var query = new GetPropertyByIdQuery { Id = 999 };
            var handler = new GetPropertyByIdHandler(propertyRepositoy, accountServiceMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            
            // Assert
            await act.Should().ThrowAsync<ApiException>().WithMessage("Property not found");
        }
    }
}