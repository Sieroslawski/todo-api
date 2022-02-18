using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DatabaseHelper;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GetAll() is automatically recognized as
        // http://localhost:<port #>/api/todo
        [HttpGet]
        public IEnumerable<ToDo> GetAll()
        {
            return _context.ToDos.ToList();
        }

        // GetById() is automatically recognized as
        // http://localhost:<port #>/api/todo/{id}

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var item = _context.ToDos.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ToDo todo)
        {
            if (todo.Description == null || todo.Description == "")
            {
                return BadRequest();
            }
            _context.ToDos.Add(todo);
            _context.SaveChanges();
            return new ObjectResult(todo);
        }

        [HttpPut]
        [Route("myedit")] // Custom route
        public IActionResult GetByParams([FromBody] ToDo todo)
        {
            var item = _context.ToDos.Where(t => t.Id == todo.Id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.IsComplete = todo.IsComplete;
                item.Description = todo.Description;
                _context.SaveChanges();
            }
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult MyDelete(int id)
        {
            var item = _context.ToDos.Where(t => t.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            _context.ToDos.Remove(item);
            _context.SaveChanges();
            return new ObjectResult(item);
        }



    }

}
