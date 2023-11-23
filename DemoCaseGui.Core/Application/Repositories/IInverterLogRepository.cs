using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;

namespace DemoCaseGui.Core.Application.Repositories;
public interface IInverterLogRepository
{
    Task<IEnumerable<InverterLog>> GetListAsync(TimeRangeQuery query, string name);
}
