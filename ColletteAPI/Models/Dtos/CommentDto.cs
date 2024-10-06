// CommentDtos.cs
// DTOs for handling comment data transfers.

namespace ColletteAPI.Models.Dtos
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCommentDto
    {
        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
    }

    public class UpdateCommentDto
    {
        public string CommentText { get; set; }
        public int Rating { get; set; }
    }
}
