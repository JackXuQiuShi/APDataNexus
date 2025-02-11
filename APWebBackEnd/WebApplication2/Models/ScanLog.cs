using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ScanLog
{
    public int Id { get; set; }

    public string Barcode { get; set; }

    public string NormalizedBarcode { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? StoreId { get; set; }

    public Guid Rowguid { get; set; }
}
