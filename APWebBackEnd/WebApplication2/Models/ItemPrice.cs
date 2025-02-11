using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ItemPrice
{
    public int ItemNbr { get; set; }

    public int StoreId { get; set; }

    public decimal? RegPrice { get; set; }

    public decimal? BottomPrice { get; set; }

    public decimal? OnsalePrice { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ModTimeStamp { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Item ItemNbrNavigation { get; set; }
}
