using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetMentoringProgram_WebApi.Context;
using dotNetMentoringProgram_WebApi.Models;

namespace ShopWdotNetMentoringProgram_WebApiebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoryController(NorthwindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var allCategories = await _context.Categories.ToListAsync();
            return Ok(allCategories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GetCategories));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Category>> Edit(int id, [FromBody] Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (!_context.Categories.Any(x => x.CategoryId == id))
            {
                return NotFound();
            }

            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            var categoryInDb = await _context.Categories.FindAsync(id);
            if (categoryInDb == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categories.Remove(categoryInDb);
                await _context.SaveChangesAsync();

                return categoryInDb;
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
