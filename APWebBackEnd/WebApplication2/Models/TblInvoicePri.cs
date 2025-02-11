using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblInvoicePri
{
    public int InvoiceId { get; set; }

    public int? PayerId { get; set; }

    public string InvoiceNumber { get; set; }

    public int? SupplierId { get; set; }

    public string ProcessNumber { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Gst { get; set; }

    public DateTime? ProcessDate { get; set; }

    public string Note { get; set; }

    public bool? InvoicePaid { get; set; }

    public bool? InvoiceToPay { get; set; }

    public string ProcessedBy { get; set; }

    public bool? InvoiceCashPaidout { get; set; }

    public DateTime? CashPayoutDate { get; set; }

    public bool? InvoiceToPrint { get; set; }

    public bool? InvoiceCashPayment { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public bool? CreditCard { get; set; }

    public bool? BankWired { get; set; }

    public bool? Appended { get; set; }

    public bool? Duplicated { get; set; }

    public string PoId { get; set; }

    public decimal? PoReceived { get; set; }

    public string PoState { get; set; }

    public int? StoreId { get; set; }

    public Guid Rowguid { get; set; }
}
