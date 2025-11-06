using Core.Entities;
using Core.Interfaces;
using InfraStructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductsRepository productsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await productsRepository.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productsRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        productsRepository.AddProduct(product);
        if (await productsRepository.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Hiba történt a termék létrehozásakor.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody]Product updatedProduct)
    {
        if (id != updatedProduct.Id || !productsRepository.ProductExists(id))
        {
            return BadRequest("Nem létező termék azonosító vagy azonosító nem egyezik.");
        }

        productsRepository.UpdateProduct(updatedProduct);

        if (!await productsRepository.SaveChangesAsync())
        {
            return BadRequest("Hiba történt a termék frissítésekor.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productsRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return BadRequest("Nem létező termék azonosító.");
        }

        productsRepository.DeleteProduct(product);
        if (await productsRepository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Hiba történt a termék törlésekor.");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await productsRepository.GetBrandsAsync();
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await productsRepository.GetTypesAsync();
        return Ok(types);
    }
}
