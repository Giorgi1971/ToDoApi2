using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Auth;
using TodoApp.Api.Models.Requests;
using TodoApp.Api.Db.Entity;
using TodoApp.Api.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TodoController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ITodoRepository _todoRepository;

        public TodoController(
            UserManager<UserEntity> userManager,
            ITodoRepository todoRepository
            )
        {
            _userManager = userManager;
            _todoRepository = todoRepository;
        }


        [HttpGet]
        public async Task<ActionResult> GetTodos()
        {
            try
            {
                return Ok(await _todoRepository.GetTodos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        // GET api/values/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ToDoEntity>> GetToDo(int id)
        {
            try
            {
                var result = await _todoRepository.GetToDo(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        // POST api/values
        //[Authorize]
        //[Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateToDo([FromBody] CreateTodoRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not Found");

            var userId = user.Id;

            //var userId = 2;
            await _todoRepository.InsertAsync(userId, request.Title, request.Decsription, request.Deadline);
            await _todoRepository.SaveChangesAsync();
            return Ok();
        }


        // PUT api/values/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ToDoEntity>> UpdateEmployee([FromBody] UpdateTodoRequest request)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound("User not Found");

                var userId = user.Id;

                var todoToUpdate = await _todoRepository.GetToDo(request.Id);
                if (todoToUpdate == null)
                    return NotFound($"Employee with Id = {request.Id} not found");

                // ეს გვჭირდება ???
                if (userId != todoToUpdate.UserId)
                    return BadRequest("Employee ID mismatch");

                return await _todoRepository.UpdateTodo(request.Id, request.Title, request.Decsription, request.Deadline);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ToDoEntity>> DeleteToDoEntity(int id)
        {
            try
            {
                var todoToDelete = await _todoRepository.GetToDo(id);

                if (todoToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                _todoRepository.DeleteTodo(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}

