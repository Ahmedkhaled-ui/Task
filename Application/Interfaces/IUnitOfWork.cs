using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {

        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntities;
        Task<int> SaveChangesAsync();
    }
}
