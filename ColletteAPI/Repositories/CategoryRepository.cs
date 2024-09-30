using ColletteAPI.Models.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ColletteAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categories.Find(category => true).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _categories.Find(category => category.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var updateDefinition = Builders<Category>.Update
                .Set(c => c.Name, category.Name)
                .Set(c => c.Description, category.Description);

            var result = await _categories.UpdateOneAsync(c => c.Id == category.Id, updateDefinition);
            return result.ModifiedCount > 0 ? category : null;
        }

        public async Task DeleteAsync(string id)
        {
            await _categories.DeleteOneAsync(category => category.Id == id);
        }
    }
}
