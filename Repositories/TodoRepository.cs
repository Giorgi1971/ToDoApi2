using System;
using TodoApp.Api.Db;
using TodoApp.Api.Db.Entity;

namespace TodoApp.Api.Repositories
{
    public interface ITodoRepository
    {
        Task InsertAsync(int userId, string title, string description, DateTime deadline);
        Task SaveChangesAsync();
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _db;

        public TodoRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task InsertAsync
            (
                int userId, string title, string description, DateTime deadline
            )
        {
            var entity = new ToDoEntity();
            entity.UserId = userId;
            entity.Title = title;
            entity.CreateAt = DateTime.UtcNow;
            entity.DeadLine = deadline;
            entity.Description = description;
            entity.Status = TodoStatus.New;
            entity.Title = title;

            await _db.Todos.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

