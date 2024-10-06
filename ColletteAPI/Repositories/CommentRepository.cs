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

        public CommentRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _comments = database.GetCollection<Comment>("Comments");
        }

        // Add a new comment
        public async Task<Comment> AddComment(Comment comment)
        {
            await _comments.InsertOneAsync(comment);
            return comment;
        }

        // Get a comment by ID
        public async Task<Comment> GetCommentById(string commentId)
        {
            return await _comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
        }

        // Get comments by Vendor ID
        public async Task<List<Comment>> GetCommentsByVendorId(string vendorId)
        {
            return await _comments.Find(c => c.VendorId == vendorId).ToListAsync();
        }

        // Update a comment by ID (only by relevant customer)
        public async Task<Comment> UpdateComment(string commentId, string customerId, UpdateCommentDto updateDto)
        {
            var comment = await _comments.Find(c => c.Id == commentId && c.CustomerId == customerId).FirstOrDefaultAsync();
            if (comment != null)
            {
                comment.CommentText = updateDto.CommentText;
                comment.Rating = updateDto.Rating;
                await _comments.ReplaceOneAsync(c => c.Id == commentId, comment);
            }
            return comment;
        }
    }
}
