using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductsStore
{
    public string ProdNum { get; set; }

    public string ProdName { get; set; }

    public string ProdDesc { get; set; }

    public int? Service { get; set; }

    public decimal UnitCost { get; set; }

    public string Measure { get; set; }

    public string Warranty { get; set; }

    public decimal? TotSales { get; set; }

    public decimal? TotProfit { get; set; }

    public int? OnSales { get; set; }

    public string Barcode { get; set; }

    public string ProdAlias { get; set; }

    public int? SerialControl { get; set; }

    public short? Tax1App { get; set; }

    public short? Tax2App { get; set; }

    public short? Tax3App { get; set; }

    public decimal? ItemBonus { get; set; }

    public int? SalePoint { get; set; }

    public float? ServiceComm { get; set; }

    public decimal? GmUcost { get; set; }

    public decimal? GmProdProfit { get; set; }

    public double? QtySold { get; set; }

    public decimal? LastyearSale { get; set; }

    public decimal? LastyearProfit { get; set; }

    public double? LastyearQtySold { get; set; }

    public string PackageSpec { get; set; }

    public decimal? SuggestSalePrice { get; set; }

    public int? PkLevel { get; set; }

    public string MasterProdNum { get; set; }

    public double? NumPerPack { get; set; }

    public string PackageSpec2 { get; set; }

    public int? PkFraction { get; set; }

    public int? PriceMode { get; set; }

    public string ConvUnit { get; set; }

    public int? ConvFactor { get; set; }

    public int? QtySale { get; set; }

    public int? QtySaleQty { get; set; }

    public double? QtySalePrice { get; set; }

    public string ModTimeStamp { get; set; }

    public int? ScaleTray { get; set; }

    public int? VolDisc { get; set; }

    public int? VolQty1 { get; set; }

    public double? VolPrice1 { get; set; }

    public int? VolQty2 { get; set; }

    public double? VolPrice2 { get; set; }

    public string Department { get; set; }

    public string LastOrdDate { get; set; }

    public double OnOrder { get; set; }

    public double OrdPoint { get; set; }

    public double? SuggestOrderQty { get; set; }

    public int? Inactive { get; set; }

    public string EnvDepLink { get; set; }

    public short? ExcludeOnRank { get; set; }

    public string QtyGroup { get; set; }

    public double? ProportionalTare { get; set; }

    public int? DeductBagWeight { get; set; }

    public double? BagWeight { get; set; }

    public double? MaxBuyQty { get; set; }

    public double? MaxOnSaleQty { get; set; }

    public short IsFood { get; set; }

    public string Source { get; set; }

    public string DeptCode { get; set; }

    public string Level2Code { get; set; }

    public string Level3Code { get; set; }

    public int StoreId { get; set; }

    public int? EthnicGroupId { get; set; }

    public Guid Rowguid { get; set; }
}
