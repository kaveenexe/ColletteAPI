// IComment.cs
// Interface for comment service layer.

using ColletteAPI.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public interface ICommentService
    {
        // Add a new comment for a vendor
        Task<CommentDto> AddCommentAsync(CreateCommentDto createCommentDto);

        // Update a comment (Rating cannot be updated, only the CommentText)
        Task<CommentDto> UpdateCommentAsync(string commentId, UpdateCommentDto updateCommentDto);

        // Get all comments for a vendor
        Task<IEnumerable<CommentDto>> GetCommentsByVendorIdAsync(string vendorId);

        // Get comment by ID
        Task<CommentDto> GetCommentByIdAsync(string commentId);
    }
}
