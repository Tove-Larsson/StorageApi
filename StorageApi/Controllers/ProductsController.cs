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
	[HttpPut("{id}")]
	public async Task<IActionResult> PutProduct(int id, UpdateProductDto updateDto)
	{
		var entity = await _context.Product.FindAsync(id);

		if (entity == null)
		{
			return NotFound();
		}

		entity.Name = updateDto.Name;
		entity.Price = updateDto.Price;
		entity.Count = updateDto.Count;
		entity.Category = updateDto.Category;
		entity.Shelf = updateDto.Shelf;
		entity.Description = updateDto.Description;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			return Conflict();
		}

		return NoContent();
	}

	// POST: api/Product
	[HttpPost]
	public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto createDto)
	{
		var entity = new Product
		{
			Name = createDto.Name,
			Price = createDto.Price,
			Category = createDto.Category,
			Shelf = createDto.Shelf,
			Count = createDto.Count,
			Description = createDto.Description
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
	public async Task<IActionResult> DeleteProduct(int id)
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
}
