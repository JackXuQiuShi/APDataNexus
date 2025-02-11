using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APWeb.Models;

public partial class Price
{
    [Key]
    public string Prodnum { get; set; }

    public decimal? Order { get; set; }

    public decimal? OrderPromotion { get; set; }

    public decimal? OrderPromotionStore { get; set; }

    public decimal? SaleCeil { get; set; }

    public decimal? SaleFloor { get; set; }

    public decimal? Sale { get; set; }

    public decimal? SaleStore { get; set; }

    public decimal? SalePromotion { get; set; }

    public decimal? SalePromotionStore { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; }

    public double? MinVolume { get; set; }

    public decimal? VolumePrice { get; set; }

    public double? MinVolume2 { get; set; }

    public double? MinVolume3 { get; set; }

    public decimal? VolumePrice2 { get; set; }

    public decimal? VolumePrice3 { get; set; }

    public double? UnitsPerPackage { get; set; }

    public int? SupplierId { get; set; }

    public string Ethnicity { get; set; }

    public string ApprovedBy { get; set; }

    public Guid Rowguid { get; set; }
}
