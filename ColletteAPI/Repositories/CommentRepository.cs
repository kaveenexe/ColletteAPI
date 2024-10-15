// CommentRepository.cs
// Implementation of comment repository to handle MongoDB operations.

using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using MongoDB.Driver;

namespace ColletteAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentRepository(IMongoDatabase database)
        {
            _comments = database.GetCollection<Comment>("Comments");
        }

        // Add a comment
        public async Task AddAsync(Comment comment)
        {
            await _comments.InsertOneAsync(comment);
        }

        // Update a comment
        public async Task UpdateAsync(Comment comment)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.Id, comment.Id);
            await _comments.ReplaceOneAsync(filter, comment);
        }

        // Get comment by ID
        public async Task<Comment> GetByIdAsync(string commentId)
        {
            return await _comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
        }

        // Get comments by vendor ID
        public async Task<IEnumerable<Comment>> GetByVendorIdAsync(string vendorId)
        {
            return await _comments.Find(c => c.VendorId == vendorId).ToListAsync();
        }
    }
}
