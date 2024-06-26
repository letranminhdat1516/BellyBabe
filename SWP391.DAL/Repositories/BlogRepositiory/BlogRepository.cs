﻿using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
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

        public async Task AddBlog(int? userId, string? blogContent, int? categoryId, string? titleName, DateTime? dateCreated)
        {
            var newBlog = new Blog
            {
                UserId = userId,
                BlogContent = blogContent,
                CategoryId = categoryId,
                TitleName = titleName,
                DateCreated = dateCreated
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();
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

        public async Task UpdateBlog(int blogId, Dictionary<string, object> updates)
        {
            var blog = await _context.Blogs.FindAsync(blogId);

            if (blog != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "userId":
                            blog.UserId = (int?)update.Value;
                            break;
                        case "blogContent":
                            blog.BlogContent = (string?)update.Value;
                            break;
                        case "categoryId":
                            blog.CategoryId = (int?)update.Value;
                            break;
                        case "titleName":
                            blog.TitleName = (string?)update.Value;
                            break;
                        case "dateCreated":
                            blog.DateCreated = (DateTime?)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Blog?> GetBlogById(int blogId)
        {
            return await _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BlogId == blogId);
        }

        public async Task<List<Blog>> GetBlogsByCategoryId(int categoryId)
        {
            return await _context.Blogs
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Category)
                .Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsByUserId(int userId)
        {
            return await _context.Blogs
                .Where(b => b.UserId == userId)
                .Include(b => b.Category)
                .Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
