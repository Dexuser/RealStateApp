using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Agent.Queries.GetById;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Agent
{
    public class GetAgentByIdQueryTests
    {
        private readonly DbContextOptions<RealStateAppContext> _dbOptions;
        
        public GetAgentByIdQueryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
                .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
                .Options;
        }
        
        
        [Fact]
        public async Task Handle_Should_Return_Agent_When_Exists()
        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context); 
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent-id"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent-id",
                    FirstName = "Pedro",
                    Email = "pedro@test.com",
                    LastName = "Pedro",
                    UserName = "Pedro Test",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    Role = nameof(Roles.Agent),
                    PhoneNumber = "802222333"
                });

            var handler = new GetAgentByIdQueryHandler(accountServiceMock.Object,propertyRepositoy);
            var query = new GetAgentByIdQuery { Id = "agent-id" };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("agent-id");
            result.FirstName.Should().Be("Pedro");
            result.Email.Should().Be("pedro@test.com");
        }
        
        [Fact]
        public async Task Handle_Should_Throw_Exception_Agent_When_Not_Exists()
        {
            // Arrange
            var context = new RealStateAppContext(_dbOptions);
            var propertyRepositoy = new PropertyRepository(context); 
            var accountServiceMock = new Mock<IBaseAccountService>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent-id"))
                .ReturnsAsync((UserDto?)null);

            var handler = new GetAgentByIdQueryHandler(accountServiceMock.Object,propertyRepositoy);
            var query = new GetAgentByIdQuery { Id = "agent-id" };

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            
            // Assert
            await act.Should().ThrowAsync<ApiException>().WithMessage("Agent not found");
        }
    }

}
