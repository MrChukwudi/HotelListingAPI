using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace HotelListingAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;

        public GenericRepository(HotelListingDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity); //Remove and Update does not happen asynchronously
            await _context.SaveChangesAsync();
        }




        public async  Task<bool> Exists(int id)
        {
           var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync(); 
        }





        public async Task<T> GetAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }


            return await _context.Set<T>().FindAsync(id);
        }





        public async Task UpdateAsync(T entity)
        {
            //Remove and Update does not happen asynchronously
            _context.Update(entity); //EF is intelligent enough to know that it is updating the entity and to find the corresponding Table and record to execute the Update
            await _context.SaveChangesAsync();
        }
    }
}
