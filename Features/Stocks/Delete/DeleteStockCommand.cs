using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Stocks.Delete
{
    public record DeleteStockCommand(int Id) : IRequest<Result<bool>>;
}
