using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class SuppliersStore
{
    public int SupplierId { get; set; }

    public int? PaymentSupplierId { get; set; }

    public string CompanyName { get; set; }

    public string Tel { get; set; }

    public string Fax { get; set; }

    public string Email { get; set; }

    public string Addr { get; set; }

    public string PostCode { get; set; }

    public string SafetyLicense { get; set; }

    public DateTime? Date { get; set; }

    public int StoreId { get; set; }

    public Guid Rowguid { get; set; }
}
