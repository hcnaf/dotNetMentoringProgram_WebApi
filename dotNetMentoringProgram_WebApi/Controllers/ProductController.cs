using dotNetMentoringProgram_WebApi.Context;
using dotNetMentoringProgram_WebApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Product>> GetProducts(int pageNumber = 1, int pageSize = 10,
            int? categoryId = null)
        {
            IQueryable<Product> products = _context.Products;
            if (categoryId != null)
            {
                products = products.Where(x => x.CategoryId == categoryId);
            }

            products = products
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return Ok(products.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public ActionResult PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var toUpdate = _context.Products.Find(id);
            if (toUpdate != null)
            {
                _context.Entry(toUpdate).CurrentValues.SetValues(product);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
