using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class InventoryAction
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public string Action { get; set; }

    public bool Completed { get; set; }

    public DateTime? CompleteTime { get; set; }

    public string ProductId { get; set; }

    public double Quantity { get; set; }

    public double? Price { get; set; }

    public string CompleteUser { get; set; }

    public DateTime ActionTime { get; set; }

    public byte[] Signature { get; set; }

    public string PoId { get; set; }

    public string BatchId { get; set; }

    public string Destination { get; set; }

    public Guid Rowguid { get; set; }
}
