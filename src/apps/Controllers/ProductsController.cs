using AuthApp.Domain.Entities;
using AuthApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// Get all products with pagination
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ServiceResult<Product>>> GetProducts([FromQuery] string query, CancellationToken cancellationToken)
    {
        var products = new List<Product>()
        {
            new Product()
            {
                Id = 1,
            }
        };
        return Ok(ServiceResult.Success(products));
    }
}
