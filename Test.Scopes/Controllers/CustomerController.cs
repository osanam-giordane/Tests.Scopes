using Microsoft.AspNetCore.Mvc;
using Test.Scopes.Domain.Interfaces;
using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;
using Test.Scopes.Domain.Models.Customers.UpdateCustomer;

namespace Test.Scopes.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class CustomerController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICreateCustomerHandler handler,
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customerId = await handler.Handle(request, cancellationToken);
        return Ok(customerId);
    }

    [HttpPut("{customerId}")]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateCustomerHandler handler,
        [FromRoute] long customerId,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        await handler.Handle(customerId, request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{customerId}")]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteCustomerHandler handler,
        [FromRoute] long customerId,
        CancellationToken cancellationToken)
    {
        await handler.Handle(customerId, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Recover(
        [FromServices] IRecoverCustomersHandler handler,
        CancellationToken cancellationToken)
    {
        var customers = await handler.Handle(cancellationToken);
        return Ok(customers);
    }

    [HttpPatch]
    [Route("{customerId}/activate")]
    public async Task<IActionResult> Activate(
        [FromServices] IActivateCustomerHandler handler, 
        [FromRoute] long customerId,
        CancellationToken cancellationToken)
    {
        await handler.Handle(customerId, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{customerId}/inactivate")]
    public async Task<IActionResult> Inactivate(
        [FromServices] IInactivateCustomerHandler handler,
        [FromRoute] long customerId,
        CancellationToken cancellationToken)
    {
        await handler.Handle(customerId, cancellationToken);
        return NoContent();
    }
}