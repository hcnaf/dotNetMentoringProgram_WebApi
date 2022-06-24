using dotNetMentoringProgram_WebApi.Context;
using dotNetMentoringProgram_WebApi.Mappers;
using dotNetMentoringProgram_WebApi.Models;
using dotNetMentoringProgram_WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static dotNetMentoringProgram_WebApi.QueryParamenters;

namespace dotNetMentoringProgram_WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVM>>> GetProducts([FromQuery] QueryParameters parameters)
        {
            var products = _context.Products.AsQueryable();

            if (parameters.CategoryId.HasValue)
            {
                products = products.Where(x => x.CategoryId == parameters.CategoryId.Value);
            }

            if (parameters.PageNumber.HasValue && parameters.PageSize.HasValue)
            {
                products = products
                    .Skip((parameters.PageNumber.Value - 1) * parameters.PageSize.Value)
                    .Take(parameters.PageSize.Value);
            }

            var productVMs = (await products.ToListAsync()).Select(x => x.ToProductVM());

            return Ok(productVMs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductVM>> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product.ToProductVM());
        }
        [HttpPost]
        public async Task<ActionResult<ProductVM>> Create([FromBody] ProductVM product)
        {
            try
            {
                await _context.Products.AddAsync(product.ToProduct());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetProducts));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> Edit(int id, [FromBody] ProductVM product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            if (!_context.Products.Any(x => x.ProductId == id))
            {
                return NotFound();
            }
            try
            {
                _context.Products.Update(product.ToProduct());
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductVM>> Delete(int id)
        {
            var productInDb = await _context.Products.FindAsync(id);
            if (productInDb == null)
            {
                return NotFound();
            }
            try
            {
                _context.Products.Remove(productInDb);
                await _context.SaveChangesAsync();
                return productInDb.ToProductVM();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
