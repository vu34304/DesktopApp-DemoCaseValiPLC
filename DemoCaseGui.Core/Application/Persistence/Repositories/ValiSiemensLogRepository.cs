using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemoCaseGui.Core.Application.Persistence.Repositories;
public class ValiSiemensLogRepository : IValiSiemensLogRepository
{
    public async Task<IEnumerable<ValiSiemensLog>> GetListAsync(TimeRangeQuery query, string name)
    {
        List<ValiSiemensLog> logs = new();
        using (var context = new KEP_Server_DBContext())
        {
            logs = await context.ValiSiemensLogs
                .AsNoTracking()
                .Where(log => log.Name == name)
                .Where(log => log.Timestamp >= query.StartTime && log.Timestamp <= query.EndTime)
                .ToListAsync();
        }
        return logs;
    }
}
