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

internal class ValiMicroLogRepository: IValiMicroLogRepository
{
    public async Task<IEnumerable<ValiMicroLog>> GetListAsync(TimeRangeQuery query, string name)
    {
        List<ValiMicroLog> logs = new();
        using (var context = new KEP_Server_DBContext())
        {
            logs = await context.ValiMicroLogs
                .AsNoTracking()
                .Where(log => log.Name == name)
                .Where(log => log.Timestamp >= query.StartTime && log.Timestamp <= query.EndTime)
                .ToListAsync();
        }
        return logs;
    }
}
