// CommentController.cs
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
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createCommentDto)
        {
            var result = await _commentService.AddComment(createCommentDto);
            return Ok(result);
        }

        // GET: api/comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var result = await _commentService.GetCommentById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // GET: api/comment/vendor/{vendorId}
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetCommentsByVendorId(string vendorId)
        {
            var results = await _commentService.GetCommentsByVendorId(vendorId);
            return Ok(results);
        }

        // PUT: api/comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var customerId = "Get this from authenticated user context"; // You need to fetch the customer ID from auth context.
            var result = await _commentService.UpdateComment(id, customerId, updateCommentDto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
