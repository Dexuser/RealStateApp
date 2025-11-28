using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class ChatMessageRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    
    public ChatMessageRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"ChatMessageRepositoryTest_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_ShouldAddChatMessageToDatabase()
    {
        
        //Arrage
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler descripcion"
        };

        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento descripcion"
        };
        
        await using var context = new RealStateAppContext(_dbOptions);
        await context.Set<SaleType>().AddAsync(saleType);
        await context.Set<PropertyType>().AddAsync(propertyType);
        
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = propertyType.Id,
            SaleTypeId = saleType.Id,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = Guid.NewGuid().ToString(),
        };
        await context.Set<Property>().AddAsync(property);

        var repo = new ChatMessageRepository(context);
        var ChatMessage = new ChatMessage
        {
            Id = 0,
            PropertyId = property.Id,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        
        //Act
        var chatMessage = await repo.AddAsync(ChatMessage);
        
        //Assert
        chatMessage.Should().NotBeNull();
        chatMessage.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new ChatMessageRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_ChatMessage_When_Exists()
    {
        //Arrage
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler descripcion"
        };

        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento descripcion"
        };
        
        await using var context = new RealStateAppContext(_dbOptions);
        await context.Set<SaleType>().AddAsync(saleType);
        await context.Set<PropertyType>().AddAsync(propertyType);
        
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = propertyType.Id,
            SaleTypeId = saleType.Id,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = Guid.NewGuid().ToString(),
        };
        
        await context.Set<Property>().AddAsync(property);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 0,
            PropertyId = property.Id,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        await context.Set<ChatMessage>().AddAsync(chatMessageToAdd);
        
        
        //Act
        var repo =  new ChatMessageRepository(context);
        var chatMessage = await repo.GetByIdAsync(chatMessageToAdd.Id);
        
        //Assert
        chatMessage.Should().NotBeNull();
        chatMessage.Id.Should().BeGreaterThan(0);
        chatMessage.Message.Should().Be("Hola");
    }
    
    
    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 0,
            PropertyId = 1,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        await context.Set<ChatMessage>().AddAsync(chatMessageToAdd);
        await context.SaveChangesAsync();

        //Act
        var repo =  new ChatMessageRepository(context);
        chatMessageToAdd.Message = "update";
        var updated = await repo.UpdateAsync(chatMessageToAdd.Id, chatMessageToAdd);
        
        //Assert
        updated.Should().NotBeNull();
        updated.Message.Should().Be("update");
        
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_ChatMessage()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 0,
            PropertyId = 1,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        await context.Set<ChatMessage>().AddAsync(chatMessageToAdd);
        await context.SaveChangesAsync();

        //Act
        var repo =  new ChatMessageRepository(context);
        chatMessageToAdd.Message = "update";
        var updated = await repo.UpdateAsync(chatMessageToAdd.Id, chatMessageToAdd);
        
        //Assert
        updated.Should().NotBeNull();
        updated.Message.Should().Be("update"); 
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repo =  new ChatMessageRepository(context);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 999,
            PropertyId = 1,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };

        //Act
        var updated = await repo.UpdateAsync(chatMessageToAdd.Id, chatMessageToAdd);
        
        //Assert
        updated.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Remove_ChatMessage()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repo =  new ChatMessageRepository(context);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 0,
            PropertyId = 1,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        await context.Set<ChatMessage>().AddAsync(chatMessageToAdd);

        //Act
        await repo.DeleteAsync(chatMessageToAdd.Id);
        
        //Assert
        var chatMessage = await repo.GetByIdAsync(chatMessageToAdd.Id);
        chatMessage.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repo =  new ChatMessageRepository(context);

        //Act
        Func<Task> act = async () => await repo.DeleteAsync(999);
        
        //Assert
        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task GetAllList_Should_Return_All_ChatMessages()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.ChatMessages.AddRange(
            new ChatMessage
            {
                Id = 0,
                PropertyId = 1,
                SenderId = Guid.NewGuid().ToString(),
                ReceiverId = Guid.NewGuid().ToString(),
                Message = "Hola",
           },
            new ChatMessage
            {
                Id = 0,
                PropertyId = 1,
                SenderId = Guid.NewGuid().ToString(),
                ReceiverId = Guid.NewGuid().ToString(),
                Message = "adios",
            });
        await context.SaveChangesAsync();
        var repository = new ChatMessageRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_AssetTypes()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ChatMessageRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_ChatMessages()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ChatMessageRepository(context);
        var chatMessageToAdd = new ChatMessage
        {
            Id = 0,
            PropertyId = 1,
            SenderId = Guid.NewGuid().ToString(),
            ReceiverId = Guid.NewGuid().ToString(),
            Message = "Hola"
        };
        context.ChatMessages.Add(chatMessageToAdd);
        await context.SaveChangesAsync();
        
        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();
        
        // Assert
        result.Should().NotBeEmpty();
    }
}