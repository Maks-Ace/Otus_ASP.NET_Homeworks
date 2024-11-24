using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {

        private List<T> _data;


        public InMemoryRepository(IEnumerable<T> data)
        {
            _data= data.ToList();
        }

        protected IEnumerable<T> Data => _data;


        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }


        public Task<Guid> CreateAsync(T item)
        {
            
            if(_data == null) _data = new List<T>(); 

            item.Id = Guid.NewGuid();
            _data.Add(item);

            return Task.FromResult(item.Id);
        }


        public Task<bool> UpdateAsync(T item) 
        {
            var itemToUpdateIndex = _data.FindIndex(x => x.Id == item.Id );

            _data[itemToUpdateIndex] = item;

            return Task.FromResult(true);
        }

        public  Task<bool> DeleteByIdAsync(Guid id)
        {
            var item = _data.FirstOrDefault(x => x.Id == id);
            
            return Task.FromResult(_data.Remove(item));
        }


    }
}