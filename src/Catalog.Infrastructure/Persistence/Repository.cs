using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence
{
    internal class Repository : IRepository
    {
        private readonly CatalogDbContext _context;

        public Repository(CatalogDbContext contex) => _context = contex;

        public async Task<Game?> GetGameByIdAsync(int id) => await _context.Game.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Game>> GetGamesAsync() => await _context.Game.ToListAsync();
        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : EntityBase
        {
            return _context.Set<TEntity>();
        }
        public async Task<bool> CreateAsync(Game game) 
        {
            game.CreatedAt = game.UpdatedAt = DateTime.Now;
            await _context.AddAsync(game);
            var saveCount = await _context.SaveChangesAsync();

            return saveCount > 0;
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            entity.UpdatedAt = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
            var saveCount = await _context.SaveChangesAsync();

            return saveCount > 0;
        }
    }

    public interface IRepository
    {
        public Task<IEnumerable<Game>> GetGamesAsync();
        public Task<Game?> GetGameByIdAsync(int id);
        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : EntityBase;
        public Task<bool> CreateAsync(Game game);
        public Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : EntityBase;
    }
}
