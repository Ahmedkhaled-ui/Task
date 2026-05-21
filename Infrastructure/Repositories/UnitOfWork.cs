using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = [];

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntities
        {


            var entityanme = typeof(TEntity).Name;

            if (repositories.TryGetValue(entityanme, out object? value))
                return (IRepository<TEntity>)value;


            var rerepository = new Reposatory<TEntity>(dbContext);
            repositories.Add(entityanme, rerepository);
            return rerepository;


        }

        public Task<int> SaveChangesAsync()
      => dbContext.SaveChangesAsync();
    }
}
