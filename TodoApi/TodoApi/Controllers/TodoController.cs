using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _context;
        /// <summary>
        /// grabs the database to use for method in the TodoController
        /// and creates a dummy todo item if no other item exists
        /// </summary>
        /// <param name="context">the database to use</param>
        public TodoController(TodoDbContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// displays all todo items
        /// </summary>
        /// <returns>all TodoItem objects in the database</returns>
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        /// <summary>
        /// displays a specific todo item based on id input
        /// </summary>
        /// <param name="id">the id of the todo item to display</param>
        /// <returns>the Todo object</returns>
        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<ActionResult<TodoItem>> GetById(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        /// <summary>
        /// creates a new todo item in the database
        /// </summary>
        /// <param name="item">the TodoItem object to create in the database</param>
        /// <returns>the newly added TodoItem object</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodo", new { id = item.ID }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, TodoItem item)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (id != item.ID)
            {
                return BadRequest();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            todo.DatListID = item.DatListID;

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
