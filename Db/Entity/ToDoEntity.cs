using System;
namespace TodoApp.Api.Db.Entity
{
    public class ToDoEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public string Status { get; set; }
    }
}

