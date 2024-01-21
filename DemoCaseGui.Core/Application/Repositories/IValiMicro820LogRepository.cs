using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Persistence.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Repositories
{
    public interface IValiMicro820LogRepository
    {
        Task<IEnumerable<ValiMicro820Log>> GetListAsync(TimeRangeQuery query, string name);
    }
}
