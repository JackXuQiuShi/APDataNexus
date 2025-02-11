using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class WarehouseCategory
{
    public int WarehouseCategoryId { get; set; }

    public string WarehouseCategoryName { get; set; }

    public Guid Rowguid { get; set; }
}
