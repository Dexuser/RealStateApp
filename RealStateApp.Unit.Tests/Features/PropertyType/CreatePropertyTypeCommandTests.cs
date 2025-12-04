using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.PropertyType.Commands.CreatePropertyType;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.PropertyType;

public class CreatePropertyTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
        
    public CreatePropertyTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
            .Options;
    }
    
    [Fact]
    public async Task Handle_Should_Return_PropertyType_Id_When_Creation_Is_Successful()
    {
            
        // Arrange
        var context =  new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);
        var command = new CreatePropertyTypeCommand
        {
            Name = "Apartamento",
            Description = "Apartamento descripcion" 
        };

        var handler = new CreatePropertyTypeCommandHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var createdEntity = await repository.GetByIdAsync(result);
        createdEntity.Should().NotBeNull();
        createdEntity.Id.Should().Be(result);
        createdEntity.Name.Should().Be("Apartamento");
        createdEntity.Description.Should().Be("Apartamento descripcion");
    }

        
    [Fact]
    public async Task Handle_Should_Throw_Creation_Is_Not_Successful()
    {
        // Arrange
        var mockRepo = new Mock<IPropertyTypeRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Core.Domain.Entities.PropertyType>()))!
            .ReturnsAsync((Core.Domain.Entities.PropertyType)null);

        var command = new CreatePropertyTypeCommand
        {
            Name = "Casa",
            Description = "Casa completa" 
        };
            
        var handler = new CreatePropertyTypeCommandHandler(mockRepo.Object);
            
        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
        // Assert
        await act.Should().ThrowAsync<ApiException>().WithMessage("Error creating property type");
    }
}