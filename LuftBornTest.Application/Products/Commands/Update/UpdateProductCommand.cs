using MediatR;
using Microsoft.EntityFrameworkCore;
using LuftBornTest.Application.Interfaces;

namespace LuftBornTest.Application.Products.Commands.Update;
public class UpdateProductCommand : IRequest<string>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageURL { get; set; } = "";
    public class Handler(ILuftBornTestDbContext context) : IRequestHandler<UpdateProductCommand, string>
    {
        private readonly ILuftBornTestDbContext _context = context;
        public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updatedProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                updatedProduct.Name = request.Name;
                updatedProduct.Description = request.Description;
                updatedProduct.Price = request.Price;
                updatedProduct.ImageURL = request.ImageURL;

                await _context.SaveChangesAsync(cancellationToken);
                return "Product has been updated successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
