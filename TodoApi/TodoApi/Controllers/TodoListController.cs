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

        public TodoListController(TodoDbContext context)
        {
            _context = context;
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(TodoList list)
        {
            _context.TodoLists.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoList", new { id = list.ID }, list);
        }

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

        private bool TodoListExists(int id)
        {
            return _context.TodoLists.Any(l => l.ID == id);
        }
    }
}
