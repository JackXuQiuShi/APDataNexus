using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ProductPrice
{
    public string ProdNum { get; set; }

    public string Grade { get; set; }

    public decimal? RegPrice { get; set; }

    public decimal? BottomPrice { get; set; }

    public decimal? OnsalePrice { get; set; }

    public string ModTimeStamp { get; set; }

    public string Source { get; set; }

    public int StoreId { get; set; }

    public Guid Rowguid { get; set; }
}
