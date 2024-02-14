using MediatR;
using Microsoft.EntityFrameworkCore;
using LuftBornTest.Application.Interfaces;
using LuftBornTest.Application.Products.Models;

namespace LuftBornTest.Application.Products.Queries;
public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
    public class Handler(ILuftBornTestDbContext context) : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly ILuftBornTestDbContext _context = context;
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _context.Products
                    .Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageURL = p.ImageURL,  
                    })
                    .ToListAsync(cancellationToken);

                return products;
            }
            catch
            {
                return [];
            }
        }
    }
}
