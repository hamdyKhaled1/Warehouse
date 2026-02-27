using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Reports
{
    public record GetOrderReportQuery(int? Id) : IRequest<Result<byte[]>>;
}
