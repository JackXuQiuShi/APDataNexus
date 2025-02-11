using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblPayerCompany
{
    public int PayerId { get; set; }

    public string PayerCompany { get; set; }

    public string ContactName { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public string FaxNumber { get; set; }

    public int? StoreIdPris { get; set; }

    public string ClientName { get; set; }

    public string ClientNumber { get; set; }

    public Guid Rowguid { get; set; }

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();
}
