// CommentService.cs
// Implementation of the IComment service, handling business logic for comments.

using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // Add a comment
        public async Task<CommentDto> AddComment(CreateCommentDto createCommentDto)
        {
            var comment = new Comment
            {
                VendorId = createCommentDto.VendorId,
                CustomerId = createCommentDto.CustomerId,
                CommentText = createCommentDto.CommentText,
                Rating = createCommentDto.Rating
            };

            var result = await _commentRepository.AddComment(comment);
            return MapToDto(result);
        }

        // Get a comment by ID
        public async Task<CommentDto> GetCommentById(string commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            return MapToDto(comment);
        }

        // Get comments by Vendor ID
        public async Task<List<CommentDto>> GetCommentsByVendorId(string vendorId)
        {
            var comments = await _commentRepository.GetCommentsByVendorId(vendorId);
            return comments.ConvertAll(c => MapToDto(c));
        }

        // Update a comment (only the relevant customer can update)
        public async Task<CommentDto> UpdateComment(string commentId, string customerId, UpdateCommentDto updateCommentDto)
        {
            var updatedComment = await _commentRepository.UpdateComment(commentId, customerId, updateCommentDto);
            return MapToDto(updatedComment);
        }

        // Helper method to map Comment to CommentDto
        private CommentDto MapToDto(Comment comment)
        {
            if (comment == null) return null;

            return new CommentDto
            {
                Id = comment.Id,
                VendorId = comment.VendorId,
                CustomerId = comment.CustomerId,
                CommentText = comment.CommentText,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
