using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
{
    public ChatMessageRepository(RealStateAppContext context) : base(context)
    {
    }
}