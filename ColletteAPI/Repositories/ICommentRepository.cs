// ICommentRepository.cs
// Interface for comment repository.

using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> AddComment(Comment comment);
        Task<Comment> GetCommentById(string commentId);
        Task<List<Comment>> GetCommentsByVendorId(string vendorId);
        Task<Comment> UpdateComment(string commentId, string customerId, UpdateCommentDto updateDto);
    }
}
