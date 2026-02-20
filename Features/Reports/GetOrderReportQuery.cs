using MediatR;

namespace Warehouse.Features.Reports
{
    public record GetOrderReportQuery(int? Id) : IRequest<byte[]>;
}
