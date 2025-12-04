using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Property.Queries.GetAll;
using RealStateApp.Core.Application.Features.Property.Queries.GetByCode;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Property
{
    public class GetAllPropertyQueryTests
    {
        private readonly DbContextOptions<RealStateAppContext> _dbOptions;

        public GetAllPropertyQueryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
                .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
                .Options;
        }


        [Fact]
        public async Task Handle_Should_Return_All_Properties()
        {
            var context = new RealStateAppContext(_dbOptions);
            var houseType = new Core.Domain.Entities.PropertyType
            {
                Id = 1,
                Name = "Casa",
                Description = "Casa bonita"
            };

            var apartmentType = new Core.Domain.Entities.PropertyType
            {
                Id = 2,
                Name = "Departamento",
                Description = "Departamento bonito"
            };

            var saleType = new SaleType
            {
                Id = 1,
                Name = "Venta",
                Description = "Venta completa"
            };

            context.PropertyTypes.AddRange(houseType, apartmentType);
            context.SaleTypes.Add(saleType);

            context.Properties.AddRange(
                new Core.Domain.Entities.Property
                {
                    Id = 1,
                    Code = "000001",
                    IsAvailable = true,
                    Price = 100,
                    Rooms = 2,
                    Bathrooms = 1,
                    CreatedAt = DateTime.UtcNow,
                    AgentId = "agent",
                    PropertyTypeId = houseType.Id,
                    SaleTypeId = saleType.Id,
                    SizeInMeters = 80,
                    Description = "Disponible",
                },
                new Core.Domain.Entities.Property
                {
                    Id = 2,
                    Code = "000002",
                    IsAvailable = false,
                    Price = 200,
                    Rooms = 3,
                    Bathrooms = 2,
                    CreatedAt = DateTime.UtcNow,
                    AgentId = "agent",
                    PropertyTypeId = apartmentType.Id,
                    SaleTypeId = saleType.Id,
                    SizeInMeters = 120,
                    Description = "No disponible"
                }
            );

            await context.SaveChangesAsync();
            var propertyRepositoy = new PropertyRepository(context);

            var accountServiceMock = new Mock<IBaseAccountService>();
            accountServiceMock
                .Setup(x => x.GetUsersByIds(new List<string> { "agent" }))
                .ReturnsAsync(new List<UserDto>()
                {
                    new UserDto()
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
                    }
                });

            var query = new GetAllPropertyQuery();
            var handler = new GetAllPropertyQueryHandler(propertyRepositoy, accountServiceMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_No_Properties_Exists()
        {
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context);

            var accountServiceMock = new Mock<IBaseAccountService>();
            accountServiceMock
                .Setup(x => x.GetUsersByIds(new List<string> { "agent" }))
                .ReturnsAsync(new List<UserDto>()
                {
                    new UserDto()
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
                    }
                });

            var query = new GetPropertyByCodeQuery() { Code = "000001" };
            var handler = new GetPropertyByCodeHandler(propertyRepositoy, accountServiceMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiException>().WithMessage("Property not found");
        }
    }
}