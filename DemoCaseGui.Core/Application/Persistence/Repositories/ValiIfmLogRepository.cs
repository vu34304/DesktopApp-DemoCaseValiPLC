using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemoCaseGui.Core.Application.Persistence.Repositories;
public class ValiIfmLogRepository : IValiIfmLogRepository
{
    public async Task<IEnumerable<ValiIfmLog>> GetListAsync(TimeRangeQuery query, string name)
    {
        List<ValiIfmLog> logs = new ();
        using (var context = new KEP_Server_DBContext())
        {
            logs = await context.ValiIfmLogs
                .AsNoTracking()
                .Where(log => log.Timestamp > query.StartTime && log.Timestamp < query.EndTime)
                .Where(log => log.Name == name)
                .ToListAsync();
        }
     
        return logs;
    }
}
