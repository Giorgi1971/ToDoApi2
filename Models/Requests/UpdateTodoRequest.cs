using System;
namespace TodoApp.Api.Models.Requests
{
    public class UpdateTodoRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Decsription { get; set; }
        public DateTime Deadline { get; set; }
    }
}