using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class DebugLog
{
    public int LogId { get; set; }

    public string LogMessage { get; set; }

    public DateTime? LogDateTime { get; set; }

    public string ProcedureName { get; set; }

    public string Severity { get; set; }

    public Guid Rowguid { get; set; }
}
