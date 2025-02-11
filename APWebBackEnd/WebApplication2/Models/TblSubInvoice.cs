using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblSubInvoice
{
    public int SubInvoiceId { get; set; }

    public int CategoryId { get; set; }

    public int InvoiceId { get; set; }

    public decimal SubAmount { get; set; }

    public string SubInvoiceNote { get; set; }

    public decimal? Gst { get; set; }

    public string ProcessedBy { get; set; }

    public DateTime? Date { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Category Category { get; set; }

    public virtual TblInvoice Invoice { get; set; }
}
