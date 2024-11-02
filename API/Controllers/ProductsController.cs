using System;
using System.Runtime.CompilerServices;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericrepository<Product> repo): ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? types, string? sort)
    {
        var spec = new ProductSpecification(brand,types,sort);
        var products = await repo.ListAsync(spec);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetbByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product values)
    {
        repo.Add(values);
        if(await repo.SaveAllAsync())
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

        repo.Update(values);
        if( await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Error occured while updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetbByIdAsync(id);
        if(product == null) return NotFound();
        repo.Remove(product);
        if( await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Error occured while deleting product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok( await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return repo.Exists (id);
    }


}
