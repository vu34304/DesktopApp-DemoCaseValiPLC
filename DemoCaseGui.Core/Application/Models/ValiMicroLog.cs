using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Models;

public partial class ValiMicroLog
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Numericid { get; set; }
    public string? Value { get; set; }
    public DateTime Timestamp { get; set; }
    public int? Quality { get; set; }
}
