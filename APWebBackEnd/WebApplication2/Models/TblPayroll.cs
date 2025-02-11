using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblPayroll
{
    public int PayrollId { get; set; }

    public int? PayerId { get; set; }

    public int? ChequeNumber { get; set; }

    public string ProcessNumber { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? PayrollDate { get; set; }

    public decimal? PayrollAmount { get; set; }

    public string PayrollNote { get; set; }

    public string ProcessedBy { get; set; }

    public DateTime? ProcessDate { get; set; }

    public bool? PayrollToPrint { get; set; }

    public bool? PayrollPrinted { get; set; }

    public bool? PayrollIssued { get; set; }

    public bool? PayrollCashed { get; set; }

    public bool? PayrollVoid { get; set; }

    public bool? PayrollComplete { get; set; }

    public DateTime? PayrollCashedDate { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public bool? Dead { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Employee Employee { get; set; }
}
