// IComment.cs
// Interface for comment service layer.

using ColletteAPI.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddComment(CreateCommentDto createCommentDto);
        Task<CommentDto> GetCommentById(string commentId);
        Task<List<CommentDto>> GetCommentsByVendorId(string vendorId);
        Task<CommentDto> UpdateComment(string commentId, string customerId, UpdateCommentDto updateCommentDto);
    }
}
