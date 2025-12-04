using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Features.Agent.Queries.GetAll;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Agent
{
    public class GetAllAgentQueryTests
    {
        private readonly DbContextOptions<RealStateAppContext> _dbOptions;

        public GetAllAgentQueryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
                .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
                .Options;

        }


        [Fact]
        public async Task Handle_Should_Return_All_Agents()
        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context);
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetAllUserOfRole(Roles.Agent, false))
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
                    },
                    new UserDto()
                    {
                        Id = "agent2",
                        FirstName = "marco",
                        Email = "marco@test.com",
                        LastName = "marco",
                        UserName = "marco Test",
                        IdentityCardNumber = "0000000002",
                        RegisteredAt = DateTime.Now,
                        Role = nameof(Roles.Agent),
                        PhoneNumber = "802222333"
                    }
                    
                });

            var handler = new GetAllAgentsQueryHandler(propertyRepositoy, accountServiceMock.Object);
            var query = new GetAllAgentsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_No_Agents_Exists()

        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context);
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetAllUserOfRole(Roles.Agent, false))
                .ReturnsAsync(new List<UserDto>());
            
            var handler = new GetAllAgentsQueryHandler(propertyRepositoy, accountServiceMock.Object);
            var query = new GetAllAgentsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}