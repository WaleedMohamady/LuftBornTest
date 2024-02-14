using MediatR;
using Microsoft.EntityFrameworkCore;
using LuftBornTest.Application.Interfaces;

namespace LuftBornTest.Application.Products.Commands.Delete;
public class DeleteProductCommand : IRequest<string>
{
    public Guid ProductId { get; set; }
    public class Handler(ILuftBornTestDbContext context) : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly ILuftBornTestDbContext _context = context;
        public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);
                return "Product has been deleted successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
