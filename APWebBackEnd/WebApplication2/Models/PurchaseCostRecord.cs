using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class PurchaseCostRecord
{
    public string ProductId { get; set; }

    public int CostType { get; set; }

    public decimal? UnitCost { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public double? MinVolume { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; }

    public decimal? CaseCost { get; set; }

    public double? UnitsPerPackage { get; set; }

    public string RequestedBy { get; set; }

    public string ApprovedBy { get; set; }

    public decimal? UnitCostOld { get; set; }

    public int? StatusId { get; set; }

    public Guid Rowguid { get; set; }
}
