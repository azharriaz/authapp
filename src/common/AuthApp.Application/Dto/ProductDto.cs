using AuthApp.Domain.Entities;
using Mapster;

namespace AuthApp.Application.Dto;

public class ProductDto : IRegister
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>();
    }
}
