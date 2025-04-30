using Microsoft.EntityFrameworkCore.Storage;
using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure.Repositories
{
    public class UintOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public IRepository<Post> Posts { get; private set; } // interface
        public IRepository<Category> Categories { get; private set; } // interface
        public IRepository<Comment> Comments { get; private set; } // interface
        public IPostRepo MyPostRepo { get; private set; } // interface

        public UintOfWork(AppDbContext context)
        {
            _context = context;
            Posts = new Repository<Post>(_context); // class
            Categories = new Repository<Category>(_context); // class
            Comments = new Repository<Comment>(_context); // class
            MyPostRepo = new PostRepo(_context); // class
        }


        public int Complte()
        {
            return _context.SaveChanges();
        }
        public async Task<int> ComplteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        // transaction

        public async Task BeginTransactionAsync()
        {
            if(_transaction is null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }
        // commit transaction  save in DB

        public async Task CommitTransactionAsync()
        {
            try
            {
                if(_transaction is not null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }


        // rollback transaction  undo changes
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction is not null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }
        public void Dispose()
        {
            _context.Dispose();
            //_transaction?.Dispose();
        }


    }
}
