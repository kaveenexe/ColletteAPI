// CommentsController.cs
// API controller for managing comments.

using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: api/comment
        [HttpPost]
        public async Task<IActionResult> AddCommentAsync([FromBody] CreateCommentDto createCommentDto)
        {
            var result = await _commentService.AddCommentAsync(createCommentDto);
            return Ok(result);
        }

        

        // GET: api/Comment/vendor/{vendorId}
        // Retrieves all comments for a specific vendor by vendor ID
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetCommentsByVendorId(string vendorId)
        {
            try
            {
                var result = await _commentService.GetCommentsByVendorIdAsync(vendorId);
                if (result == null || !result.Any())
                {
                    return NotFound($"No comments found for vendor with ID {vendorId}.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Comment/{commentId}
        // Retrieves a comment by its ID
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(string commentId)
        {
            try
            {
                var result = await _commentService.GetCommentByIdAsync(commentId);
                if (result == null)
                {
                    return NotFound($"Comment with ID {commentId} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Comment/{commentId}
        // Updates the comment text only (rating cannot be modified)
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(string commentId, UpdateCommentDto updateCommentDto)
        {
            try
            {
                var result = await _commentService.UpdateCommentAsync(commentId, updateCommentDto);
                if (result == null)
                {
                    return BadRequest("Failed to update comment or rating cannot be modified.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
