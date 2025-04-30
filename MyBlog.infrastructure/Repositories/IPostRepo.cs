using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure.Repositories
{
    public interface IPostRepo
    {
        public Task<IEnumerable<Post>> GetAllPosts();
    }
}
