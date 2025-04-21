using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCenter.Common.Entities;
using MongoDB.Driver;

namespace GameCenter.Infra
{
    //public class DatabaseRepository<T> where T : EntityBase
    //{
    //    private readonly IMongoCollection<T> _collection;

    //    public DatabaseRepository(MongoDbContext context)
    //    {
    //        _collection = context.GetCollection<T>();
    //    }

    //    public async Task<List<T>> GetAllAsync()
    //    {
    //        return await _collection.Find(_ => true).ToListAsync();
    //    }

    //    public async Task<T> GetByIdAsync(Guid id)
    //    {
    //        return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    //    }

    //    public async Task AddAsync(T user)
    //    {
    //        await _collection.InsertOneAsync(user);
    //    }

    //    public async Task UpdateAsync(T user)
    //    {
    //        await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
    //    }

    //    public async Task DeleteAsync(Guid id)
    //    {
    //        await _collection.DeleteOneAsync(u => u.Id == id);
    //    }
    //}

}
