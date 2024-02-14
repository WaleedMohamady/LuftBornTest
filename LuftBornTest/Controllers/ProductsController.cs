using MediatR;
using Microsoft.AspNetCore.Mvc;
using LuftBornTest.Application.Products.Commands.Create;
using LuftBornTest.Application.Products.Commands.Delete;
using LuftBornTest.Application.Products.Commands.Update;
using LuftBornTest.Application.Products.Models;
using LuftBornTest.Application.Products.Queries;
using LuftBornTest.Domain.Entities;

namespace LuftBornTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts(GetAllProductsQuery request)
    {
        try
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
        try
        {
            var response = await _mediator.Send(new GetProductByIdQuery
            {
                ProductId = id
            });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] Guid id, UpdateProductCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var response = await _mediator.Send(new DeleteProductCommand
            {
                ProductId = id
            });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
