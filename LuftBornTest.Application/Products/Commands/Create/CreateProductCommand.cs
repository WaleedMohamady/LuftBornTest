using LuftBornTest.Application.Interfaces;
using LuftBornTest.Domain.Entities;
using MediatR;

namespace LuftBornTest.Application.Products.Commands.Create;
public class CreateProductCommand : IRequest<string>
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageURL { get; set; } = "";
    public class Handler(ILuftBornTestDbContext context) : IRequestHandler<CreateProductCommand, string>
    {
        private readonly ILuftBornTestDbContext _context = context;
        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    ImageURL = request.ImageURL,
                };

                await _context.Products.AddAsync(newProduct, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return "Product has been saved successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
