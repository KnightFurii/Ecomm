using System;
using System.Runtime.CompilerServices;
using Core.Entities;
using Core.Interfaces;
using Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo): ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? types, string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand,types,sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductById(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product values)
    {
        repo.AddProduct(values);
        if(await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new{id = values.Id}, values);
        }
        return BadRequest("Error occured while creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product values)
    {
        if (values.Id != id || !ProductExists(id))
            return BadRequest("Error occured while updating product");

        repo.UpdateProduct(values);
        if( await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Error occured while updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductById(id);
        if(product == null) return NotFound();
        repo.DeleteProduct(product);
        if( await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Error occured while deleting product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }


}
