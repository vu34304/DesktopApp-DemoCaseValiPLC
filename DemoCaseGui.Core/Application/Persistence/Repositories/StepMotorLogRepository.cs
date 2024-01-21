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
    public class StepMotorLogRepository: IStepMotorLogRepository
    {
        public async Task<IEnumerable<StepMotorLog>> GetListAsync(TimeRangeQuery query, string name)
        {
            List<StepMotorLog> logs = new();
            using (var context = new KEP_Server_DBContext())
            {
                logs = await context.StepMotorLogs
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
