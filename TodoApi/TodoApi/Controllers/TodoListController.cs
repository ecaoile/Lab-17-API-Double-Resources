using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TodoListController : ControllerBase
    {
        private readonly TodoDbContext _context;

        /// <summary>
        /// grabs the database to use for other methods in 
        /// the TodoListController
        /// </summary>
        /// <param name="context"></param>
        public TodoListController(TodoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// grabs all of the TodoList objects in the database
        /// </summary>
        /// <returns>a list of TodoList objects</returns>
        [HttpGet]
        public async Task<ActionResult<List<TodoList>>> GetAllAsync()
        {
            var demLists = await _context.TodoLists.ToListAsync();
            foreach (var item in demLists)
            {
                var todos = _context.TodoItems.Where(l => l.DatListID == item.ID).ToList();
            }
            return demLists;
        }

        /// <summary>
        /// grabs a specific TodoList object
        /// </summary>
        /// <param name="id">the ID of the TodoList object to view</param>
        /// <returns>the TodoList object with the matching ID</returns>
        [HttpGet("{id}", Name = "GetTodoList")]
        public async Task<ActionResult<TodoList>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TodoList datList = await _context.TodoLists.FindAsync(id);
            if (datList == null)
            {
                return NotFound();
            }

            var todos = _context.TodoItems.Where(l => l.DatListID == id).ToList();
            datList.TodoItems = todos;


            return datList;
        }

        /// <summary>
        /// creates a new TodoList object in the database
        /// </summary>
        /// <param name="list">the TodoList object to add</param>
        /// <returns>created object response with the added object</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TodoList list)
        {
            _context.TodoLists.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoList", new { id = list.ID }, list);
        }

        /// <summary>
        /// edit an existing TodoList object in the database
        /// </summary>
        /// <param name="id">the ID of the TodoList object to edit</param>
        /// <param name="list">the object with the properties to edit</param>
        /// <returns>an empty status code object</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoList list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ensures that the same list is being updated
            if (id != list.ID)
            {
                return BadRequest();
            }

            var datList = await _context.TodoLists.FindAsync(id);

            if (datList == null)
            {
                return NotFound();
            }

            datList.Name = list.Name;

            _context.TodoLists.Update(datList);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// deletes a TodoList object from the database
        /// </summary>
        /// <param name="id">the IE of the TodoList object to delete</param>
        /// <returns>an empty status code object</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var datList = await _context.TodoLists.FindAsync(id);

            if (datList == null)
            {
                return NotFound();
            }

            _context.TodoLists.Remove(datList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
