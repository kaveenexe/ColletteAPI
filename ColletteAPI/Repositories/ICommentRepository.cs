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
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task<Comment> GetByIdAsync(string commentId);
        Task<IEnumerable<Comment>> GetByVendorIdAsync(string vendorId);
    }
}
