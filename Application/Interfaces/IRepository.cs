using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntities
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(int id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);



    }
}
