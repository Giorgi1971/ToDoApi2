using System;
namespace TodoApp.Api.Db.Entity
{
    public class ToDoEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime CreateAt { get; set; }
        public TodoStatus Status { get; set; }
    }

    public enum TodoStatus
    {
        New,
        Done,
        Canceled
    }
}
