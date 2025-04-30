using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // my perpos
        IRepository<Post> Posts { get; }  // post table
        IRepository<Category> Categories { get; } // category table
        IRepository<Comment> Comments { get; } // comment table
        IPostRepo MyPostRepo { get; } // my post repo

        // defualt
        int Complte();
        Task<int> ComplteAsync();

        // transaction
        Task BeginTransactionAsync(); // start transaction
        Task CommitTransactionAsync(); // save in DB
        Task RollbackTransactionAsync(); // undo changes

    }

}
