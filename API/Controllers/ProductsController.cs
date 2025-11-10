using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using InfraStructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> productsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);

        var products = await productsRepository.ListAsync(spec);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productsRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        productsRepository.Add(product);
        if (await productsRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Hiba történt a termék létrehozásakor.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody]Product updatedProduct)
    {
        if (id != updatedProduct.Id || !productsRepository.Exists(id))
        {
            return BadRequest("Nem létező termék azonosító vagy azonosító nem egyezik.");
        }

        productsRepository.Update(updatedProduct);

        if (!await productsRepository.SaveAllAsync())
        {
            return BadRequest("Hiba történt a termék frissítésekor.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productsRepository.GetByIdAsync(id);
        if (product == null)
        {
            return BadRequest("Nem létező termék azonosító.");
        }

        productsRepository.Remove(product);
        if (await productsRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Hiba történt a termék törlésekor.");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await productsRepository.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await productsRepository.ListAsync(spec));
    }
}
