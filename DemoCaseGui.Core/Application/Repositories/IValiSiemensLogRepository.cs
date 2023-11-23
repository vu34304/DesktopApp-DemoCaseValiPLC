using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;

namespace DemoCaseGui.Core.Application.Repositories;
public interface IValiSiemensLogRepository
{
    Task<IEnumerable<ValiSiemensLog>> GetListAsync(TimeRangeQuery query, string name);
}
