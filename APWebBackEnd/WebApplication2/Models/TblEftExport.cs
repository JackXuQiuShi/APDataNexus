using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblEftExport
{
    public int Id { get; set; }

    public int? EftId { get; set; }

    public string Eftinfo { get; set; }

    public string InvoiceNumber { get; set; }

    public Guid Rowguid { get; set; }
}
