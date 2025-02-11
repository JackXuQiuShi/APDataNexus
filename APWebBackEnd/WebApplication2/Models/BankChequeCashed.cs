using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class BankChequeCashed
{
    public double? PayerId { get; set; }

    public double? ChequeNumber { get; set; }

    public DateTime? ChequeCashedDate { get; set; }

    public double? Amount { get; set; }

    public int Id { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public Guid Rowguid { get; set; }
}
