using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductsPri
{
    public string Barcode { get; set; }

    public string Rank { get; set; }

    public string RecRank { get; set; }

    public string User { get; set; }

    public string RecDate { get; set; }

    public DateTime? ModTime { get; set; }

    public double? InStock { get; set; }

    public DateTime? InStockDate { get; set; }

    public string Rank1 { get; set; }

    public string Rank2 { get; set; }

    public DateOnly? LastSaleDate { get; set; }

    public DateOnly? LastInventoryDate { get; set; }

    public string Source { get; set; }

    public string SourceId { get; set; }

    public string SecondName { get; set; }

    public string LocationCode { get; set; }

    public string SubCategory { get; set; }

    public string Category { get; set; }

    public string GeneralCategory { get; set; }

    public string LocationCode2 { get; set; }

    public string CheckDigit { get; set; }

    public decimal? WmCost { get; set; }

    public int? WmCode { get; set; }

    public bool Active { get; set; }

    public double? WhInStock { get; set; }

    public string WmRetail { get; set; }

    public string WmUpc { get; set; }

    public Guid Rowguid { get; set; }
}
