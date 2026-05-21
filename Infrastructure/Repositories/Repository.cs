using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class Reposatory<Tentity>(ApplicationDbContext dbContext)
       : IRepository<Tentity> where Tentity : BaseEntities
    {
        public void Add(Tentity entity)
       => dbContext.Add(entity);

      

        public void Delete(Tentity entity)
       => dbContext.Remove(entity);

        public async Task<IEnumerable<Tentity>> GetAllAsync()
       => await dbContext.Set<Tentity>().ToListAsync();

      
     
        public async Task<Tentity?> GetByIdAsync(int id)
      => await dbContext.Set<Tentity>().FindAsync(id);

       

        public void Update(Tentity entity)
       => dbContext.Update(entity);
    }
}
