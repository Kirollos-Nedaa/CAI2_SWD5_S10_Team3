using Microsoft.EntityFrameworkCore;
using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure.Repositories
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _context;
        public PostRepo(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Post>> GetAllPosts()
        //{
        //    return await _context.Posts
        //        .Include(c => c.Category)
        //        //.Include(c => c.Comments)
        //        //.ThenInclude(c => c.User)
        //        .ToListAsync();

        //    //var post = await _context.Posts.FindAsync();



        //    ////await _context.Entry(post).Reference(p => p.Category).LoadAsync();
        //    ////await _context.Entry(post).Collection(p => p.Comments).LoadAsync();


        //    //return post;
        //}

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.Include(p => p.Category).ToListAsync();
        }
    }
}
