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
        public async Task<CommentDto> AddCommentAsync(CreateCommentDto createCommentDto)
        {
            var comment = new Comment
            {
                VendorId = createCommentDto.VendorId,
                CustomerId = createCommentDto.CustomerId,
                CommentText = createCommentDto.CommentText,
                Rating = createCommentDto.Rating
            };

            await _commentRepository.AddAsync(comment);
            return new CommentDto
            {
                Id = comment.Id,
                VendorId = comment.VendorId,
                CustomerId = comment.CustomerId,
                CommentText = comment.CommentText,
                Rating = comment.Rating
            };
        }

        // Update the comment text
        public async Task<CommentDto> UpdateCommentAsync(string commentId, UpdateCommentDto updateCommentDto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(commentId);
            if (existingComment == null)
            {
                throw new KeyNotFoundException("Comment not found.");
            }

            existingComment.CommentText = updateCommentDto.CommentText;

            await _commentRepository.UpdateAsync(existingComment);

            return new CommentDto
            {
                Id = existingComment.Id,
                VendorId = existingComment.VendorId,
                CustomerId = existingComment.CustomerId,
                CommentText = existingComment.CommentText,
                Rating = existingComment.Rating
            };
        }

        // Get comments by vendor
        public async Task<IEnumerable<CommentDto>> GetCommentsByVendorIdAsync(string vendorId)
        {
            var comments = await _commentRepository.GetByVendorIdAsync(vendorId);
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                VendorId = c.VendorId,
                CustomerId = c.CustomerId,
                CommentText = c.CommentText,
                Rating = c.Rating
            }).ToList();
        }

        // Get comment by ID
        public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            return new CommentDto
            {
                Id = comment.Id,
                VendorId = comment.VendorId,
                CustomerId = comment.CustomerId,
                CommentText = comment.CommentText,
                Rating = comment.Rating
            };
        }
    }
}
