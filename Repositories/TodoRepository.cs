using System;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Db;
using TodoApp.Api.Db.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace TodoApp.Api.Repositories
{
    public interface ITodoRepository
    {
        Task InsertAsync(int userId, string title, string description, DateTime deadline);
        Task SaveChangesAsync();
        Task<IEnumerable<ToDoEntity>> GetTodos();
        Task<ToDoEntity> GetToDo(int todoId);
        Task<ToDoEntity> UpdateTodo(int userId, string title, string description, DateTime deadline);
        void DeleteTodo(int todoId);
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _db;

        public TodoRepository(AppDbContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<ToDoEntity>> GetTodos()
        {
            return await _db.Todos.ToListAsync();
        }


        public async Task<ToDoEntity> GetToDo(int todoId)
        {
            var result = await _db.Todos.FirstOrDefaultAsync(e => e.Id == todoId);
            if (result is null)
                return null;
            return result;
        }


        public async Task<ToDoEntity> UpdateTodo(
            int id, string title, string description, DateTime deadline
            )
        {
            var result = await _db.Todos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                result.DeadLine = deadline;
                result.Title = title;
                result.Description = description;

                await _db.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async void DeleteTodo(int todoId)
        {
            var result = await _db.Todos
                .FirstOrDefaultAsync(e => e.Id == todoId);
            if (result != null)
            {
                _db.Todos.Remove(result);
                await _db.SaveChangesAsync();
            }
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
            // saveChanges აქ რატომ არ ვაკეთებთ???
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
