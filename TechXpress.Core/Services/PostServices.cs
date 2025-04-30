using MyBlog.domain.Models;
using MyBlog.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.core.Services
{
    public class PostServices
    {
        //private readonly IRepository<Post> _postRepo;
        //private readonly IPostRepo _IpostRepo;
        private readonly Action<string> _logAction;
        private readonly IUnitOfWork _unitOfWork;
        public PostServices(/*IRepository<Post> postRepo,IPostRepo IpostRepo*/ IUnitOfWork unitOfWork)
        {
            //_postRepo = postRepo;
            //_IpostRepo = IpostRepo;
            _logAction = message => Console.WriteLine($"LOG: {message}");
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Post>> GetAllIPostsAsync()
        {
            //return await _IpostRepo.GetAllPosts();
            return await _unitOfWork.MyPostRepo.GetAllPosts();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(
            Expression<Func<Post,bool>> criteria = null, // where
            Expression<Func<Post, object>>[] includes = null // include
            ){
            //return await _postRepo.GetAllAsync();
            //return await _postRepo.GetAllAsync(criteria,includes);
            return await _unitOfWork.Posts.GetAllAsync(criteria, includes);
        }
        public async Task<Post> GetPostAsync(int id)
        {
            //return await _postRepo.GetByIdAsync(id);
            return await _unitOfWork.Posts.GetByIdAsync(id);
        }
        public async Task AddPostAsync(Post post)
        {
             //await _postRepo.AddAsync(post,_logAction);
             await _unitOfWork.Posts.AddAsync(post,_logAction);
        }
        public async Task UpdatePostAsync(Post post)
        {
            //await _postRepo.UpdateAstnc(post, _logAction);
            await _unitOfWork.Posts.UpdateAstnc(post, _logAction);
        }
        public async Task DeletePostAsync(int id)
        {
            //await _postRepo.DeleteAsync(id, _logAction);
            await _unitOfWork.Posts.DeleteAsync(id, _logAction);
        }
    }
}
