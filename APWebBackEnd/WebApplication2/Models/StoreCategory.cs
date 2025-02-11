using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class StoreCategory
{
    public int StoreCategoryId { get; set; }

    public string StoreCategoryName { get; set; }

    public Guid Rowguid { get; set; }
}
