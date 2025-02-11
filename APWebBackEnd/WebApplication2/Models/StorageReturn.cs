using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class StorageReturn
{
    public string ProductId { get; set; }

    public int? SupplierId { get; set; }

    public int? ReturnQuantity { get; set; }

    public int StoreId { get; set; }

    public DateTime Date { get; set; }

    public int StatusId { get; set; }

    public int ReturnId { get; set; }

    public float? UnitCost { get; set; }

    public bool? Tax { get; set; }

    public DateTime? PrintDate { get; set; }

    public string ProductName { get; set; }

    public string CreditNumber { get; set; }

    public Guid Rowguid { get; set; }
}
