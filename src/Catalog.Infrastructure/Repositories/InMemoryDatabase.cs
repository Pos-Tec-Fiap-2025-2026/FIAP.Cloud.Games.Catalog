using Catalog.Core.Models;

namespace Catalog.Infrastructure.Repositories
{
    public class InMemoryDatabase
    {
        public List<Game> Games { get; } = new();
        
        public Dictionary<int, List<Guid>> UserLibraries { get; } = new();

        public InMemoryDatabase()
        {
            Games.Add(new Game { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Elden Ring", Price = 250.00m, Description = "GOTY" });
            Games.Add(new Game { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "FIFA 25", Price = 300.00m, Description = "Futebol" });
            Games.Add(new Game { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Hollow Knight", Price = 50.00m, Description = "Indie" });
        }

        public void AddToLibrary(int userId, Guid gameId)
        {
            if (!UserLibraries.ContainsKey(userId))
            {
                UserLibraries[userId] = new List<Guid>();
            }

            if (!UserLibraries[userId].Contains(gameId))
            {
                UserLibraries[userId].Add(gameId);
            }
        }
    }
}