using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblBonu
{
    public int BonusId { get; set; }

    public int? PayerId { get; set; }

    public int? ChequeNumber { get; set; }

    public string ProcessNumber { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? BonusDate { get; set; }

    public decimal? BonusAmount { get; set; }

    public string BonusNote { get; set; }

    public string ProcessedBy { get; set; }

    public DateTime? ProcessDate { get; set; }

    public bool? BonusToPrint { get; set; }

    public bool? BonusPrinted { get; set; }

    public bool? BonusIssued { get; set; }

    public bool? BonusCashed { get; set; }

    public bool? BonusVoid { get; set; }

    public bool? BonusComplete { get; set; }

    public DateTime? BonusCashedDate { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public bool? Dead { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Employee Employee { get; set; }
}
