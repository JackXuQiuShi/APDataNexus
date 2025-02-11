﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APWeb.Models;

public partial class Products_Store
{
    [Column("Prod_Num")]
    public string ProductID { get; set; }

    [Column("prod_Name")]
    public string ProductName { get; set; }

    public string Prod_Desc { get; set; }

    public int? Service { get; set; }

    public decimal Unit_Cost { get; set; }

    public string Measure { get; set; }

    public string Warranty { get; set; }

    public decimal? Tot_Sales { get; set; }

    public decimal? Tot_Profit { get; set; }

    public int? OnSales { get; set; }

    public string Barcode { get; set; }

    public string Prod_Alias { get; set; }

    public int? Serial_Control { get; set; }

    public short? Tax1App { get; set; }

    public short? Tax2App { get; set; }

    public short? Tax3App { get; set; }

    public decimal? ItemBonus { get; set; }

    public int? SalePoint { get; set; }

    public float? ServiceComm { get; set; }

    public decimal? GM_UCost { get; set; }

    public decimal? GM_ProdProfit { get; set; }

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

    public string Last_Ord_Date { get; set; }

    public double On_Order { get; set; }

    public double Ord_Point { get; set; }

    public double? SuggestOrderQty { get; set; }

    public int? Inactive { get; set; }

    public string EnvDepLink { get; set; }

    public short? ExcludeOnRank { get; set; }

    public string QtyGroup { get; set; }

    public double? ProportionalTare { get; set; }

    public int? Deduct_Bag_Weight { get; set; }

    public double? Bag_Weight { get; set; }

    public double? MaxBuyQty { get; set; }

    public double? MaxOnSaleQty { get; set; }

    public short isFood { get; set; }

    public string Source { get; set; }

    public string DeptCode { get; set; }

    public string Level2Code { get; set; }

    public string Level3Code { get; set; }

    public int Store_ID { get; set; }

    public int? EthnicGroupID { get; set; }
}