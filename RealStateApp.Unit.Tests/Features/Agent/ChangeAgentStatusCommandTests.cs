using AutoMapper;
using FluentAssertions;
using Moq;
using RealStateApp.Core.Application;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Agent.Commands.ChangeStatus;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Unit.Tests.Features.Agent
{
    public class ChangeAgentStatusCommandTests
    {
        
        [Fact]
        public async Task Handle_Should_Change_Agent_Status()
        {
            // Arrange
            var accountServiceMock = new Mock<IBaseAccountService>();
            var propertyRepositoryMock = new Mock<IPropertyRepository>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent"))
                .ReturnsAsync(new UserDto
                {
                    Id = "agent",
                    Role = nameof(Roles.Agent),
                    FirstName = "Pedro",
                    LastName = "Estoy cansado.",
                    Email = "Pero sigo aqui",
                    UserName = "Pedro",
                    IdentityCardNumber = "0000000001",
                    RegisteredAt = DateTime.Now,
                    PhoneNumber = "80222333",
                    
                });

            accountServiceMock
                .Setup(x => x.SetStateOnUser("agent", false))
                .ReturnsAsync(Result.Ok());

            var handler = new ChangeAgentStatusCommandHandler(
                accountServiceMock.Object,
                propertyRepositoryMock.Object
            );

            var command = new ChangeAgentStatusCommand
            {
                Id = "agent",
                StatusToSet = false
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(MediatR.Unit.Value);

            accountServiceMock.Verify(
                x => x.SetStateOnUser("agent", false),
                Times.Once
            );
        }

        
        [Fact]
        public async Task Handle_Should_Throw_Exception_Agent_When_Not_Exists()
        {
            // Arrange
            var accountServiceMock = new Mock<IBaseAccountService>();
            var propertyRepositoryMock = new Mock<IPropertyRepository>();

            accountServiceMock
                .Setup(x => x.GetUserById("agent-id"))
                .ReturnsAsync((UserDto?)null);

            accountServiceMock
                .Setup(x => x.SetStateOnUser("agent-id", false))
                .ReturnsAsync(Result.Fail("User not found"));

            var handler = new ChangeAgentStatusCommandHandler(
                accountServiceMock.Object,
                propertyRepositoryMock.Object
            );

            var command = new ChangeAgentStatusCommand
            {
                Id = "agent-id",
                StatusToSet = false
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            // Assert
            await act.Should().ThrowAsync<ApiException>().WithMessage("Agent not found");
        }
    }

}
