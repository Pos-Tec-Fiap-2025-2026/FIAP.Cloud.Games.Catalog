using Catalog.Core.Models;

namespace Catalog.Infrastructure.Repositories
{
    public class InMemoryDatabase
    {
        public List<Game> Games { get; } = new();
        
        public Dictionary<int, List<int>> UserLibraries { get; } = new();

        public InMemoryDatabase()
        {
            Games.Add(new Game { Id = 1, Name = "Elden Ring", Price = 250.00m, Description = "GOTY" });
            Games.Add(new Game { Id = 2, Name = "FIFA 25", Price = 300.00m, Description = "Futebol" });
            Games.Add(new Game { Id = 3, Name = "Hollow Knight", Price = 50.00m, Description = "Indie" });
        }

        public void AddToLibrary(int userId, int gameId)
        {
            if (!UserLibraries.ContainsKey(userId))
            {
                UserLibraries[userId] = new List<int>();
            }

            if (!UserLibraries[userId].Contains(gameId))
            {
                UserLibraries[userId].Add(gameId);
            }
        }
    }
}