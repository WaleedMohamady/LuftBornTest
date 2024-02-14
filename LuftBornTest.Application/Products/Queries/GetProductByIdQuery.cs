using MediatR;
using Microsoft.EntityFrameworkCore;
using LuftBornTest.Application.Interfaces;
using LuftBornTest.Application.Products.Models;

namespace LuftBornTest.Application.Products.Queries;
public class GetProductByIdQuery : IRequest<ProductDto>
{
    public Guid ProductId { get; set; }
    public class Handler(ILuftBornTestDbContext context) : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ILuftBornTestDbContext _context = context;
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            { 
                var product = await _context.Products
                    .Where(p => p.Id == request.ProductId)
                    .Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageURL = p.ImageURL,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return product;
            }
            catch
            {
                return null;
            }
        }
    }
}
