using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Persistence.Repositories;

internal class ValiCompactLogRepository: IValiCompactLogRepository
{
    public async Task<IEnumerable<ValiCompactLog>> GetListAsync(TimeRangeQuery query, string name)
    {
        List<ValiCompactLog> logs = new();
        using (var context = new KEP_Server_DBContext())
        {
            logs = await context.ValiCompactLogs
                .AsNoTracking()
                .Where(log => log.Name == name)
                .Where(log => log.Timestamp >= query.StartTime && log.Timestamp <= query.EndTime)
                .ToListAsync();
        }
        return logs;
    }
}
