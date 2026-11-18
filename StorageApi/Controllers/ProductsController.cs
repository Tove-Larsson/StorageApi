using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Dto;
using StorageApi.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageApiContext _context;
    public ProductsController(StorageApiContext context)
    {
        _context = context;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct()
    {
        var products = await _context.Product
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Count = p.Count,
            })
            .ToListAsync();

        return products;
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {

        var product = await _context.Product
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Count = p.Count
            }).FirstOrDefaultAsync();
        
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int? id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Product
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto product)
    {
        var entity = new Product
        {
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            Shelf = product.Shelf,
            Count = product.Count,
            Description = product.Description
        };

        _context.Product.Add(entity);
        await _context.SaveChangesAsync();

		var productDto = new ProductDto
		{
			Id = entity.Id,
			Name = entity.Name,
			Price = entity.Price,
			Count = entity.Count
		};

		return CreatedAtAction("GetProduct", new { id = productDto.Id }, productDto);
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
