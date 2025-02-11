using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductRequest
{
    public DateTime? RequestDate { get; set; }

    public int? RequestStoreId { get; set; }

    public string Applicant { get; set; }

    public short? Online { get; set; }

    public int DepartmentId { get; set; }

    public int CheckDigit { get; set; }

    public string ProductFullName { get; set; }

    public string ProductName { get; set; }

    public string ProductAlias { get; set; }

    public string PackageSpec { get; set; }

    public string Measure { get; set; }

    public double NumPerPack { get; set; }

    public int Tax1App { get; set; }

    public decimal UnitCost { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal PromotionPrice { get; set; }

    public decimal CaseCost { get; set; }

    public decimal? VolumeUnitCost { get; set; }

    public double? MinBulkVolume { get; set; }

    public string CountryOfOrigin { get; set; }

    public string Ethnic { get; set; }

    public int SupplierId { get; set; }

    public int StatusId { get; set; }

    public string ProductId { get; set; }

    public double UnitSize { get; set; }

    public string UnitSizeUom { get; set; }

    public int Tax2App { get; set; }

    public int? BuyerId { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Supplier Supplier { get; set; }
}
