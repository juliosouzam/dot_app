using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.DTOs.Comments;
using core.Models;

namespace core.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment?> CreateAsync(Comment model);

        Task<Comment?> UpdateAsync(int commentId, UpdateCommentRequest model);

        Task<Comment?> DeleteAsync(int commentId);
    }
}