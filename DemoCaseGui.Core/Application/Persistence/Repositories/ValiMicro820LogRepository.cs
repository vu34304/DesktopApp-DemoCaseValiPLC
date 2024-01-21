using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Persistence.Repositories
{
    public class ValiMicro820LogRepository:IValiMicro820LogRepository
    {

        public async Task<IEnumerable<ValiMicro820Log>> GetListAsync(TimeRangeQuery query, string name)
        {
            List<ValiMicro820Log> logs = new();
            using (var context = new KEP_Server_DBContext())
            {
                logs = await context.ValiMicro820Logs
                .AsNoTracking()
                .Where(log =>
                log.Timestamp > query.StartTime && log.Timestamp < query.EndTime)
                .Where(log => log.Name == name)
                .ToListAsync();
            }

            return logs;
        }
    }
}
