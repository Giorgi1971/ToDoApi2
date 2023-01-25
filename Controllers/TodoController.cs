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


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Authorize]
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

