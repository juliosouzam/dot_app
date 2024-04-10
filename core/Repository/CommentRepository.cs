using core.Data;
using core.DTOs.Comments;
using core.Interfaces;
using core.Models;
using Microsoft.EntityFrameworkCore;

namespace core.Repository
{
    public class CommentRepository(ApplicationDBContext context) : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Comment?> CreateAsync(Comment model)
        {
            await _context.Comments.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var model = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == commentId);
            if (model == null)
            {
                return null;
            }
            _context.Comments.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.User).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int commentId, UpdateCommentRequest model)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return null;
            }
            comment.Title = model.Title;
            comment.Content = model.Content;
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}