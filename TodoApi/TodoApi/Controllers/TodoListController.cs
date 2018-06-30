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
    public class TodoListController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoListController(TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<TodoList>> GetAll()
        {
            return _context.TodoLists.ToList();
        }

        [HttpGet("{id}", Name = "GetTodoList")]
        public async Task<ActionResult<TodoList>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TodoList datList = await _context.TodoLists.FindAsync(id);
            var todos = _context.TodoItems.Where(l => l.DatListID == id).ToList();
            datList.TodoItems = todos;

            if (datList == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Update(long id, TodoList list)
        {
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
        public async Task<IActionResult> Delete(long id)
        {
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
