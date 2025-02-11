using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string CategoryChName { get; set; }

    public string Description { get; set; }

    public byte[] Picture { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public Guid Rowguid { get; set; }

    public virtual ICollection<TblSubInvoice> TblSubInvoices { get; set; } = new List<TblSubInvoice>();
}
