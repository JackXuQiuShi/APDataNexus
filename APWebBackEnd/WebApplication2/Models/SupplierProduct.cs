using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class SupplierProduct
{
    public string ProductId { get; set; }

    public int SupplierId { get; set; }

    public Guid Rowguid { get; set; }
}
