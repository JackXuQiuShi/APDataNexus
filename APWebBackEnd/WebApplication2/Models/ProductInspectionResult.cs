using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductInspectionResult
{
    public string ProductId { get; set; }

    public int StoreId { get; set; }

    public DateOnly InspectedDate { get; set; }

    public string ProductName { get; set; }

    public string ProductChineseName { get; set; }

    public string Department { get; set; }

    public DateOnly LastSalesDate { get; set; }

    public decimal? LastSalesUnitPrice { get; set; }

    public int? SupplierId { get; set; }

    public string SupplierName { get; set; }

    public decimal? LastPoreceivedUnitCost { get; set; }

    public DateOnly? LastPolastReceivedDate { get; set; }

    public int? Tax { get; set; }

    public string LocationName { get; set; }

    public int? StatusId { get; set; }

    public Guid Rowguid { get; set; }
}
