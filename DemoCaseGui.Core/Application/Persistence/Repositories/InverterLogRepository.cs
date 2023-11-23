using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemoCaseGui.Core.Application.Persistence.Repositories;
public class InverterLogRepository : IInverterLogRepository
{
    public async Task<IEnumerable<InverterLog>> GetListAsync(TimeRangeQuery query, string name)
    {
        List<InverterLog> logs = new();
        using (var context = new KEP_Server_DBContext())
        {
            logs = await context.InverterLogs
            .AsNoTracking()
            .Where(log =>
            log.Timestamp > query.StartTime && log.Timestamp < query.EndTime)
            .Where(log => log.Name == name)
            .ToListAsync();
        }

        return logs;
    }
}
