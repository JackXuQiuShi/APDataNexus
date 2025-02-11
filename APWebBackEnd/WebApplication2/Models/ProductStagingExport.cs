using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductStagingExport
{
    public string PriceGroupName { get; set; }

    public string ProdNum { get; set; }

    public string ProdName { get; set; }

    public string ProdDesc { get; set; }

    public string Measure { get; set; }

    public string OnSales { get; set; }

    public string ProdAlias { get; set; }

    public string Tax1App { get; set; }

    public string Tax2App { get; set; }

    public string PackageSpec { get; set; }

    public string PriceMode { get; set; }

    public string QtySale { get; set; }

    public string QtySaleQty { get; set; }

    public string QtySalePrice { get; set; }

    public string ScaleTray { get; set; }

    public string Department { get; set; }

    public string Inactive { get; set; }

    public string QtyGroup { get; set; }

    public string MaxBuyQty { get; set; }

    public string MaxOnSaleQty { get; set; }

    public string IsFood { get; set; }

    public string RegPrice { get; set; }

    public string OnsalePrice { get; set; }

    public string DeptCode { get; set; }

    public string UnitsPerCase { get; set; }

    public string UnitCost { get; set; }

    public string VolumeCost { get; set; }

    public string MinVolume { get; set; }

    public string Ethnic { get; set; }

    public Guid Rowguid { get; set; }
}
