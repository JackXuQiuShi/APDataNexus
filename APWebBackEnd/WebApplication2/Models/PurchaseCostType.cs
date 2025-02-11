using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class PurchaseCostType
{
    public int CostTypeId { get; set; }

    public string CostTypeDescription { get; set; }

    public string CostTypeDescriptionChinese { get; set; }

    public Guid Rowguid { get; set; }
}
