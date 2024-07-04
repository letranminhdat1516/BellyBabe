using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.BlogRepository
{
    public class BlogRepository
    {
        private readonly Swp391Context _context;

        public BlogRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddBlog(int? userId, string? blogContent, int? categoryId, string? titleName, string? image)
        {
            var newBlog = new Blog
            {
                UserId = userId,
                BlogContent = blogContent,
                CategoryId = categoryId,
                TitleName = titleName,
                Image = image
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Blog>> GetBlogsByUserIdAsync(int userId)
        {
            return await _context.Blogs.Where(b => b.UserId == userId).ToListAsync();
        }
        public async Task DeleteBlog(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBlog(int blogId, int? userId, string? blogContent, int? categoryId, string? titleName)
        {
            var blog = await _context.Blogs.FindAsync(blogId);

            if (blog == null)
            {
                throw new ArgumentException("Blog không tồn tại.");
            }

            if (titleName != null)
            {
                if (string.IsNullOrWhiteSpace(titleName) || titleName.Length > 100)
                {
                    throw new ArgumentException("Tên tiêu đề không được để trống và phải dưới 100 ký tự.");
                }

            }

            blog.UserId = userId ?? blog.UserId;
            blog.BlogContent = blogContent ?? blog.BlogContent;
            blog.CategoryId = categoryId ?? blog.CategoryId;
            blog.TitleName = titleName ?? blog.TitleName;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _context.Blogs
               // .Include(b => b.Category)
               // .Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Blog?> GetBlogById(int blogId)
        {
            return await _context.Blogs
                //.Include(b => b.Category)
                //.Include(b => b.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BlogId == blogId);
        }

        public async Task<List<Blog>> GetBlogsByCategoryId(int categoryId)
        {
            return await _context.Blogs
                .Where(b => b.CategoryId == categoryId)
                //.Include(b => b.Category)
                //.Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsByUserId(int userId)
        {
            return await _context.Blogs
                .Where(b => b.UserId == userId)
                //.Include(b => b.Category)
                //.Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
