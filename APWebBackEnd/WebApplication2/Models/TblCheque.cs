using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblCheque
{
    public int TransactionId { get; set; }

    public int? PayerId { get; set; }

    public int? ChequeNumber { get; set; }

    public int? SupplierId { get; set; }

    public string ProcessNumber { get; set; }

    public DateTime? ChequeDate { get; set; }

    public bool? ChequeCashed { get; set; }

    public DateTime? ProcessDate { get; set; }

    public decimal? ChequeAmount { get; set; }

    public decimal? ChequeGst { get; set; }

    public string Note { get; set; }

    public bool? ChequeToPrint { get; set; }

    public bool? ChequePrinted { get; set; }

    public bool? ChequeIssued { get; set; }

    public bool? ChequeVoid { get; set; }

    public bool? ChequeComplete { get; set; }

    public string ProcessedBy { get; set; }

    public string ChequeSendInfo { get; set; }

    public DateTime? ChequeSendDate { get; set; }

    public DateTime? ChequeCashedDate { get; set; }

    public bool? CashPaymentFlag { get; set; }

    public string ChequeNote { get; set; }

    public DateTime? ChequeDueDate { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public bool? Dead { get; set; }

    public Guid Rowguid { get; set; }
}
