using DemoCaseGui.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Communication
{
    public interface IExcelExporter
    {
        void ExportReport(string filePath, IEnumerable<FilterEntry> filters);
    }
}
