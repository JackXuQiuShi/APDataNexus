using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class PurchaseCostStatus
{
    public int StatusId { get; set; }

    public string StatusDescription { get; set; }

    public string StatusDescriptionChinese { get; set; }

    public Guid Rowguid { get; set; }
}
