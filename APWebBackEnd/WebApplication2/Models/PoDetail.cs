using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class PoDetail
{
    public string PoId { get; set; }

    public string ProductId { get; set; }

    public int UnitsOnPo { get; set; }

    public decimal? UnitPodraftPrice { get; set; }

    public float? Discount { get; set; }

    public bool Reviewed { get; set; }

    public bool PriceApproved { get; set; }

    public bool UnitsApproved { get; set; }

    public bool ReceivedStatus { get; set; }

    public bool Status { get; set; }

    public int UnitsReceived { get; set; }

    public DateTime? ReceivingDate { get; set; }

    public string ReceivedBy { get; set; }

    public float? PriceReceived { get; set; }

    public int? UnitsPerPackage { get; set; }

    public bool? Taxable { get; set; }

    public double? TaxRate { get; set; }

    public DateTime? OrderingDate { get; set; }

    public string OrderedBy { get; set; }

    public double? UnitsOrdered { get; set; }

    public double? PriceOrdered { get; set; }

    public int PaidItems { get; set; }

    public int FreeItems { get; set; }

    public double? Srprice { get; set; }

    public double? UnitsShipped { get; set; }

    public float? PriceShipped { get; set; }

    public bool? ShippedStatus { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? Confirm { get; set; }

    public int StoreId { get; set; }

    public int? TransactionId { get; set; }

    public Guid Rowguid { get; set; }
}
