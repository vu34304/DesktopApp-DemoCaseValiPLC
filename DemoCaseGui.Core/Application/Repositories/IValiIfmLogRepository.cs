using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;

namespace DemoCaseGui.Core.Application.Repositories;
public interface IValiIfmLogRepository
{
    Task<IEnumerable<ValiIfmLog>> GetListAsync(TimeRangeQuery query, string name);
}
